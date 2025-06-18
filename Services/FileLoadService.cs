using LeaveManagementSystem.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Services
{
    public class FileLoadService : IDisposable
    {
        private readonly string _filePath = "leave_requests.json";
        private bool _disposed = false;

        public async Task<List<LeaveRequest>> LoadLeavesAsync()
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
        #region member 7
        // Filter leaves by employee, status (Approved/Cancelled), and reason
        public async Task<List<LeaveRequest>> GetFilteredLeavesAsync(int employeeId, string reason)
        {
            var leaves = await LoadLeavesAsync();

            var filtered = leaves
                .Where(lr => lr.EmployeeId == employeeId &&
                             (lr.Status == LeaveStatus.Approved || lr.Status == LeaveStatus.Cancelled) &&
                             lr.Reason.Contains(reason, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(lr => lr.StartDate)
                .ToList();

            return filtered;
        }
        #endregion

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