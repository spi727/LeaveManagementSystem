using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Department { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<int>? EmployeeIds { get; set; } = new();
    }
}