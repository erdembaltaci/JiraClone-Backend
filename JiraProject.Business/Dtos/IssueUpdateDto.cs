using JiraProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraProject.Business.Dtos
{
    
    // MEVCUT BİR GÖREVİ GÜNCELLEMEK İÇİN GEREKENLER
    public class IssueUpdateDto
    {
        // Zorunlu alanlar
        public string Title { get; set; } = null!;

        // İsteğe bağlı veya güncellenebilir alanlar
        public string? Description { get; set; }
        public int? AssigneeId { get; set; } // Atanan kişi değişebilir
        public PriorityLevel Priority { get; set; }
        public JiraProject.Entities.TaskStatus Status { get; set; }
        public DateTime? DueDate { get; set; }

        // ProjectId burada YOK, çünkü görevin projesi değiştirilmez.
    }
}
