using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs
{
    public class CreateHotelDTO
    {
        [Required] 
        [StringLength(maximumLength:150,ErrorMessage ="Hotel name must be less than 150")]
        public string Name { get; set; }

        [Required] 
        [StringLength(maximumLength:250,ErrorMessage ="Address must be less than 250")]
        public string Address { get; set; }

        [Required]
        [Range(0,5)]
        public double Rating { get; set; }
        [Required]
        public int CountryId { get; set; }
    }

    public class HotelDTO : CreateHotelDTO
    {
        public int Id { get; set; }
        public CountryDTO Country { get; set; }
    }
}
