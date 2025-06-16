using System;
using System.Collections.Generic;
using System.Linq;

namespace LeaveManagementSystem.Models
{
    public class LeaveService
    {
        private List<LeaveRequest> _leaveRequests;

        public LeaveService(List<LeaveRequest> requests)
        {
            _leaveRequests = requests;
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
                return false; // Cannot cancel if not found or already processed
            }

            leaveRequest.Status = LeaveStatus.Cancelled;
            return true;
        }

        public void ApplyLeave(LeaveRequest request)
        {
            // Basic overlapping check
            bool isOverlap = _leaveRequests.Any(lr =>
                lr.EmployeeId == request.EmployeeId &&
                lr.Status != LeaveStatus.Cancelled &&
                lr.StartDate < request.EndDate &&
                lr.EndDate > request.StartDate);

            if (isOverlap)
            {
                throw new InvalidOperationException("Leave request overlaps with an existing approved or pending request.");
            }

            _leaveRequests.Add(request);
        }

        public bool ApproveRequest(int leaveRequestId, int managerId)
        {
            var leaveRequest = _leaveRequests.FirstOrDefault(lr =>
                lr.LeaveRequestId == leaveRequestId &&
                lr.ApprovedByManagerId == managerId &&
                lr.Status == LeaveStatus.Pending);

            if (leaveRequest == null) return false;

            leaveRequest.Status = LeaveStatus.Approved;
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
            return true;
        }
    }
}

