using HotelListing.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.IRepository
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDTO apiUserDTO, string role);
        Task<AuthResponseDTO> Login(LoginUserDTO loginUserDTO);
        Task<string> CreateRefrshToken();
        Task<AuthResponseDTO> VerifyRefreshToken(AuthResponseDTO request);
    }
}
