// Yer: JiraProject.Business/Dtos/IssueCreateDto.cs
namespace JiraProject.Business.Dtos
{
    public class IssueCreateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Entities.TaskStatus Status { get; set; }
        public int ProjectId { get; set; }
    }
}