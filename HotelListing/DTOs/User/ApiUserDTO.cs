using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs.User
{
    public class ApiUserDTO: LoginUserDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        
    }
}
