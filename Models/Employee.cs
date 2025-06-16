using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Models
{
    /// <summary>
    /// Represents an employee in the leave management system.
    /// </summary>

    public class Employee
    {
        public int EmployeeId { get; set; }

        public string Name { get; set; }

        public string Department { get; set; }

        public string Email { get; set; }

        public int LeaveBalance { get; set; }

        public List<LeaveRequest>? LeaveRequests { get; set; }
    }

}

