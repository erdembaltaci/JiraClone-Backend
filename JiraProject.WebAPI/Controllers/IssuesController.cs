// Yer: JiraProject.WebAPI/Controllers/IssuesController.cs
using JiraProject.Business.Abstract;
using JiraProject.Business.Dtos; // DTO'ları kullanmak için
using JiraProject.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq; // LINQ kullanmak için
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class IssuesController : ControllerBase
{
    private readonly IIssueService _issueService;

    public IssuesController(IIssueService issueService)
    {
        _issueService = issueService;
    }

    // GET: api/Issues
    [HttpGet]
    public async Task<IActionResult> GetAllIssues()
    {
        var issues = await _issueService.GetAllIssuesAsync();

        // Entity listesini DTO listesine dönüştürüyoruz (Mapping)
        var issuesDto = issues.Select(issue => new IssueDto
        {
            Id = issue.Id,
            Title = issue.Title,
            Description = issue.Description,
            Status = issue.Status.ToString(), // Enum'ı string'e çeviriyoruz
            ProjectId = issue.ProjectId,
            CreatedAt = issue.CreatedAt,
            UpdatedAt = issue.UpdatedAt
        });

        return Ok(issuesDto);
    }

    // GET: api/Issues/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetIssueById(int id)
    {
        var issue = await _issueService.GetIssueByIdAsync(id);
        if (issue == null)
        {
            return NotFound();
        }

        // Tek bir Entity'yi DTO'ya dönüştürüyoruz
        var issueDto = new IssueDto
        {
            Id = issue.Id,
            Title = issue.Title,
            Description = issue.Description,
            Status = issue.Status.ToString(),
            ProjectId = issue.ProjectId,
            CreatedAt = issue.CreatedAt,
            UpdatedAt = issue.UpdatedAt
        };

        return Ok(issueDto);
    }

    // POST: api/Issues
    [HttpPost]
    public async Task<IActionResult> CreateIssue([FromBody] IssueCreateDto issueDto)
    {
        if (issueDto == null)
        {
            return BadRequest();
        }

        // DTO'yu, veritabanına kaydedilecek gerçek Entity'ye dönüştürüyoruz (Mapping).
        var issueToCreate = new Issue
        {
            Title = issueDto.Title,
            Description = issueDto.Description,
            Status = issueDto.Status,
            ProjectId = issueDto.ProjectId,
            Order = 0 // Order'ı varsayılan olarak 0 atayabiliriz.
        };

        // Servis metodumuzu çağırarak görevi oluşturuyoruz.
        await _issueService.CreateIssueAsync(issueToCreate);

        // Yeni oluşturulan kaynağın tam halini ve konumunu (adresini) döndürüyoruz.
        // Dönen sonucu da DTO'ya çevirip göstermek en iyisidir.
        var resultDto = new IssueDto
        {
            Id = issueToCreate.Id,
            Title = issueToCreate.Title,
            Description = issueToCreate.Description,
            Status = issueToCreate.Status.ToString(),
            ProjectId = issueToCreate.ProjectId,
            CreatedAt = issueToCreate.CreatedAt,
            UpdatedAt = issueToCreate.UpdatedAt
        };

        return CreatedAtAction(nameof(GetIssueById), new { id = issueToCreate.Id }, resultDto);
    }

    // PUT: api/Issues/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIssue(int id, [FromBody] IssueUpdateDto issueDto)
    {
        var issueFromDb = await _issueService.GetIssueByIdAsync(id);
        if (issueFromDb == null) return NotFound();

        issueFromDb.Title = issueDto.Title;
        issueFromDb.Description = issueDto.Description;
        issueFromDb.Status = issueDto.Status;
        issueFromDb.Order = issueDto.Order;
        issueFromDb.UpdatedAt = System.DateTime.UtcNow;

        await _issueService.UpdateIssueAsync(issueFromDb);
        return NoContent();
    }

    // DELETE: api/Issues/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIssue(int id)
    {
        var issue = await _issueService.GetIssueByIdAsync(id);
        if (issue == null) return NotFound();

        await _issueService.DeleteIssueAsync(id);
        return NoContent();
    }
}