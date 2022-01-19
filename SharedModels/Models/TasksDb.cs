using System;
using System.ComponentModel.DataAnnotations;

namespace TaskService.Database
{
    public class TasksDb
    {
        [Key]
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime StartTime{get; set;}
        public DateTime EndTime { get; set; }
        public int Status { get; set; }

        public int EmpId { get; set; }
    }
}
