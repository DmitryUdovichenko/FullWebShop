using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Shop.API.Dtos;

namespace Shop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
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
        public async Task<ActionResult<IReadOnlyList<ProductReturnDto>>> Get()
        {
            var spec = new ProductWithSpecification();
            var products = await _productRepository.GetListWithSpecAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<ProductReturnDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReturnDto>> Get(int id)
        {
            var spec = new ProductWithSpecification(id);
            var product = await _productRepository.GetEntityWithSpec(spec);
            if(product == null)
                return NotFound();
            
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
