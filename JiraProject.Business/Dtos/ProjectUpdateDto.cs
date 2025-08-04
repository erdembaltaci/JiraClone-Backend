// Yer: JiraProject.Business/Dtos/ProjectUpdateDto.cs
namespace JiraProject.Business.Dtos
{
    public class ProjectUpdateDto
    {
        // Şimdilik CreateDto ile aynı alanları içerecek.
        // İleride "Proje Adı değiştirilemez" gibi kurallar gelirse,
        // bu DTO'dan Name alanını silebiliriz.
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}