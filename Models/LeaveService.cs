using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Models
{
    public class LeaveService
    {
        private readonly string _filePath;

        public LeaveService(string filePath)
        {
            _filePath = filePath;
        }

        private async Task<List<LeaveRequest>> LoadLeaveRequestsAsync()
        {
            if (!File.Exists(_filePath))
                return new List<LeaveRequest>();

            string json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<LeaveRequest>>(json) ?? new List<LeaveRequest>();
        }

        public async Task<List<LeaveRequest>> GetLeaveHistoryByEmployeeAsync(string employeeId)
        {
            var requests = await LoadLeaveRequestsAsync();
            return requests
                .Where(lr => lr.EmployeeId == employeeId)
                .OrderByDescending(lr => lr.StartDate)
                .ToList();
        }

        public async Task<List<LeaveRequest>> GetPendingApprovalsAsync(string managerId)
        {
            var requests = await LoadLeaveRequestsAsync();
            return requests
                .Where(lr => lr.ManagerId == managerId && lr.Status == "Pending")
                .OrderBy(lr => lr.StartDate)
                .ToList();
        }
    }
}

