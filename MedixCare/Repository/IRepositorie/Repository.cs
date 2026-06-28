using MedixCare.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedixCare.Repository.IRepositorie
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger _logger;
        public Repository(ApplicationDbContext context, ILogger<Repository<T>> logger)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _logger = logger;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                _dbSet.Add(entity);
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                _dbSet.Remove(entity);
            }
        }
        public async Task<int> CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes");
                return 0;
            }
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }


        public async Task<T?> GetOneAsync(Expression<Func<T, bool>>? filter, CancellationToken cancellationToken = default, bool Tracked = true, Func<IQueryable<T>, IQueryable<T>>? includes = null)
        {
            var query = _dbSet.AsQueryable();
            if (filter is not null)
            {
                query = query.Where(filter);
            }
            if (includes is not null)
            {
                query = includes(query);
            }
            if (!Tracked)
            {
                query = query.AsNoTracking();
            }
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<IEnumerable<T?>> GetAllAsync(Expression<Func<T, bool>>? filter, CancellationToken cancellationToken, bool Tracked, Func<IQueryable<T>, IQueryable<T>>? includes)
        {
            var query = _dbSet.AsQueryable();
            if (filter is not null)
            {
                query = query.Where(filter);
            }
            if (includes is not null)
            {
                query = includes(query);
            }
            if (!Tracked)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync(cancellationToken);
        }
    }
}

