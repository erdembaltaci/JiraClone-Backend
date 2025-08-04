// Yer: JiraProject.WebAPI/Controllers/ProjectsController.cs
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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            var projectsDto = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            });
            return Ok(projectsDto);
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();

            var projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };
            return Ok(projectDto);
        }

        // POST: api/Projects
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDto projectDto)
        {
            if (projectDto == null) return BadRequest();

            var projectToCreate = new Project
            {
                Name = projectDto.Name,
                Description = projectDto.Description
            };

            await _projectService.CreateProjectAsync(projectToCreate);

            var resultDto = new ProjectDto { Id = projectToCreate.Id, Name = projectToCreate.Name, Description = projectToCreate.Description, CreatedAt = projectToCreate.CreatedAt };
            return CreatedAtAction(nameof(GetProjectById), new { id = projectToCreate.Id }, resultDto);
        }

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectUpdateDto projectDto)
        {
            var projectFromDb = await _projectService.GetProjectByIdAsync(id);
            if (projectFromDb == null) return NotFound();

            projectFromDb.Name = projectDto.Name;
            projectFromDb.Description = projectDto.Description;

            var projectToUpdate = new Project
            {
                Id = id,
                Name = projectDto.Name,
                Description = projectDto.Description,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = projectFromDb.CreatedAt // CreatedAt'ı koru
            };

            await _projectService.UpdateProjectAsync(projectToUpdate);
            return NoContent();
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();

            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
    }
}