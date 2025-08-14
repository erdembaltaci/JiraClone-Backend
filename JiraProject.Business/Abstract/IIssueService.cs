using JiraProject.Business.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiraProject.Business.Abstract
{
    public interface IIssueService
    {
        
        Task<IEnumerable<IssueDto>> GetIssuesByProjectIdAsync(int projectId);      
        Task<IssueDto> GetIssueByIdAsync(int issueId);
        Task<IssueDto> CreateIssueAsync(IssueCreateDto createDto, int reporterId);
        Task<IssueDto> UpdateIssueAsync(int issueId, IssueUpdateDto updateDto, int currentUserId);     
        Task DeleteIssueAsync(int issueId, int currentUserId);
        Task MoveIssueAsync(int issueId, IssueMoveDto moveDto, int currentUserId);
        Task<IEnumerable<IssueDto>> FilterIssuesAsync(IssueFilterDto filterDto);
    }
}