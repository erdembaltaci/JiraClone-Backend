// Yer: JiraProject.Business/Dtos/ProjectCreateDto.cs
namespace JiraProject.Business.Dtos
{
    public class ProjectCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}