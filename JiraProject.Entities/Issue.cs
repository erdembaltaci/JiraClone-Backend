using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraProject.Entities
{
    public class Issue : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Order { get; set; }

        // Artık StatusId'ye gerek yok, Status'ü doğrudan enum olarak tutacağız.
        public TaskStatus Status { get; set; }

        // Foreign Key
        public int ProjectId { get; set; }

        // Navigation Property
        public Project Project { get; set; } = null!;
    }
}