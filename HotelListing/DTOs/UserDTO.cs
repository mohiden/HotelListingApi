using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs
{
    public class LoginUserDTO
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your password is limited to {2} to {1}",MinimumLength = 5)]
        public string Password { get; set; }
    }
    public class UserDTO : LoginUserDTO
    {
        public string FirstNme { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }



    }
}
