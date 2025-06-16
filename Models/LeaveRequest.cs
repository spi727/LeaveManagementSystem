using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Models
{
    public class LeaveRequest
    {
        public string EmployeeId { get; set; }
        public string ManagerId { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
    }
}
