using Core.Entities;
using Core.Interfaces;
using Order = Core.Entities.OrderAggregate.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.API.Errors;
using Stripe;
using Core.Entities.OrderAggregate;

namespace Shop.API.Controllers
{
    public class PaymentController : BaseApiController
    {
        public IPaymentService _paymentService;
        private const string endpointSecret = "whsec_946c706b4e151777c6db509f669d01924aa64a06b911a6182a4ec73a4c407b78";
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
            
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<Basket>> SetPaymentIntent(string basketId)
        {
            var basket =  await _paymentService.SetPaymentIntent(basketId);
            if(basket == null ) return BadRequest(new ApiResponse(400, "Basket problem"));

            return basket;
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(); 
            Order order;
            PaymentIntent paymentIntent;
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    paymentIntent = (PaymentIntent) stripeEvent.Data.Object;
                    order = await _paymentService.UpdateOrder(paymentIntent.Id, OrderStatus.PaymentFailed);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    paymentIntent = (PaymentIntent) stripeEvent.Data.Object;
                    order = await _paymentService.UpdateOrder(paymentIntent.Id, OrderStatus.PaymentRecived);
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}