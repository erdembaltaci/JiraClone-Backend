// Yer: JiraProject.DataAccess/Concrete/GenericRepository.cs
using JiraProject.Business.Abstract;
using JiraProject.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

// Bu sınıf, herhangi bir tablo (Entity) için standart veritabanı işlemlerini yapan "Uzman Aşçı"dır.
// <T> ifadesi, bu sınıfın Issue, Project, Status gibi herhangi bir tiple çalışabileceğini belirtir (Jenerik).
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    // Veritabanı ile konuşmak için kullanılacak DbContext nesnesi.
    protected readonly JiraProjectDbContext _context;

    // Bu repository'nin sorumlu olduğu veritabanı tablosunu (DbSet) temsil eder.
    // T, Issue ise bu _dbSet, Issues tablosu olur.
    private readonly DbSet<T> _dbSet;

    // Bu sınıf oluşturulduğunda, çalışması için bir veritabanı bağlantısına (context) ihtiyaç duyar.
    public GenericRepository(JiraProjectDbContext context)
    {
        _context = context;
        // Hangi tabloyla (T) çalışacağını DbContext üzerinden belirler.
        _dbSet = context.Set<T>();
    }

    // Verilen bir nesneyi (entity) veritabanına eklenmek üzere işaretler.
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    // Belirli bir koşula uyan tüm kayıtları bulur ve bir liste olarak döndürür.
    // Örnek: predicate = issue => issue.ProjectId == 5
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    // Tablodaki tüm kayıtları bir liste olarak döndürür.
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // Verilen Id'ye (Primary Key) sahip tek bir kaydı bulur ve döndürür.
    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    // Verilen bir nesneyi veritabanından silinmek üzere işaretler.
    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    // Verilen bir nesneyi veritabanında güncellenmek üzere işaretler.
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}