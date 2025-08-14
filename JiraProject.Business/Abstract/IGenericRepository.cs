// Yer: JiraProject.Business/Abstract/IGenericRepository.cs
using System.Linq.Expressions;

namespace JiraProject.Business.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        // Temel metotlar aynı kalıyor
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);

        // İlişkili verileri getirmek için güncellenmiş metotlar (string tabanlı)
        Task<T?> GetByIdWithIncludesAsync(int id, params string[] includeStrings);
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params string[] includeStrings);
    }
}