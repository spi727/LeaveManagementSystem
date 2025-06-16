using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Models
{
    /// <summary>
    /// Represents a leave request made by an employee.
    /// </summary>
    public class LeaveRequest
    {
        public int LeaveRequestId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; }

        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public int? ApprovedByManagerId { get; set; }
    }

    /// <summary>
    /// Enumeration of leave request status values.
    /// </summary>
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }
}
