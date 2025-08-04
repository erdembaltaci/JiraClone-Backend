namespace JiraProject.DataAccess.Concrete
{
    // Yer: JiraProject.DataAccess/Concrete/UnitOfWork.cs
    using JiraProject.Business.Abstract;
    using JiraProject.DataAccess.Contexts;
    using JiraProject.Entities;
    using System.Threading.Tasks;

    // Bu sınıf, IUnitOfWork arayüzündeki sözleşmeyi uygulayan somut sınıftır.
    // Tüm repository'leri yöneten ve veritabanı işlemlerinin bütünlüğünü sağlayan "Mutfak Şefi"dir.
    public class UnitOfWork : IUnitOfWork
    {
        // Veritabanı ile doğrudan iletişim kuracak olan DbContext nesnesi.
        private readonly JiraProjectDbContext _context;

        // Dışarıya açılan ve her bir tablo için özel repository'leri temsil eden özellikler.
        // Bu sayede Business katmanı, "unitOfWork.Issues" diyerek doğrudan Görev ustasına ulaşabilir.
        public IGenericRepository<Issue> Issues { get; private set; }
        public IGenericRepository<Project> Projects { get; private set; }
        public IGenericRepository<User> Users { get; private set; }

        // UnitOfWork (Mutfak Şefi) oluşturulduğunda, çalışması için bir veritabanı bağlantısına (context) ihtiyaç duyar.
        public UnitOfWork(JiraProjectDbContext context)
        {
            _context = context;

            // Mutfak Şefi, emrindeki tüm uzman aşçıları (repository'leri) göreve hazırlar.
            // Her bir repository'ye, çalışmaları için veritabanı bağlantısını (_context) verir.
            Issues = new GenericRepository<Issue>(_context);
            Projects = new GenericRepository<Project>(_context);
            Users = new GenericRepository<User>(_context);
        }

        // Bu metot, o ana kadar yapılan tüm değişiklikleri (Ekleme, Güncelleme, Silme)
        // tek bir işlem (transaction) olarak veritabanına kaydeder.
        public async Task<int> CompleteAsync()
        {
            // Değişiklikleri kaydetmek için DbContext'in SaveChangesAsync metodunu çağırır.
            return await _context.SaveChangesAsync();
        }

        // UnitOfWork nesnesi ile işimiz bittiğinde, veritabanı bağlantısı gibi
        // yönetilen kaynakları serbest bırakmak için bu metot çağrılır.
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}