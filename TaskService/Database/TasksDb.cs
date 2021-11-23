using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService.Database
{
    public class TasksDb
    {
        [Key]
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime StartTime{get; set;}
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
    }
}
