using AutoMapper;
using JiraProject.Business.Abstract;
using JiraProject.Business.Dtos;
using JiraProject.Business.Exceptions;
using JiraProject.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiraProject.Business.Concrete
{
    public class IssueManager : IIssueService
    {
        // Artık özel repository yerine Generic olanı kullanıyoruz.
        private readonly IGenericRepository<Issue> _issueRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IssueManager(IGenericRepository<Issue> issueRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _issueRepository = issueRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IssueDto> CreateIssueAsync(IssueCreateDto createDto, int reporterId)
        {
            var issueEntity = _mapper.Map<Issue>(createDto);
            issueEntity.ReporterId = reporterId;
            await _issueRepository.AddAsync(issueEntity);
            await _unitOfWork.CompleteAsync();
            return await GetIssueByIdAsync(issueEntity.Id);
        }

        public async Task<IssueDto> GetIssueByIdAsync(int issueId)
        {
            // Özel metot yerine, Generic metodu "include" string'leri ile çağırıyoruz.
            var issue = await _issueRepository.GetByIdWithIncludesAsync(issueId, "Project.Team", "Assignee", "Reporter");
            if (issue == null) throw new NotFoundException($"'{issueId}' ID'li görev bulunamadı.");
            return _mapper.Map<IssueDto>(issue);
        }

        public async Task<IEnumerable<IssueDto>> GetIssuesByProjectIdAsync(int projectId)
        {
            // Bütün görevleri detaylarıyla çekip sonra filtreliyoruz.
            // Daha büyük projelerde bu sorgu için de özel bir metot yazılabilirdi.
            var allIssuesWithDetails = await _issueRepository.GetAllWithIncludesAsync("Project.Team", "Assignee", "Reporter");
            var filteredIssues = allIssuesWithDetails.Where(i => i.ProjectId == projectId);
            return _mapper.Map<IEnumerable<IssueDto>>(filteredIssues);
        }

        public async Task<IssueDto> UpdateIssueAsync(int issueId, IssueUpdateDto updateDto, int currentUserId)
        {
            var issueFromDb = await _issueRepository.GetByIdWithIncludesAsync(issueId, "Project.Team");
            if (issueFromDb == null) throw new NotFoundException("Güncellenecek görev bulunamadı.");

            var teamLeadId = issueFromDb.Project.Team.TeamLeadId;
            if (issueFromDb.ReporterId != currentUserId && issueFromDb.AssigneeId != currentUserId && teamLeadId != currentUserId)
            {
                throw new ForbiddenException("Bu görevi sadece oluşturan kişi, atanan kişi veya takım lideri güncelleyebilir.");
            }

            _mapper.Map(updateDto, issueFromDb);
            await _unitOfWork.CompleteAsync();
            return await GetIssueByIdAsync(issueId);
        }

        public async Task MoveIssueAsync(int issueId, IssueMoveDto moveDto, int currentUserId)
        {
            var issueFromDb = await _issueRepository.GetByIdWithIncludesAsync(issueId, "Project.Team");
            if (issueFromDb == null) throw new NotFoundException("Taşınacak görev bulunamadı.");

            var teamLeadId = issueFromDb.Project.Team.TeamLeadId;
            if (issueFromDb.AssigneeId != currentUserId && teamLeadId != currentUserId)
            {
                throw new ForbiddenException("Bu görevi sadece atanan kişi veya takım lideri taşıyabilir.");
            }

            issueFromDb.Status = moveDto.NewStatus;
            issueFromDb.Order = moveDto.NewOrder;
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteIssueAsync(int issueId, int currentUserId)
        {
            var issueToDelete = await _issueRepository.GetByIdWithIncludesAsync(issueId, "Project.Team");
            if (issueToDelete == null) throw new NotFoundException("Silinecek görev bulunamadı.");

            if (issueToDelete.Project.Team.TeamLeadId != currentUserId)
            {
                throw new ForbiddenException("Görevi sadece takım lideri silebilir.");
            }

            _issueRepository.Remove(issueToDelete);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<IssueDto>> FilterIssuesAsync(IssueFilterDto filterDto)
        {
            // 1. IQueryable'ı al. Bu, sorguyu veritabanına henüz GÖNDERMEZ, sadece hazırlar.
            var query = (await _issueRepository.GetAllWithIncludesAsync("Project.Team", "Assignee", "Reporter")).AsQueryable();

            // 2. Filtre DTO'sundan gelen her bir dolu alan için, sorguya bir "Where" koşulu ekle.
            if (filterDto.ProjectId.HasValue)
            {
                query = query.Where(i => i.ProjectId == filterDto.ProjectId.Value);
            }
            if (filterDto.Status.HasValue)
            {
                query = query.Where(i => i.Status == filterDto.Status.Value);
            }
            if (filterDto.AssigneeId.HasValue)
            {
                query = query.Where(i => i.AssigneeId == filterDto.AssigneeId.Value);
            }
            if (filterDto.ReporterId.HasValue)
            {
                query = query.Where(i => i.ReporterId == filterDto.ReporterId.Value);
            }
            if (filterDto.Date.HasValue)
            {
                // Sadece tarih kısmını karşılaştır, saat/dakika önemli değil.
                var dateToFilter = filterDto.Date.Value.Date;
                query = query.Where(i => i.CreatedAt.Date == dateToFilter);
            }

            // 3. Tüm "Where" koşulları eklendikten sonra, sorguyu veritabanında çalıştır ve sonuçları al.
            var issues = query.ToList();

            // 4. Sonuçları DTO'ya map'leyip döndür.
            return _mapper.Map<IEnumerable<IssueDto>>(issues);
        }
    }
}