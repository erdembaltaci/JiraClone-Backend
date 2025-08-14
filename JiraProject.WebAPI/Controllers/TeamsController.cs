using JiraProject.Business.Abstract;
using JiraProject.Business.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JiraProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        /// <summary>
        /// Yeni bir takım oluşturur.
        /// </summary>
        [HttpPost("create")] // İSİMLENDİRİLDİ
        public async Task<IActionResult> CreateTeam([FromBody] TeamCreateDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var creatorUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var createdTeamDto = await _teamService.CreateTeamAsync(createDto, creatorUserId);

            return CreatedAtAction(nameof(GetTeamById), new { id = createdTeamDto.Id }, createdTeamDto);
        }

        /// <summary>
        /// Belirtilen takıma yeni bir kullanıcı ekler.
        /// </summary>
        [HttpPost("{teamId}/add-member")] // İSİMLENDİRİLDİ
        public async Task<IActionResult> AddUserToTeam(int teamId, [FromBody] AddUserToTeamDto addUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _teamService.AddUserToTeamAsync(teamId, addUserDto, currentUserId);

            return Ok(new { message = "Kullanıcı takıma başarıyla eklendi." });
        }

        /// <summary>
        /// Belirtilen takımdan bir kullanıcıyı çıkarır.
        /// </summary>
        [HttpDelete("{teamId}/remove-member/{userId}")] // İSİMLENDİRİLDİ
        public async Task<IActionResult> RemoveUserFromTeam(int teamId, int userId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _teamService.RemoveUserFromTeamAsync(teamId, userId, currentUserId);

            return Ok(new { message = "Kullanıcı takımdan başarıyla çıkarıldı." });
        }

        /// <summary>
        /// Tüm takımları listeler.
        /// </summary>
        [HttpGet("get-all")] // İSİMLENDİRİLDİ
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return Ok(teams);
        }

        /// <summary>
        /// ID'si verilen bir takımı getirir.
        /// </summary>
        [HttpGet("get-by-id/{id}")] // İSİMLENDİRİLDİ
        public async Task<IActionResult> GetTeamById(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            return Ok(team);
        }
    }
}