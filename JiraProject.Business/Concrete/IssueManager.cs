namespace JiraProject.Business.Concrete
{
    using JiraProject.Business.Abstract;
    using JiraProject.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    // IssueManager, IIssueService sözleşmesine uymak zorundadır.
    // Bu sınıf, Issue (Görev) ile ilgili tüm işlemleri yöneten "Issue Ustası"dır.
    // Bu sınıf, IIssueService arayüzünü uygulayarak, görevlerle ilgili
    // ekleme, güncelleme, silme ve listeleme gibi işlemleri gerçekleştirir.
    public class IssueManager : IIssueService
    {
        private readonly IUnitOfWork _unitOfWork;

        // Bu işleme 'Dependency Injection' diyoruz.
        public IssueManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateIssueAsync(Issue issue)
        {
            issue.CreatedAt = DateTime.UtcNow;
            await _unitOfWork.Issues.AddAsync(issue);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Issue>> GetAllIssuesAsync()
        {
            // Unit of Work üzerinden Issue Repository'sine ulaşıp tüm görevleri alıyoruz.
            return await _unitOfWork.Issues.GetAllAsync();
        }

        public async Task<Issue> GetIssueByIdAsync(int id)
        {
            return await _unitOfWork.Issues.GetByIdAsync(id);
        }

        public async Task UpdateIssueAsync(Issue issue)
        {
            // 1. Önce veritabanındaki mevcut kaydı ID'sine göre bul.
            //    Bu işlem, EF Core'un bu kaydı "takip etmeye" başlamasını sağlar.
            var issueFromDb = await _unitOfWork.Issues.GetByIdAsync(issue.Id);

            // 2. Eğer kayıt bulunduysa, özelliklerini gelen yeni verilerle güncelle.
            if (issueFromDb != null)
            {
                // Gelen yeni verileri, veritabanından çektiğimiz nesnenin üzerine yazıyoruz.
                issueFromDb.Title = issue.Title;
                issueFromDb.Description = issue.Description;
                issueFromDb.Status = issue.Status;
                issueFromDb.Order = issue.Order;
                issueFromDb.UpdatedAt = DateTime.UtcNow; // Güncellenme tarihini ayarla

                // 3. Değişiklikleri kaydet. EF Core, issueFromDb'nin
                //    değiştiğini zaten bildiği için doğru UPDATE komutunu oluşturur.
                //    Burada _unitOfWork.Issues.Update() dememize GEREK YOKTUR.
                await _unitOfWork.CompleteAsync();
            }
            // (Eğer issueFromDb null ise, bir 'bulunamadı' hatası fırlatılabilir.
            //  Bu, ilerideki hata yönetimi konumuz.)
        }

        public async Task DeleteIssueAsync(int id)
        {
            // Önce silinecek görevi Id'sine göre veritabanında buluyoruz.
            var issueToDelete = await _unitOfWork.Issues.GetByIdAsync(id);

            // Eğer görev bulunursa...
            if (issueToDelete != null)
            {
                // Unit of Work üzerinden "Bu görevi sil" emrini veriyoruz.
                _unitOfWork.Issues.Remove(issueToDelete);
                await _unitOfWork.CompleteAsync(); // Değişikliği kaydet
            }
        }

        public Task<IEnumerable<string>> GetToDoIssuesUpperCaseTitlesAsync()
        {
            throw new NotImplementedException();
        }
    }
}