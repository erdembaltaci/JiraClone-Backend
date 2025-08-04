// Yer: JiraProject.Business/Abstract/IIssueService.cs
using JiraProject.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiraProject.Business.Abstract
{
    /// <summary>
    /// Issue (Görev) nesneleriyle ilgili tüm iş mantığı operasyonlarının
    /// sözleşmesini tanımlar. IssueManager bu kurallara uymak zorundadır.
    /// </summary>
    public interface IIssueService
    {
        /// <summary>
        /// Veritabanındaki tüm görevleri getirir.
        /// </summary>
        Task<IEnumerable<Issue>> GetAllIssuesAsync();

        /// <summary>
        /// Verilen ID'ye sahip tek bir görevi getirir.
        /// </summary>
        Task<Issue> GetIssueByIdAsync(int id);

        /// <summary>
        /// Yeni bir görev oluşturur.
        /// </summary>
        Task CreateIssueAsync(Issue issue);

        /// <summary>
        /// Mevcut bir görevi günceller.
        /// </summary>
        Task UpdateIssueAsync(Issue issue);

        /// <summary>
        /// Verilen ID'ye sahip bir görevi siler.
        /// </summary>
        Task DeleteIssueAsync(int id);

        /// <summary>
        /// LINQ pratiği için yazdığımız, sadece 'To Do' statüsündeki
        /// görevlerin başlıklarını büyük harfle getiren metot.
        /// </summary>
        Task<IEnumerable<string>> GetToDoIssuesUpperCaseTitlesAsync();
    }
}