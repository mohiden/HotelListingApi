namespace HotelListing.DTOs.User
{
    public class AuthResponseDTO
    {
        public string UserId { get; set; }
        public  string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
