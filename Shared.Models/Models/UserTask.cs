using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models.Models
{
    public class UserTask
    {
        public int EmpId { get; set; }
        public int TaskId { get; set; }

        public DateTime StartedOn { get; set; }
    }
}
