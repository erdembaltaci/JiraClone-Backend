// Yer: JiraProject.Business/Dtos/UserCreateDto.cs
namespace JiraProject.Business.Dtos
{
    public class UserCreateDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!; // Şifreyi düz metin alacağız, sonra hash'leyeceğiz.
        public string Email { get; set; } = null!;
    }
}