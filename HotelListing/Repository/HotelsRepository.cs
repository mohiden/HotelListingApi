using HotelListing.Data;
using HotelListing.IRepository;

namespace HotelListing.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        public HotelsRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
