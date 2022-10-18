using AutoMapper;
using HotelListing.Data;
using HotelListing.DTOs;
using HotelListing.DTOs.Country;
using HotelListing.DTOs.Hotel;
using HotelListing.DTOs.User;

namespace HotelListing.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country,CreateCountryDTO>().ReverseMap();
            CreateMap<Country,GetCountryDTO>().ReverseMap();
            CreateMap<Country,CountryDTO>().ReverseMap();
            CreateMap<Country,UpdateCountryDTO>().ReverseMap();

            CreateMap<Hotel,HotelDTO>().ReverseMap();
            CreateMap<Hotel,CreateHotelDTO>().ReverseMap();


            CreateMap<ApiUserDTO,ApiUser>().ReverseMap();
            CreateMap<LoginUserDTO,ApiUser>().ReverseMap();
        }
    }
}
