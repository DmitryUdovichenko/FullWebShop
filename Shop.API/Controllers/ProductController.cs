using System.Security.Claims;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.API.Dtos;
using Shop.API.Errors;
using Shop.API.Helpers;

namespace Shop.API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploader;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileUploadService fileUploader,
        IUserIdProvider userIdProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileUploader = fileUploader;
            _userIdProvider = userIdProvider;
        }

        //[Caching(3600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductReturnDto>>> Get([FromQuery] ProductParams productParams)
        {
            var spec = new ProductWithSpecification(productParams);
            var countSpec = new ProductWithFilters(productParams);
            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);
            var products = await _unitOfWork.Repository<Product>().GetListWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ProductReturnDto>>(products);
            return Ok(new Pagination<ProductReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        //[Caching(3600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductReturnDto>> Get(int id)
        {
            var spec = new ProductWithSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);
            if(product == null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<ProductReturnDto>(product));
            
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("admin-list")]
        public async Task<ActionResult<Pagination<ProductReturnDto>>> GetAdmin([FromQuery] ProductParams productParams)
        {
            var spec = new ProductWithSpecification(_userIdProvider.UserId);
            var countSpec = new ProductWithFilters(_userIdProvider.UserId);
            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);
            var products = await _unitOfWork.Repository<Product>().GetListWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ProductReturnDto>>(products);
            return Ok(new Pagination<ProductReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductReturnDto>> Create([FromForm] ProductCreateDto prouctDto)
        {
            var product = _mapper.Map<Product>(prouctDto);
            var imagepath = await _fileUploader.UploadFile(prouctDto.Image);
            product.ImageUrl = imagepath;
            _unitOfWork.Repository<Product>().Add(product);
            var result = await _unitOfWork.Complete();

           if (result <= 0) return BadRequest(new ApiResponse(400, "Wrong product data"));

           return Ok(product);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            _unitOfWork.Repository<Product>().Delete(product);
            var result =await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Deletion Failed"));
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductReturnDto>> Update(int id, ProductCreateDto productDto)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

            _mapper.Map(productDto,product);

            _unitOfWork.Repository<Product>().Update(product);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Failed updating product"));

            return Ok(product);
        }

        [Caching(3600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _unitOfWork.Repository<ProductBrand>().GetListAsync());
        }

        [Caching(3600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _unitOfWork.Repository<ProductType>().GetListAsync());
        }
    }
}
