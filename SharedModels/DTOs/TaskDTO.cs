using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedModels.DTOs
{
    public class TaskDTO
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
        public int EmpId { get; set; }
    }
}
