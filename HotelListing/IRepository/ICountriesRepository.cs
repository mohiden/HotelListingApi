using HotelListing.Data;

namespace HotelListing.IRepository
{
    public interface ICountriesRepository:IGenericRepository<Country>
    {
         Task<Country> GetDetails(int id);
    }
}
