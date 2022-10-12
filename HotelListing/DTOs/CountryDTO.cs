using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs
{
    public class CreateCountryDTO
    {
        [Required]
        [StringLength(maximumLength:50, ErrorMessage = "Country name length must be less than 50 char")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength:2, ErrorMessage = "Short name must be 2 char max")]
        public string ShortName { get; set; }
    }
    public class CountryDTO : CreateCountryDTO
    {
        public int Id { get; set; }
        public virtual IList<HotelDTO> Hotels { get; set; }
    }
}
