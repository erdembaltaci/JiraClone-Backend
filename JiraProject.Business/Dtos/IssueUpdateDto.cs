using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraProject.Business.Dtos
{
    public class IssueUpdateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Order { get; set; }
        public Entities.TaskStatus Status { get; set; }
    }
}
