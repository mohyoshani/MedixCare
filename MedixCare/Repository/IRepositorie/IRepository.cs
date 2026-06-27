using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Linq.Expressions;

namespace MedixCare.Repository.IRepositorie
{
    public interface IRepository<T> where T : class
    {
        void Delete(T entity);
        void Update(T entity);
        public Task CreateAsync(T entity, CancellationToken cancellationToken);
        public Task<T?> GetOneAsync(Expression<Func<T, bool>>? filter, CancellationToken cancellationToken = default, bool Tracked = true, Func<IQueryable<T>, IQueryable<T>>? includes = null);
        public Task<IEnumerable<T?>> GetAllAsync(Expression<Func<T, bool>>? filter , CancellationToken cancellationToken = default, bool Tracked = true, Func<IQueryable<T>, IQueryable<T>>? includes = null);
        public Task<int> CommitChangesAsync(CancellationToken cancellationToken = default);

        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}
