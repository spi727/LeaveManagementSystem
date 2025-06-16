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