using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveManagementSystem.Models;


namespace LeaveManagementSystem.Models
{
    public class LeaveService
    {
        private List<LeaveRequest> _leaveRequests;
        public LeaveService(List<LeaveRequest> requests)
        {
            _leaveRequests = requests;
        }

        public List<LeaveRequest> GetLeaveHistoryByEmployee(string employeeId)
        {
            return _leaveRequests
                .Where(lr => lr.EmployeeId == employeeId)
                .OrderByDescending(lr => lr.StartDate)
                .ToList();
        }
        public List<LeaveRequest> GetPendingApprovala(string managerId)
        {
            return _leaveRequests
                .Where(lr => lr.ManagerId == managerId && lr.Status == "Pending")
                .OrderBy(lr => lr.StartDate)
                .ToList();
        }
    }
}
