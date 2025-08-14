using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JiraProject.Entities
{
    public class Team : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        // --- İLİŞKİLER ---

        // 1. Takımın Lideri (Bir Takımın BİR Lideri olur)
        public int TeamLeadId { get; set; } // Foreign Key
        public User TeamLead { get; set; } = null!; // Navigation Property

        // 2. Takımın Projeleri (Bir Takımın ÇOK Projesi olur)
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        // 3. Takımın Üyeleri (Çoka-Çok ilişki için ara tabloya referans)
        public ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>();
    }
}