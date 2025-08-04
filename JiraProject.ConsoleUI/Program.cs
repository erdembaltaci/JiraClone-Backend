using JiraProject.Business.Abstract;
using JiraProject.Business.Concrete;
using JiraProject.DataAccess.Concrete;
using JiraProject.DataAccess.Contexts;
using JiraProject.Entities;     
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Bu kod, JiraProject uygulamasının konsol arayüzünü başlatır ve gerekli bağımlılıkları yapılandırır.
// Program.cs dosyası, uygulamanın başlangıç noktasını temsil eder.
// Burada, Entity Framework Core kullanarak veritabanı bağlantısını ayarlar,
// gerekli servisleri (repository, unit of work, service) ekler ve uygulamayı çalıştırır.
// Bu kod, Dependency Injection (DI) kullanarak uygulamanın bağımlılıklarını yönetir.
// Bu sayede, uygulama modüler ve test edilebilir hale gelir.

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<JiraProjectDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Bağımlılıkları ekliyoru<
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IIssueService, IssueManager>();
        services.AddScoped<IProjectService, ProjectManager>();
        services.AddScoped<IUserService, UserManager>();
    })
    .Build();


// Uygulamayı başlatıyoruz
await RunAppAsync(host);

// RunAppAsync metodu, uygulamanın ana işlevselliğini içerir.
static async Task RunAppAsync(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            // --- DEPENDENCY INJECTION ÖRNEĞİ ---
            // Sisteme daha önce tanıttığımız servisleri burada talep ediyoruz.
            var issueService = services.GetRequiredService<IIssueService>();

            Console.WriteLine("--- Test Başlıyor ---");

            // --- CRUD - DELETE (Silme) ÖRNEĞİ ---
            // Her testin başında veritabanını temizliyoruz.
            var allIssues = await issueService.GetAllIssuesAsync();
            foreach (var issue in allIssues)
            {
                await issueService.DeleteIssueAsync(issue.Id);
            }
            Console.WriteLine("Mevcut görevler temizlendi.");


            // --- CRUD - CREATE (Oluşturma) ÖRNEĞİ ---
            Console.WriteLine("\n--- Adım 1: Create ---");
            var newIssue = new JiraProject.Entities.Issue
            {
                Title = "Ödeme Sayfası Butonu",
                Order = 1,
                ProjectId = 1,
                Status = JiraProject.Entities.TaskStatus.ToDo // Enum kullanıyoruz
            };
            await issueService.CreateIssueAsync(newIssue);
            Console.WriteLine($"Oluşturuldu: ID: {newIssue.Id}, Başlık: {newIssue.Title}");


            // --- CRUD - UPDATE (Güncelleme) ÖRNEĞİ ---
            Console.WriteLine("\n--- Adım 2: Update ---");
            newIssue.Title = "ÖDEME SAYFASI BUTONU - GÜNCELLENDİ";
            await issueService.UpdateIssueAsync(newIssue);


            // --- CRUD - READ (Okuma) ÖRNEĞİ ---
            var updatedIssue = await issueService.GetIssueByIdAsync(newIssue.Id);
            Console.WriteLine($"Güncellendi: ID: {updatedIssue.Id}, Başlık: {updatedIssue.Title}");


            // --- LINQ, ASYNC/AWAIT ve TASK ÖRNEĞİ ---
            // Yukarıdaki tüm 'await' kelimeleri, veritabanı işlemlerini
            // asenkron olarak beklediğimizi gösterir. Metotların dönüş tipleri "Task"tır.
            //Console.WriteLine("\n--- LINQ Testi: 'To Do' Başlıkları ---");
            //var todoTitles = await issueService.GetToDoIssuesUpperCaseTitlesAsync(); // .Where ve .Select bu metodun içinde çalıştı

            //foreach (var title in todoTitles)
            //{
            //    Console.WriteLine(title);
            //}
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Bir hata oluştu: {ex.Message}");
        }
    }
}