using JiraProject.Business.Abstract;
using JiraProject.Business.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JiraProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Tüm projeleri listeler.
        /// </summary>
        [HttpGet("get-all")] // İSİMLENDİRİLDİ
        public async Task<IActionResult> GetAllProjects()
        {
            var projectsDto = await _projectService.GetAllProjectsAsync();
            return Ok(projectsDto);
        }

        /// <summary>
        /// ID'si verilen tek bir projeyi getirir.
        /// </summary>
        [HttpGet("get-by-id/{id}")] // İSİMLENDİRİLDİ
        public async Task<IActionResult> GetProjectById(int id)
        {
            var projectDto = await _projectService.GetProjectByIdAsync(id);
            return Ok(projectDto);
        }

        /// <summary>
        /// Yeni bir proje oluşturur.
        /// </summary>
        [HttpPost("create")] // İSİMLENDİRİLDİ
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDto projectCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdProjectDto = await _projectService.CreateProjectAsync(projectCreateDto);

            return CreatedAtAction(nameof(GetProjectById), new { id = createdProjectDto.Id }, createdProjectDto);
        }

        /// <summary>
        /// ID'si verilen bir projeyi günceller.
        /// </summary>
        [HttpPut("update/{id}")] // İSİMLENDİRİLDİ
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectUpdateDto projectUpdateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedProjectDto = await _projectService.UpdateProjectAsync(id, projectUpdateDto);
            return Ok(updatedProjectDto);
        }

        /// <summary>
        /// ID'si verilen bir projeyi siler.
        /// </summary>
        [HttpDelete("delete/{id}")] // İSİMLENDİRİLDİ
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
    }
}