using AutoMapper;
using JiraProject.Business.Abstract;
using JiraProject.Business.Dtos;
using JiraProject.Business.Exceptions;
using JiraProject.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraProject.Business.Concrete
{
    public class TeamManager : ITeamService
    {
        private readonly IGenericRepository<Team> _teamRepository;
        private readonly IGenericRepository<UserTeam> _userTeamRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeamManager(IGenericRepository<Team> teamRepository, IGenericRepository<UserTeam> userTeamRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _userTeamRepository = userTeamRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TeamDto> CreateTeamAsync(TeamCreateDto dto, int creatorUserId)
        {
            if (dto.TeamLeadId != creatorUserId)
            {
                throw new ForbiddenException("Bir takımı sadece kendinizi lider olarak atayarak oluşturabilirsiniz.");
            }

            var teamEntity = _mapper.Map<Team>(dto);
            await _teamRepository.AddAsync(teamEntity);

            var userTeam = new UserTeam { Team = teamEntity, UserId = dto.TeamLeadId };
            await _userTeamRepository.AddAsync(userTeam);

            await _unitOfWork.CompleteAsync();
            return await GetTeamByIdAsync(teamEntity.Id);
        }

        public async Task AddUserToTeamAsync(int teamId, AddUserToTeamDto dto, int currentUserId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null) throw new NotFoundException("Takım bulunamadı.");
            if (team.TeamLeadId != currentUserId) throw new ForbiddenException("Sadece takım lideri üye ekleyebilir.");

            var existing = (await _userTeamRepository.FindAsync(ut => ut.TeamId == teamId && ut.UserId == dto.UserId)).Any();
            if (existing) return;

            var userTeam = new UserTeam { TeamId = teamId, UserId = dto.UserId };
            await _userTeamRepository.AddAsync(userTeam);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveUserFromTeamAsync(int teamId, int userId, int currentUserId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null) throw new NotFoundException("Takım bulunamadı.");
            if (team.TeamLeadId == userId) throw new BadRequestException("Takım lideri takımdan çıkarılamaz.");
            if (team.TeamLeadId != currentUserId) throw new ForbiddenException("Sadece takım lideri üye çıkarabilir.");

            var userTeam = (await _userTeamRepository.FindAsync(ut => ut.TeamId == teamId && ut.UserId == userId)).FirstOrDefault();
            if (userTeam != null)
            {
                _userTeamRepository.Remove(userTeam);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteTeamAsync(int id, int currentUserId)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null) throw new NotFoundException("Silinecek takım bulunamadı.");
            if (team.TeamLeadId != currentUserId) throw new ForbiddenException("Sadece takım lideri takımı silebilir.");

            _teamRepository.Remove(team);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<TeamDto> GetTeamByIdAsync(int id)
        {
            var team = await _teamRepository.GetByIdWithIncludesAsync(id, "TeamLead", "UserTeams.User");
            if (team == null) throw new NotFoundException("Takım bulunamadı.");
            return _mapper.Map<TeamDto>(team);
        }

        public async Task<IEnumerable<TeamDto>> GetAllTeamsAsync()
        {
            var teams = await _teamRepository.GetAllWithIncludesAsync("TeamLead", "UserTeams.User");
            return _mapper.Map<IEnumerable<TeamDto>>(teams);
        }
    }
}