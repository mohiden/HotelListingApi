using AutoMapper;
using HotelListing.Data;
using HotelListing.DTOs.User;
using HotelListing.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private  ApiUser _user;

        public  const string _loginProvider = "HotelListingApi";
        private const string _refreshToken = "RefreshToken";


        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> CreateRefrshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
             var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
            var result = await _userManager.SetAuthenticationTokenAsync(_user,_loginProvider,_refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        public async Task<AuthResponseDTO> Login(LoginUserDTO loginUserDTO)
        {
            _user  = await _userManager.FindByEmailAsync(loginUserDTO.Email);
            bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginUserDTO.Password);
            if(_user == null || isValidUser == false)
            {
                return null;
            }
            var token = await GenerateToken();
            return new AuthResponseDTO
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefrshToken()
            };
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDTO apiUserDTO, string role)
        {
            _user = _mapper.Map<ApiUser>(apiUserDTO);

            _user.UserName = apiUserDTO.Email;

            var result = await _userManager.CreateAsync(_user,apiUserDTO.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, role);
            }
            return result.Errors;

        }

        public async Task<AuthResponseDTO> VerifyRefreshToken(AuthResponseDTO request)
        {


                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
                var email = tokenContent.Claims.ToList().FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
                _user = await _userManager.FindByEmailAsync(email);
            if(_user == null || _user.Id != request.UserId)
            {
                return null;
            }
            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.Token);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDTO
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefrshToken(),
                };
            }
            await _userManager.UpdateSecurityStampAsync(_user);
            return null;


        }

        private async Task<string> GenerateToken()
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey,  SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(_user);

     
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
            }.Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
               signingCredentials: credentials,
               claims: claims,
            expires:DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"]))
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

            
        }
    }
}
