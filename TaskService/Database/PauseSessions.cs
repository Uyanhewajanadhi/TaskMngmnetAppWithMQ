using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService.Database
{
    public class PauseSessions
    {
        [Key]
        public int PublicSessionId { get; set; }
        public DateTime ContinueTime { get; set; }
        public DateTime PauseTime { get; set; }

        //Foreign Key
        public int TaskId { get; set; }
        public TasksDb Task { get; set; }
    }
}
