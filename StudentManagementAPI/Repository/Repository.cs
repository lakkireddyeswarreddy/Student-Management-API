using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.ApplicationDbContext;
using StudentManagementAPI.RepositoryInterface;

namespace StudentManagementAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Add(T item)
        {
            _appDbContext.Set<T>().Add(item);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<ICollection<T>> GetAll()
        {
            return await _appDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(long id)
        {
            return await _appDbContext.Set<T>().FindAsync(id);
        }

        public async Task Remove(long id)
        {
            var item = await GetById(id);

            _appDbContext.Set<T>().Remove(item);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Update(T item)
        {
            _appDbContext.Set<T>().Update(item);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
