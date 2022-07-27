using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Shop.API.Dtos;
using Shop.API.Errors;
using Shop.API.Helpers;

namespace Shop.API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<ProductType> _productTypeRepository;
        private readonly IBaseRepository<ProductBrand> _productBrandRepository;
        private readonly IMapper _mapper;

        public ProductController(IBaseRepository<Product> productRepository,
        IBaseRepository<ProductType> productTypeRepository,
        IBaseRepository<ProductBrand> productBrandRepository,
        IMapper mapper)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _productBrandRepository = productBrandRepository;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductReturnDto>>> Get([FromQuery] ProductParams productParams)
        {
            var spec = new ProductWithSpecification(productParams);
            var countSpec = new ProductWithFilters(productParams);
            var totalItems = await _productRepository.CountAsync(countSpec);
            var products = await _productRepository.GetListWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ProductReturnDto>>(products);
            return Ok(new Pagination<ProductReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductReturnDto>> Get(int id)
        {
            var spec = new ProductWithSpecification(id);

            var product = await _productRepository.GetEntityWithSpec(spec);

            if(product == null)
                return NotFound(new ApiResponse(404));
            
            return Ok(_mapper.Map<ProductReturnDto>(product));
            
        }

        [HttpPost]
        public bool Create([FromBody] string obj)
        {
            return true;
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepository.GetListAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepository.GetListAsync());
        }
    }
}
