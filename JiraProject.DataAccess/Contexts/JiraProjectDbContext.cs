using JiraProject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace JiraProject.DataAccess.Contexts
{
    public class JiraProjectDbContext : DbContext
    {
        public JiraProjectDbContext(DbContextOptions<JiraProjectDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base metodu en başta bir kere çağırmak yeterlidir.
            base.OnModelCreating(modelBuilder);

            // --- İLİŞKİ YAPILANDIRMALARI ---

            // User-Team için Çoka-Çok (Many-to-Many) ilişkisi
            // 1. Ara tablonun birleşik anahtarını tanımla (SADECE BİR KERE)
            modelBuilder.Entity<UserTeam>()
                .HasKey(ut => new { ut.UserId, ut.TeamId });

            // 2. User -> UserTeam ilişkisi (isteğe bağlı, EF Core bunu kendi de bulabilir)
            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTeams)
                .HasForeignKey(ut => ut.UserId);

            // 3. Team -> UserTeam ilişkisi (isteğe bağlı)
            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.Team)
                .WithMany(t => t.UserTeams)
                .HasForeignKey(ut => ut.TeamId);

            // Team -> TeamLead (User) ilişkisi
            modelBuilder.Entity<Team>()
                .HasOne(t => t.TeamLead)
                .WithMany() // Bir user birden çok takımın lideri olabilir
                .HasForeignKey(t => t.TeamLeadId)
                .OnDelete(DeleteBehavior.Restrict); // Lider olan user silinirse hata ver, takımı silme.

            // Issue -> Reporter (User) ilişkisi (EKLENMESİ GEREKEN ÖNEMLİ YAPI)
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Reporter)
                .WithMany() // Bir user birden çok görevi raporlayabilir
                .HasForeignKey(i => i.ReporterId)
                .OnDelete(DeleteBehavior.Restrict); // Raporlayan user silinirse hata ver.

            // Not: Issue -> Assignee ilişkisi nullable olduğu için EF Core genellikle
            // onu otomatik olarak doğru yapılandırır, ama istersen onu da ekleyebilirsin:
            // modelBuilder.Entity<Issue>()
            //     .HasOne(i => i.Assignee)
            //     .WithMany(u => u.AssignedIssues)
            //     .HasForeignKey(i => i.AssigneeId)
            //     .OnDelete(DeleteBehavior.SetNull); // Atanan user silinirse AssigneeId'yi null yap.


            // --- OTOMATİK ALAN YAPILANDIRMALARI ---

            // BaseEntity'den miras alan tüm sınıflar için CreatedAt ve UpdatedAt ayarları
            var entityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType));

            foreach (var entityType in entityTypes)
            {
                modelBuilder.Entity(entityType.Name)
                    .Property<DateTime>("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");

                modelBuilder.Entity(entityType.Name)
                    .Property<DateTime>("UpdatedAt")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("GETUTCDATE()");
            }

            // --- İSTEĞE BAĞLI: ENUM'ları Veritabanına Metin Olarak Kaydetme ---
            // modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
            // modelBuilder.Entity<Issue>().Property(i => i.Status).HasConversion<string>();
            // modelBuilder.Entity<Issue>().Property(i => i.Priority).HasConversion<string>();
        }
    }
}