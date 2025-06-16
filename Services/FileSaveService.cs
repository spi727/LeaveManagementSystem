using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Services
{
    public class FileSaveService
    {
        private readonly string filePath = "leave_records.json";

        public void SaveLeave(LeaveApplication leave)
        {
            List<LeaveApplication> existing = new();

            // Load existing data if file exists
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                existing = JsonSerializer.Deserialize<List<LeaveApplication>>(json) ?? new();
            }

            // Add new leave
            existing.Add(leave);

            // Save to file
            string updatedJson = JsonSerializer.Serialize(existing, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, updatedJson);
        }
    }
}
