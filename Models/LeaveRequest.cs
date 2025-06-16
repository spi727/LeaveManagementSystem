using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Models
{
    public class LeaveRequest
    {
        public int RequestId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
        public DateTime? ApprovedOn { get; set; }
        public string? RejectionReason { get; set; }
    }
}
