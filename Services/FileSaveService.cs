using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Services
{
    public class FileSaveService
    {
        private readonly string filePath = "leave_records.json";

        public async Task SaveLeaveAsync(LeaveApplication leave)
        {
            List<LeaveApplication> existing = new();

            if (File.Exists(filePath))
            {
                using FileStream readStream = File.OpenRead(filePath);
                existing = await JsonSerializer.DeserializeAsync<List<LeaveApplication>>(readStream) ?? new();
            }

            existing.Add(leave);

            using FileStream writeStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(writeStream, existing, new JsonSerializerOptions { WriteIndented = true });
        }

        public async Task SaveAllLeavesAsync(List<LeaveApplication> allLeaves)
        {
            string textFilePath = "leaves.txt";

            using StreamWriter writer = new StreamWriter(textFilePath, false); // false = overwrite
            foreach (var leave in allLeaves)
            {
                string line = $"{leave.EmployeeId},{leave.StartDate},{leave.EndDate},{leave.Status}";
                await writer.WriteLineAsync(line);
            }
        }
    }
}
