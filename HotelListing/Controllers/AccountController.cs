using AutoMapper;
using HotelListing.Data;
using HotelListing.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        //private readonly SignInManager<ApiUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApiUser> userManager,ILogger<AccountController> logger, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            //_signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
           public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attemp for {userDTO.Email} and {userDTO.Password}");
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = _mapper.Map<ApiUser>(userDTO); 
                user.UserName = userDTO.Email;
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                { 
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return StatusCode(500,"Internal server error, try again later.");
            }
        }

        //[HttpPost]
        //[Route("login")]
        //   public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        //{
        //    _logger.LogInformation($"Login Attemp for {loginUserDTO.Email} and {loginUserDTO.Password}");
        //    if(!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {
        //        var result =await  _signInManager.PasswordSignInAsync(loginUserDTO.Email, loginUserDTO.Password, false, false);

        //        if (!result.Succeeded)
        //        {
        //            return Unauthorized(loginUserDTO);
        //        }
        //        return Accepted(); 
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Something went wrong in the {nameof(Login)}");
        //        return StatusCode(500,"Internal server error, try again later.");
        //    }
        //}

    }
}
