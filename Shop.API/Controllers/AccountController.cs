using System.Security.Claims;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.API.Dtos;
using Shop.API.Errors;
using Shop.API.Extensions;

namespace Shop.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser(){

            var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };

        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistedAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet("address"), Authorize]
        public async Task<ActionResult<AddressDto>> GetAdress()
        {
            var user = await _userManager.FindWithAddressAsync(HttpContext.User);
            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [HttpPut("address"), Authorize]
        public async Task<ActionResult<AddressDto>> UpdateAdress(AddressDto address)
        {
            var user = await _userManager.FindWithAddressAsync(HttpContext.User);
            user.Address = _mapper.Map<AddressDto, Address>(address);
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));

            return BadRequest("User upd problems");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if(user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if(!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };

        }

        [HttpPost("registration")]
        public async Task<ActionResult<UserDto>> Registration(RegistrationDto registrationUser)
        {
            if(CheckEmailExistedAsync(registrationUser.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors= new []{"Email addres is in use"}});
            }

            var user = new User
            {
                DisplayName = registrationUser.DisplayName,
                Email = registrationUser.Email,
                UserName = registrationUser.Email
            };

            var result = await _userManager.CreateAsync(user, registrationUser.Password);

            if(!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }
    }
}