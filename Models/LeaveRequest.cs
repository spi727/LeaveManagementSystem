using System;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Models
{
    public class LeaveRequest
    {
        public int LeaveRequestId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; }

        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public int? ApprovedByManagerId { get; set; }
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }
}