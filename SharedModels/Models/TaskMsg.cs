using System;

namespace SharedModels.Models
{
    public class TaskMsg
    {
        public int EmpId { get; set; }
        public int TaskId { get; set; }
        public DateTime startTime { get; set; }
    }
}
