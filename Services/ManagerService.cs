using LeaveManagementSystem.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


//Both Member4 and Member3 has worked on this file
//Member4 has written the logic
//Member3 has implemented async and await
#region Member4
namespace LeaveManagementSystem.Services
{
    public class ManagerService : IDisposable
    {
        private readonly string _filePath = "leave_requests.json";
        private bool _disposed = false;

        public async Task<List<LeaveRequest>> GetPendingRequestsAsync(int managerId)
        {
            if (!File.Exists(_filePath))
                return new List<LeaveRequest>();

            try
            {
                await using FileStream stream = File.OpenRead(_filePath);
                var allRequests = await JsonSerializer.DeserializeAsync<List<LeaveRequest>>(stream) ?? new List<LeaveRequest>();
                return allRequests.Where(r => r.Status == LeaveStatus.Pending).ToList();
            }
            catch
            {
                return new List<LeaveRequest>();
            }
        }

        public async Task<bool> ApproveLeaveAsync(int requestId, int managerId)
        {
            List<LeaveRequest> requests;

            try
            {
                await using FileStream readStream = File.OpenRead(_filePath);
                requests = await JsonSerializer.DeserializeAsync<List<LeaveRequest>>(readStream) ?? new List<LeaveRequest>();
            }
            catch
            {
                return false;
            }

            var request = requests.FirstOrDefault(r => r.LeaveRequestId == requestId);

            if (request == null || request.Status != LeaveStatus.Pending)
                return false;

            request.Status = LeaveStatus.Approved;
            request.ApprovedByManagerId = managerId;

            try
            {
                await using FileStream writeStream = File.Create(_filePath);
                await JsonSerializer.SerializeAsync(writeStream, requests, new JsonSerializerOptions { WriteIndented = true });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RejectLeaveAsync(int requestId, int managerId)
        {
            List<LeaveRequest> requests;

            try
            {
                await using FileStream readStream = File.OpenRead(_filePath);
                requests = await JsonSerializer.DeserializeAsync<List<LeaveRequest>>(readStream) ?? new List<LeaveRequest>();
            }
            catch
            {
                return false;
            }

            var request = requests.FirstOrDefault(r => r.LeaveRequestId == requestId);

            if (request == null || request.Status != LeaveStatus.Pending)
                return false;

            request.Status = LeaveStatus.Rejected;
            request.ApprovedByManagerId = managerId;

            try
            {
                await using FileStream writeStream = File.Create(_filePath);
                await JsonSerializer.SerializeAsync(writeStream, requests, new JsonSerializerOptions { WriteIndented = true });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
#endregion