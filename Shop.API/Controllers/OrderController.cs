using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.API.Dtos;
using Shop.API.Errors;
using Shop.API.Extensions;

namespace Shop.API.Controllers
{
    [Authorize]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User?.RetriveEmailFromPrincipal();
            var address = _mapper.Map<AddressDto, Address>(orderDto.Address);
            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);
            if(order == null) return BadRequest(new ApiResponse(400,"Order not created"));
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderResponseDto>>> GetUserOrders()
        {
            var email = HttpContext.User.RetriveEmailFromPrincipal();
            var orders = await _orderService.GetUserOrdersAsync(email);
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderResponseDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
        {
            var email = HttpContext.User.RetriveEmailFromPrincipal();
            var order = await _orderService.GetOrderByIdAsync(id,email);
            if(order == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Order,OrderResponseDto>(order);
        }

        [HttpGet("deliverymethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }
}