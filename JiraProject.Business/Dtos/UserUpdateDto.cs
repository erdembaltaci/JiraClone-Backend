// Yer: JiraProject.Business/Dtos/UserUpdateDto.cs
namespace JiraProject.Business.Dtos
{
    public class UserUpdateDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;

        // Not: Şifre güncelleme gibi hassas işlemler genellikle
        // kendi özel endpoint'leri ile yapılır (örn: /api/users/change-password),
        // bu yüzden genel güncelleme DTO'sunda yer almaz. Bu daha güvenli bir yaklaşımdır.
    }
}