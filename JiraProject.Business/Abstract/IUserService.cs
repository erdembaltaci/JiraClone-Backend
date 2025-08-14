using JiraProject.Business.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiraProject.Business.Abstract
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(UserCreateDto dto);
        Task<UserDto> UpdateUserAsync(int id, UserUpdateDto dto);
        Task DeleteUserAsync(int id);

        // Login metodu, başarılı olursa UserDto döner, şifre vs. içermez.
        // Başarısız olursa null döner. Controller bu bilgiyle Token üretir.
        Task<UserDto?> LoginAsync(string email, string password);
    }
}