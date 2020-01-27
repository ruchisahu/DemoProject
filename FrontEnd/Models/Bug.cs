using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Models
{
    public class Bug
    {
            [Key]
            public System.Guid TaskId { get; set; }
            public string TaskName { get; set; }
            public System.DateTime CreatedAt { get; set; }
            public Nullable<System.DateTime> CreatedBY { get; set; }
            public string Status { get; set; }
            public string Severity { get; set; }
            public string AssignedBY { get; set; }
        }
}
