using JiraProject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraProject.DataAccess.Contexts
{
    // Bu sınıf, Entity Framework Core'un veritabanı ile iletişim kurmasını sağlayan ana köprüdür.
    // Veritabanı oturumu olarak da düşünülebilir.
    public class JiraProjectDbContext : DbContext
    {
        // Bu constructor, Program.cs dosyasından gelen veritabanı bağlantı ayarlarını (connection string gibi) alır
        // ve ana DbContext sınıfına ileterek bağlantıyı kurar.
        public JiraProjectDbContext(DbContextOptions<JiraProjectDbContext> options) : base(options)
        {
        }
        // DbContext sınıfı, Entity Framework Core'un veritabanı ile etkileşim kurmasını sağlar.
        // Bu DbSet'ler, veritabanında oluşacak tabloları temsil eder.
        // Her bir DbSet, bir C# sınıfını bir veritabanı tablosuyla eşleştirir.
        public DbSet<Project> Projects { get; set; } // Projects tablosunu Project sınıfıyla eşleştirir.
        public DbSet<Issue> Issues { get; set; }     // Issues tablosunu Issue sınıfıyla eşleştirir.
        public DbSet<User> Users { get; set; }       // Users tablosunu User sınıfıyla eşleştirir.
    }
}