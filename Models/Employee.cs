using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Department { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Range(0, 365)]
        public int LeaveBalance { get; set; } = 30;

        public List<LeaveRequest>? LeaveRequests { get; set; } = new();
    }
}