using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Services
{
    public class FileLoadService
    {
        private readonly string filePath = "leave_records.json";

        public async Task<List<LeaveApplication>> LoadLeavesAsync()
        {
            if (!File.Exists(filePath))
                return new List<LeaveApplication>();

            using FileStream stream = File.OpenRead(filePath);
            var data = await JsonSerializer.DeserializeAsync<List<LeaveApplication>>(stream);
            return data ?? new List<LeaveApplication>();
        }
    }
}
