// Yer: JiraProject.Business/Concrete/ProjectManager.cs
using JiraProject.Business.Abstract;
using JiraProject.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiraProject.Business.Concrete
{
    public class ProjectManager : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateProjectAsync(Project project)
        {
            project.CreatedAt = DateTime.UtcNow;
            project.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project != null)
            {
                _unitOfWork.Projects.Remove(project);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _unitOfWork.Projects.GetAllAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _unitOfWork.Projects.GetByIdAsync(id);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            var projectFromDb = await _unitOfWork.Projects.GetByIdAsync(project.Id);
            if (projectFromDb != null)
            {
                projectFromDb.Name = project.Name;
                projectFromDb.Description = project.Description;
                projectFromDb.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}