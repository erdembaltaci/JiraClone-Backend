// Önce ilişkili veriler için küçük özet DTO'lar oluşturalım
public class UserSummaryDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
}

// Sonra ana DTO'muzu bu özetlerle zenginleştirelim
public class IssueDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = null!;
    public string Priority { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }

    // İlişkili verilerin özetleri
    public string ProjectName { get; set; } = null!;
    public UserSummaryDto? Assignee { get; set; } // Atanan kişi bilgileri
    public UserSummaryDto Reporter { get; set; } = null!; // Oluşturan kişi bilgileri
}