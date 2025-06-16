using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Models
{
    public class LeaveService : IDisposable
    {
        private List<LeaveRequest> _leaveRequests;
        private readonly string _filePath = "leave_requests.json";
        private bool _disposed = false;

        public LeaveService()
        {
            _leaveRequests = LoadLeaveRequests().GetAwaiter().GetResult();
        }

        public List<LeaveRequest> GetLeaveHistoryByEmployee(int employeeId)
        {
            return _leaveRequests
                .Where(lr => lr.EmployeeId == employeeId)
                .OrderByDescending(lr => lr.StartDate)
                .ToList();
        }

        public List<LeaveRequest> GetPendingApprovals(int managerId)
        {
            return _leaveRequests
                .Where(lr => lr.ApprovedByManagerId == managerId && lr.Status == LeaveStatus.Pending)
                .OrderBy(lr => lr.StartDate)
                .ToList();
        }

        public bool CancelLeaveRequest(int leaveRequestId, int employeeId)
        {
            var leaveRequest = _leaveRequests.FirstOrDefault(lr =>
                lr.LeaveRequestId == leaveRequestId &&
                lr.EmployeeId == employeeId);

            if (leaveRequest == null || leaveRequest.Status != LeaveStatus.Pending)
            {
                return false;
            }

            leaveRequest.Status = LeaveStatus.Cancelled;
            SaveChanges();
            return true;
        }

        public void ApplyLeave(LeaveRequest request)
        {
            bool isOverlap = _leaveRequests.Any(lr =>
                lr.EmployeeId == request.EmployeeId &&
                lr.Status != LeaveStatus.Cancelled &&
                lr.StartDate < request.EndDate &&
                lr.EndDate > request.StartDate);

            if (isOverlap)
            {
                throw new InvalidOperationException("Leave request overlaps with an existing approved or pending request.");
            }

            request.LeaveRequestId = _leaveRequests.Any() ? _leaveRequests.Max(lr => lr.LeaveRequestId) + 1 : 1;
            _leaveRequests.Add(request);
            SaveChanges();
        }

        public bool ApproveRequest(int leaveRequestId, int managerId)
        {
            var leaveRequest = _leaveRequests.FirstOrDefault(lr =>
                lr.LeaveRequestId == leaveRequestId &&
                lr.ApprovedByManagerId == managerId &&
                lr.Status == LeaveStatus.Pending);

            if (leaveRequest == null) return false;

            leaveRequest.Status = LeaveStatus.Approved;
            SaveChanges();
            return true;
        }

        public bool RejectRequest(int leaveRequestId, int managerId)
        {
            var leaveRequest = _leaveRequests.FirstOrDefault(lr =>
                lr.LeaveRequestId == leaveRequestId &&
                lr.ApprovedByManagerId == managerId &&
                lr.Status == LeaveStatus.Pending);

            if (leaveRequest == null) return false;

            leaveRequest.Status = LeaveStatus.Rejected;
            SaveChanges();
            return true;
        }

        private async Task<List<LeaveRequest>> LoadLeaveRequests()
        {
            if (!File.Exists(_filePath))
                return new List<LeaveRequest>();

            try
            {
                await using FileStream stream = File.OpenRead(_filePath);
                return await JsonSerializer.DeserializeAsync<List<LeaveRequest>>(stream) ?? new List<LeaveRequest>();
            }
            catch
            {
                return new List<LeaveRequest>();
            }
        }

        private void SaveChanges()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_leaveRequests, options);
                File.WriteAllText(_filePath, json);
            }
            catch
            {
                // Log error in real application
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                SaveChanges();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}