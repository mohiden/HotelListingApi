using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs.User
{
    public class LoginUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your password is limited to {2} to {1}",MinimumLength = 6)]
        public string Password { get; set; }

    }
}
