using JiraProject.Entities;
using System;

namespace JiraProject.Business.Dtos
{
    // Bu DTO, hangi filtrelerin uygulanabileceğini tanımlar.
    // Tüm alanları nullable (?) yaptık ki, kullanıcı istediği filtreyi kullanabilsin
    // (sadece tarihe göre, veya sadece statüye göre gibi).
    public class IssueFilterDto
    {
        public int? ProjectId { get; set; }
        public JiraProject.Entities.TaskStatus? Status { get; set; }
        public int? AssigneeId { get; set; }
        public int? ReporterId { get; set; }
        public DateTime? Date { get; set; } // Sadece tarih kısmını dikkate alacağız
    }
}