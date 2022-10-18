using AutoMapper;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Mvc;
using HotelListing.DTOs.User;
using Microsoft.AspNetCore.Authorization;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthManager authManager,ILogger<AccountController> logger)
        {
            _authManager = authManager;
            _logger = logger;
        }
        //POST: api/account/register/admin
        [HttpPost]
        [Route("register/admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
           public async Task<IActionResult> RegisterAdmin([FromBody] ApiUserDTO userDTO)
        {
            var errors = await _authManager.Register(userDTO, "Admin");
            if(errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok(userDTO);

        }


        //POST: api/account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
           public async Task<IActionResult> Register([FromBody] ApiUserDTO userDTO)
        {
            _logger.LogInformation($"Attemp to register {userDTO.Email}");

            try
            {

            var errors = await _authManager.Register(userDTO, "User");
            if(errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok(userDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Something went wrong in ${nameof(Register)} - user register attemp with email ${userDTO.Email}");
                return Problem($"Something Went Wrong in the {nameof(Register)}",statusCode:500);
            }

        }

        //POST: api/account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
           public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            var authResponse = await _authManager.Login(userDTO);
            if (authResponse == null)
            {
                return Unauthorized();
            }
            return Ok(authResponse);

        }

        //POST: api/account/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
           public async Task<IActionResult> RefreshToken([FromBody] AuthResponseDTO request)
        {
            var authResponse = await _authManager.VerifyRefreshToken(request);
            if (authResponse == null)
            {
                return Unauthorized();
            }
            return Ok(authResponse);

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
