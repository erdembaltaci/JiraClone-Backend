using JiraProject.Business.Abstract;
using JiraProject.Business.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        /// Tüm kullanıcıları listeler. Sadece yetkili kullanıcılar erişebilir.
        /// </summary>
        [HttpGet("get-all")] // İSİMLENDİRİLDİ
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var usersDto = await _userService.GetAllUsersAsync();
            return Ok(usersDto);
        }

        /// <summary>
        /// ID'si verilen tek bir kullanıcıyı getirir.
        /// </summary>
        [HttpGet("get-by-id/{id}")] // İSİMLENDİRİLDİ
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);
            return Ok(userDto);
        }

        /// <summary>
        /// Yeni bir kullanıcı oluşturur (Kayıt Olma). Herkes erişebilir.
        /// </summary>
        [HttpPost("register")] // Bu isimlendirme zaten doğru ve standart.
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var createdUserDto = await _userService.CreateUserAsync(userCreateDto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUserDto.Id }, createdUserDto);
        }

        // NOT: Login endpoint'ini AuthController'a taşıdığımız için buradan silebiliriz.
        // Eğer burada kalacaksa, isimlendirmesi zaten doğrudur.

        /// <summary>
        /// ID'si verilen bir kullanıcının bilgilerini günceller.
        /// </summary>
        [HttpPut("update/{id}")] // İSİMLENDİRİLDİ
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedUserDto = await _userService.UpdateUserAsync(id, userUpdateDto);
            return Ok(updatedUserDto);
        }

        /// <summary>
        /// ID'si verilen bir kullanıcıyı siler.
        /// </summary>
        [HttpDelete("delete/{id}")] // İSİMLENDİRİLDİ
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPut("changerole")]
        [Authorize(Roles = "TeamLead")]
        public async Task<IActionResult> ChangeUserRole([FromBody] UserRoleChangeDto roleChangeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.ChangeUserRoleAsync(roleChangeDto);

            // Remove IsSuccess check, just return Ok if result is not null, otherwise BadRequest
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Role change failed.");
        }
    }
}
