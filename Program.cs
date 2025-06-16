using System;
using System.Collections.Generic;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Services;

class Program
{
    static void Main()
    {
        FileLoadService loadService = new FileLoadService();
        FileSaveService saveService = new FileSaveService();

        Console.WriteLine("Welcome to the Leave Management System\n");

        Console.WriteLine("1. Apply Leave");
        Console.WriteLine("2. View All Leave Applications");
        Console.Write("Choose an option: ");
        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            LeaveApplication newLeave = new LeaveApplication();

            Console.Write("Enter Employee ID: ");
            newLeave.EmployeeId = int.Parse(Console.ReadLine());

            Console.Write("Enter Start Date (yyyy-mm-dd): ");
            newLeave.StartDate = Console.ReadLine();

            Console.Write("Enter End Date (yyyy-mm-dd): ");
            newLeave.EndDate = Console.ReadLine();

            newLeave.Status = "Pending";

            saveService.SaveLeave(newLeave);
            Console.WriteLine("Leave application saved successfully!");
        }
        else if (choice == 2)
        {
            List<LeaveApplication> leaves = loadService.LoadLeaves();

            Console.WriteLine("\nLeave Applications:");
            foreach (var leave in leaves)
            {
                Console.WriteLine($"Employee ID: {leave.EmployeeId}, Start: {leave.StartDate}, End: {leave.EndDate}, Status: {leave.Status}");
            }
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
}