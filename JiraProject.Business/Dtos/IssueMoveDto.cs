using JiraProject.Entities;

namespace JiraProject.Business.Dtos
{
    public class IssueMoveDto
    {
        // Görevin taşınacağı yeni statü (örn: InProgress)
        public JiraProject.Entities.TaskStatus NewStatus { get; set; }

        // Görevin o statüdeki yeni dikey sırası (örn: 0, 1, 2...)
        public int NewOrder { get; set; }
    }
}