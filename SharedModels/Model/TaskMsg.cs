using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedModels.Model
{
    public class TaskMsg
    {
        public int EmpId { get; set; }
        public int TaskId { get; set; }

        public DateTime startTime { get; set; }

    }
}
