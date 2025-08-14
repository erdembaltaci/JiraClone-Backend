using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JiraProject.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)] // Açıklama için daha uzun bir limit
        public string? Description { get; set; }


        // --- İlişkiler (Relationships) ---

        // 1. Bir Proje BİR Takım'a aittir (One-to-Many)
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;

        // 2. Bir Proje'nin ÇOK sayıda Issue'su olabilir (One-to-Many)
        public ICollection<Issue> Issues { get; set; } = new List<Issue>();
    }
}