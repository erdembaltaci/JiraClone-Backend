using JiraProject.Business.Abstract;
using JiraProject.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly JiraProjectDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(JiraProjectDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync(); 
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        // FindAsync, primary key'e göre arama yapmak için en verimli yoldur.
        return await _dbSet.FindAsync(id);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }


    public async Task<T?> GetByIdWithIncludesAsync(int id, params string[] includeStrings) 
    {
        IQueryable<T> query = _dbSet; 
        foreach (var include in includeStrings)
        {
            query = query.Include(include);
        }
        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params string[] includeStrings)
    {
        IQueryable<T> query = _dbSet;
        foreach (var include in includeStrings)
        {
            query = query.Include(include);
        }
        return await query.ToListAsync();
    }
}