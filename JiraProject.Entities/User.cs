using System.ComponentModel.DataAnnotations;

namespace JiraProject.Entities
{
    public class User : BaseEntity // BaseEntity'den miras alabilir
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string PasswordHash { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
    }
}