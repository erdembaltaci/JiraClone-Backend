using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JiraProject.Entities
{
    public class User : BaseEntity
    {
        [Required] // -> required = not null 
        [MaxLength(50)] // Veritabanında nvarchar(50) olarak ayarlanır
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(100)] // Veritabanında nvarchar(100) olarak ayarlanır
        public string Email { get; set; } = null!;

        // --- GÜVENLİK İÇİN EKLENEN ALANLAR ---
        [Required]
        // Artık string olarak saklıyoruz, çünkü BCrypt bize string veriyor.
        public string PasswordHash { get; set; } = null!;

        // --- KULLANILABİLİRLİK İÇİN EKLENEN ALANLAR ---
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        // --- İLİŞKİLER (Navigation Properties) ---
        public ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>(); // Kullanıcının üye olduğu takımlar
        public ICollection<Issue> AssignedIssues { get; set; } = new List<Issue>(); // Kullanıcının atandığı görevler
    }
}