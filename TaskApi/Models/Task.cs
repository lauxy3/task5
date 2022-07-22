using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApi.Models
{
    public class Task {
        public int Id { get; set; }
        public string TaskDescription { get; set; }
        public string TaskPriority { get; set; }
        public string TaskStatus { get; set; }
        public int CustomerID { get; set; }
    }
}
