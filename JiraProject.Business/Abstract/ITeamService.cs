using JiraProject.Business.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiraProject.Business.Abstract
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamDto>> GetAllTeamsAsync();
        Task<TeamDto> GetTeamByIdAsync(int id);
        Task<TeamDto> CreateTeamAsync(TeamCreateDto dto, int creatorUserId);
        Task DeleteTeamAsync(int id, int currentUserId);
        Task AddUserToTeamAsync(int teamId, AddUserToTeamDto dto, int currentUserId);
        Task RemoveUserFromTeamAsync(int teamId, int userId, int currentUserId);
    }
}