// Yer: JiraProject.Business/Abstract/IGenericRepository.cs
using System.Linq.Expressions;

namespace JiraProject.Business.Abstract
{
    // Generic repository arayüzü, temel CRUD (Create, Read, Update, Delete) işlemlerini
    // ve sorgulama işlemlerini destekler. Bu arayüz, herhangi bir Entity tipi için
    // genel veritabanı işlemlerini gerçekleştirmek üzere tasarlanmıştır.
  
    public interface IGenericRepository<T> where T : class
    {
        // Verilen Id'ye sahip bir kaydı asenkron olarak getirir.
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}