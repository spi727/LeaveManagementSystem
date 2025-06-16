using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Models
{
    /// <summary>
    /// Represents a manager who oversees employee leave requests.
    /// </summary>
    public class Manager
    {
        public int ManagerId { get; set; }

        public string Name { get; set; }

        public string Department { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// List of employee IDs that report to this manager.
        /// </summary>
        public List<int>? EmployeeIds { get; set; }
    }
}
