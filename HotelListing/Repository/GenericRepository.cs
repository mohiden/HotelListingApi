using HotelListing.Data;
using HotelListing.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DatabaseContext _context;
        //private readonly DbSet<T> _db;
        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            //_db = _context.Set<T>();
        }
        //public async Task Delete(int id)
        //{
        //    var entity = await _db.FindAsync(id);
        //     _db.Remove(entity);

        //}

        //public void DeleteRange(IEnumerable<T> entities)
        //{
        //    _db.RemoveRange(entities);
        //}

        //public async Task<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> expression, List<string> includes = null)
        //{
        //    IQueryable<T> query = _db;

        //    if(includes != null)
        //    {
        //        foreach(var includeProperty in includes)
        //        {
        //            query = query.Include(includeProperty);
        //        }
        //    }
        //    return await query.AsNoTracking().FirstOrDefaultAsync(expression);

        //}

        //public async Task<IList<T>> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        //{

        //    IQueryable<T> query = _db;

        //    if (expression != null)
        //    {
        //        query = query.Where(expression);
        //    }

        //    if(includes != null)
        //    {
        //        foreach(var includeProperty in includes)
        //        {
        //            query = query.Include(includeProperty);
        //        }
        //    }

        //    if (orderBy != null)
        //    {
        //        query = orderBy(query);
        //    }

        //    return await query.AsNoTracking().ToListAsync();

        //}

        //public async Task Insert(T entity)
        //{
        //    await _db.AddAsync(entity);
        //}

        //public async Task InsertRange(IEnumerable<T> entities)
        //{
        //    await _db.AddRangeAsync(entities);
        //}

        //public void Update(T entity)
        //{
        //    _db.Attach(entity);
        //    _context.Entry(entity).State = EntityState.Modified;

        //}
        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
            if (id is null)
            {
                return null;
            }
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
