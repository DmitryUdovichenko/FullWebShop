using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shop.API.Dtos;

namespace Shop.API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,ILogger<BasketController> logger, IMapper mapper)
        {
            _logger = logger;
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasketById(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            return Ok(basket ?? new Basket(basketId));
        }

        [HttpPost]
        public async Task<ActionResult<Basket>> UpdateBasket(BasketDto basket)
        {
            var mapedBasket = _mapper.Map<BasketDto, Basket>(basket);
            var _basket = await _basketRepository.UpdateBasketAsync(mapedBasket);
            return _basket;
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}