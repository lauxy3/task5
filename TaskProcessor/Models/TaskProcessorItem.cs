using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProcessor.Models
{
    public class TaskProcessorItem
    {
        public int Id { get; set; }
        public string TaskDescription { get; set; }
        public string TaskPriority { get; set; }
        public string TaskStatus { get; set; }
        public int CustomerID { get; set; }
    }
}
