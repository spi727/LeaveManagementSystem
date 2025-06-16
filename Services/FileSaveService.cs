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

            
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                existing = JsonSerializer.Deserialize<List<LeaveApplication>>(json) ?? new();
            }

            
            existing.Add(leave);

           
            string updatedJson = JsonSerializer.Serialize(existing, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, updatedJson);
        }

        public void SaveAllLeaves(List<LeaveApplication> allLeaves)
        {
            // You can use your existing logic but overwrite all leaves.
            string filePath = "leaves.txt"; // or wherever you store leaves
            using (StreamWriter writer = new StreamWriter(filePath, false)) // false = overwrite
            {
                foreach (var leave in allLeaves)
                {
                    string line = $"{leave.EmployeeId},{leave.StartDate},{leave.EndDate},{leave.Status}";
                    writer.WriteLine(line);
                }
            }
        }

    }
}