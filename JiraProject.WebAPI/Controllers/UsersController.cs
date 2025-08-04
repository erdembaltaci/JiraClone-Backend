// Yer: JiraProject.WebAPI/Controllers/UsersController.cs
using JiraProject.Business.Abstract;
using JiraProject.Business.Dtos;
using JiraProject.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JiraProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Tüm kullanıcıları listeler.
        /// </summary>
        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            // Entity'leri ASLA doğrudan döndürme! DTO'ya çevirerek hassas bilgileri gizle.
            var usersDto = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            });

            return Ok(usersDto);
        }

        /// <summary>
        /// ID'si verilen tek bir kullanıcıyı getirir.
        /// </summary>
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Kullanıcı bulunamadıysa 404 hatası döndür.
            }

            // Entity'yi DTO'ya çeviriyoruz. Şifre bilgisi dışarı sızmıyor.
            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };

            return Ok(userDto);
        }

        /// <summary>
        /// Yeni bir kullanıcı oluşturur.
        /// </summary>
        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userDto)
        {
            if (userDto == null) return BadRequest();

            // DTO'yu, veritabanına kaydedilecek gerçek User Entity'sine dönüştürüyoruz.
            var userToCreate = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                // GERÇEK BİR UYGULAMADA ŞİFRE BURADA GÜVENLİ BİR ŞEKİLDE HASH'LENMELİ!
                // Şimdilik test amaçlı düz metin olarak kaydediyoruz.
                PasswordHash = userDto.Password
            };

            await _userService.CreateUserAsync(userToCreate);

            // Dönen sonucu, şifre gibi hassas bilgiler içermeyen UserDto'ya çeviriyoruz.
            var resultDto = new UserDto
            {
                Id = userToCreate.Id,
                Username = userToCreate.Username,
                Email = userToCreate.Email,
                CreatedAt = userToCreate.CreatedAt
            };

            return CreatedAtAction(nameof(GetUserById), new { id = userToCreate.Id }, resultDto);
        }

        /// <summary>
        /// ID'si verilen bir kullanıcının bilgilerini günceller.
        /// </summary>
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
        {
            var userFromDb = await _userService.GetUserByIdAsync(id);
            if (userFromDb == null)
            {
                return NotFound();
            }

            userFromDb.Username = userDto.Username;
            userFromDb.Email = userDto.Email;
            userFromDb.UpdatedAt = DateTime.UtcNow;

            await _userService.UpdateUserAsync(userFromDb);

            return NoContent(); // Başarılı, HTTP 204
        }

        /// <summary>
        /// ID'si verilen bir kullanıcıyı siler.
        /// </summary>
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}