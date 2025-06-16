using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Services
{
    public class FileLoadService
    {
        private readonly string filePath = "leave_records.json";

        public List<LeaveApplication> LoadLeaves()
        {
            if (!File.Exists(filePath))
                return new List<LeaveApplication>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<LeaveApplication>>(json) ?? new List<LeaveApplication>();
        }
    }
}