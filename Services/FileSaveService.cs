using LeaveManagementSystem.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Services
{
    #region Member3
    public class FileSaveService : IDisposable
    {
        private readonly string _filePath = "leave_requests.json";
        private bool _disposed = false;

        public async Task SaveLeaveAsync(LeaveRequest leave)
        {
            List<LeaveRequest> existing = new();

            if (File.Exists(_filePath))
            {
                await using FileStream readStream = File.OpenRead(_filePath);
                existing = await JsonSerializer.DeserializeAsync<List<LeaveRequest>>(readStream) ?? new();
            }

            existing.Add(leave);

            await using FileStream writeStream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(writeStream, existing, new JsonSerializerOptions { WriteIndented = true });
        }
        #region Member2
        public async Task SaveAllLeavesAsync(List<LeaveRequest> allLeaves)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            await using FileStream writeStream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(writeStream, allLeaves, options);
        }
        #endregion

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