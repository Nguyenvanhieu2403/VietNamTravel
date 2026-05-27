using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelVietnam.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        IQueryable<T> Query();
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
