using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JiraProject.Entities
{
    // Önce yeni enum'ı tanımlayalım
    public enum PriorityLevel
    {
        Lowest,
        Low,
        Medium,
        High,
        Highest
    }

    public class Issue : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        public TaskStatus Status { get; set; }
        public PriorityLevel Priority { get; set; }

        public int Order { get; set; }
        public DateTime? DueDate { get; set; }

        // --- İLİŞKİLER ---

        // Proje İlişkisi
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        // "Assignee" (Görevi Yapan) İlişkisi (Nullable)
        public int? AssigneeId { get; set; }
        public User? Assignee { get; set; }

        // "Reporter" (Görevi Oluşturan) İlişkisi (Non-Nullable)
        public int ReporterId { get; set; }
        public User Reporter { get; set; } = null!;
    }
}