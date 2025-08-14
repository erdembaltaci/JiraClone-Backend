using JiraProject.Business.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto> GetProjectByIdAsync(int id);
    Task<ProjectDto> CreateProjectAsync(ProjectCreateDto dto);
    Task<ProjectDto> UpdateProjectAsync(int id, ProjectUpdateDto dto);
    Task DeleteProjectAsync(int id);
}