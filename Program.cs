using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Services;

class Program
{
    static async Task Main()
    {
        FileLoadService loadService = new FileLoadService();
        FileSaveService saveService = new FileSaveService();

        Console.WriteLine("Welcome to the Leave Management System\n");

        Console.WriteLine("1. Apply Leave");
        Console.WriteLine("2. View All Leave Applications");
        Console.WriteLine("3. Cancel Leave");
        Console.Write("Choose an option: ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("Invalid input.");
            return;
        }

        switch (choice)
        {
            case 1:
                LeaveApplication newLeave = new LeaveApplication();

                Console.Write("Enter Employee ID: ");
                newLeave.EmployeeId = int.Parse(Console.ReadLine());

                Console.Write("Enter Start Date (yyyy-mm-dd): ");
                newLeave.StartDate = Console.ReadLine();

                Console.Write("Enter End Date (yyyy-mm-dd): ");
                newLeave.EndDate = Console.ReadLine();

                newLeave.Status = "Pending";

                await saveService.SaveLeaveAsync(newLeave);
                Console.WriteLine("Leave application saved successfully!");
                break;

            case 2:
                List<LeaveApplication> leaves = await loadService.LoadLeavesAsync();

                Console.WriteLine("\nLeave Applications:");
                foreach (var leave in leaves)
                {
                    Console.WriteLine($"Employee ID: {leave.EmployeeId}, Start: {leave.StartDate}, End: {leave.EndDate}, Status: {leave.Status}");
                }
                break;

            case 3:
                Console.Write("Enter Employee ID: ");
                int employeeId = int.Parse(Console.ReadLine());

                Console.Write("Enter Start Date of Leave to Cancel (yyyy-mm-dd): ");
                string startDate = Console.ReadLine();

                List<LeaveApplication> allLeaves = await loadService.LoadLeavesAsync();

                var leaveToCancel = allLeaves.Find(l =>
                    l.EmployeeId == employeeId &&
                    l.StartDate == startDate &&
                    l.Status == "Pending");

                if (leaveToCancel != null)
                {
                    leaveToCancel.Status = "Cancelled";
                    await saveService.SaveAllLeavesAsync(allLeaves); // async overwrite
                    Console.WriteLine("Leave application cancelled successfully.");
                }
                else
                {
                    Console.WriteLine("Leave not found or cannot be cancelled.");
                }
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}
