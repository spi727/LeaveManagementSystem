namespace LeaveManagementSystem.Models
{
    public class LeaveApplication
    {
        public int EmployeeId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}