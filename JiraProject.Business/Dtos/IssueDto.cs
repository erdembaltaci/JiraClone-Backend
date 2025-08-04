// Yer: JiraProject.Business/Dtos/IssueDto.cs
using System;

namespace JiraProject.Business.Dtos
{
    public class IssueDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = null!; // Enum'ı metin olarak göstereceğiz
        public int ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}