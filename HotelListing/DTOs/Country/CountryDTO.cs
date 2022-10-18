using HotelListing.DTOs.Hotel;

namespace HotelListing.DTOs.Country
{
    public class CountryDTO : GetCountryDTO
    {
        public List<HotelDTO> Hotels { get; set; }
    }

    
}
