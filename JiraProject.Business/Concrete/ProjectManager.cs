using AutoMapper;
using JiraProject.Business.Abstract;
using JiraProject.Business.Dtos;
using JiraProject.Business.Exceptions;
using JiraProject.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiraProject.Business.Concrete
{
    public class ProjectManager : IProjectService
    {
        private readonly IGenericRepository<Project> _projectRepository;
        private readonly IGenericRepository<Team> _teamRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectManager(IGenericRepository<Project> projectRepository, IGenericRepository<Team> teamRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjectDto> CreateProjectAsync(ProjectCreateDto dto, int creatorUserId)
        {
            var team = await _teamRepository.GetByIdAsync(dto.TeamId);
            if (team == null) throw new NotFoundException("Projenin ekleneceği takım bulunamadı.");
            if (team.TeamLeadId != creatorUserId)
            {
                throw new ForbiddenException("Sadece takım lideri kendi takımına proje ekleyebilir.");
            }

            var projectEntity = _mapper.Map<Project>(dto);
            await _projectRepository.AddAsync(projectEntity);
            await _unitOfWork.CompleteAsync();

            return await GetProjectByIdAsync(projectEntity.Id);
        }

        public async Task<ProjectDto> UpdateProjectAsync(int id, ProjectUpdateDto dto, int currentUserId)
        {
            var projectFromDb = await _projectRepository.GetByIdWithIncludesAsync(id, "Team");
            if (projectFromDb == null) throw new NotFoundException("Güncellenecek proje bulunamadı.");

            if (projectFromDb.Team.TeamLeadId != currentUserId)
            {
                throw new ForbiddenException("Projeyi sadece takım lideri güncelleyebilir.");
            }

            _mapper.Map(dto, projectFromDb);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProjectDto>(projectFromDb);
        }

        public async Task DeleteProjectAsync(int id, int currentUserId)
        {
            var project = await _projectRepository.GetByIdWithIncludesAsync(id, "Team");
            if (project == null) throw new NotFoundException("Silinecek proje bulunamadı.");

            if (project.Team.TeamLeadId != currentUserId)
            {
                throw new ForbiddenException("Projeyi sadece takım lideri silebilir.");
            }

            _projectRepository.Remove(project);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdWithIncludesAsync(id, "Team");
            if (project == null) throw new NotFoundException("Proje bulunamadı.");
            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.GetAllWithIncludesAsync("Team");
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        public Task<ProjectDto> CreateProjectAsync(ProjectCreateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDto> UpdateProjectAsync(int id, ProjectUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProjectAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}