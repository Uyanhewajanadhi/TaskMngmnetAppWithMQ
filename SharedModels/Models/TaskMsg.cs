using System;

namespace SharedModels.Models
{
    public class TaskMsg
    {
        public int EmpId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }

    }
}
