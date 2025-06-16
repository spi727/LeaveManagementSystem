namespace LeaveManagementSystem.Services
{
    using global::LeaveManagementSystem.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    namespace LeaveManagementSystem.Services
    {
        public class ManagerService
        {
            private readonly string leaveFilePath = "leaveRequests.json";

            public async Task<List<LeaveRequest>> GetPendingRequestsAsync()
            {
                if (!File.Exists(leaveFilePath))
                    return new List<LeaveRequest>();

                var json = await File.ReadAllTextAsync(leaveFilePath);
                var allRequests = JsonSerializer.Deserialize<List<LeaveRequest>>(json) ?? new List<LeaveRequest>();

                return allRequests.Where(r => r.Status == LeaveStatus.Pending).ToList();
            }

            public async Task<bool> ApproveLeaveAsync(int requestId, int managerId)
            {
                var requests = await LoadLeaveRequestsAsync();

                var request = requests.FirstOrDefault(r => r.LeaveRequestId == requestId);

                if (request == null || request.Status != LeaveStatus.Pending)
                    return false;

                request.Status = LeaveStatus.Approved;
                request.ApprovedByManagerId = managerId;

                await SaveLeaveRequestsAsync(requests);
                return true;
            }

            public async Task<bool> RejectLeaveAsync(int requestId, int managerId)
            {
                var requests = await LoadLeaveRequestsAsync();

                var request = requests.FirstOrDefault(r => r.LeaveRequestId == requestId);

                if (request == null || request.Status != LeaveStatus.Pending)
                    return false;

                request.Status = LeaveStatus.Rejected;
                request.ApprovedByManagerId = managerId;

                await SaveLeaveRequestsAsync(requests);
                return true;
            }

            private async Task<List<LeaveRequest>> LoadLeaveRequestsAsync()
            {
                if (!File.Exists(leaveFilePath))
                    return new List<LeaveRequest>();

                var json = await File.ReadAllTextAsync(leaveFilePath);
                return JsonSerializer.Deserialize<List<LeaveRequest>>(json) ?? new List<LeaveRequest>();
            }

            private async Task SaveLeaveRequestsAsync(List<LeaveRequest> requests)
            {
                var json = JsonSerializer.Serialize(requests, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(leaveFilePath, json);
            }
        }
    }

}
