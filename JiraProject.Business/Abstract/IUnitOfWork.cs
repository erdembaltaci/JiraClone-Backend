using JiraProject.Entities;
using System;

namespace JiraProject.Business.Abstract
{
   
    /// Tüm repository'leri bir arada tutan ve veritabanı işlemlerini
    /// tek bir transaction (işlem bütünlüğü) içinde yöneten yapının sözleşmesi.
    /// Lokanta analojisindeki "Mutfak Şefi"nin görev tanımıdır.
   
    public interface IUnitOfWork : IDisposable
    {
       
        /// Issue (Görev) verileri üzerinde işlem yapacak olan repository'ye erişim sağlar.
       
        IGenericRepository<Issue> Issues { get; }

       
        /// Project (Proje) verileri üzerinde işlem yapacak olan repository'ye erişim sağlar.
      
        IGenericRepository<Project> Projects { get; }

        /// User (Kullanıcı) verileri üzerinde işlem yapacak olan repository'ye erişim sağlar.

        IGenericRepository<User> Users { get; }


        /// O ana kadar yapılan tüm değişiklikleri (ekleme, güncelleme, silme)
        /// veritabanına tek bir işlem olarak kaydeder.

        /// <returns>Veritabanında etkilenen satır sayısı.</returns>
        Task<int> CompleteAsync();
    }
}