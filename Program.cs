﻿using System;
using System.Threading.Tasks;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Services;

//Member3 has implemeted async and await


namespace LeaveManagementSystem
{
    class Program
    {
        public static async Task Main()
        {
            var fileLoadService = new FileLoadService();
            var fileSaveService = new FileSaveService();
            var leaveService = new LeaveService();
            var managerService = new ManagerService();

            Console.WriteLine("Welcome to the Leave Management System\n");

            while (true)
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1. Apply for Leave");
                Console.WriteLine("2. View My Leave History");
                Console.WriteLine("3. Cancel Leave Request");
                Console.WriteLine("4. Approve/Reject Leave Requests (Manager)");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        #region Member2
                        case 1: // Apply Leave
                            var newRequest = new LeaveRequest();

                            Console.Write("Enter Employee ID: ");
                            newRequest.EmployeeId = int.Parse(Console.ReadLine());

                            Console.Write("Enter Start Date (yyyy-mm-dd): ");
                            newRequest.StartDate = DateTime.Parse(Console.ReadLine());

                            Console.Write("Enter End Date (yyyy-mm-dd): ");
                            newRequest.EndDate = DateTime.Parse(Console.ReadLine());

                            Console.Write("Enter Reason: ");
                            newRequest.Reason = Console.ReadLine();

                            Console.Write("Enter Manager ID for approval: ");
                            newRequest.ApprovedByManagerId = int.Parse(Console.ReadLine());

                            leaveService.ApplyLeave(newRequest);
                           // await fileSaveService.SaveLeaveAsync(newRequest);
                            Console.WriteLine("Leave request submitted successfully!");
                            break;
                        #endregion

                     

                        
                            

                        #region member 7
                        case 2: // View Leave History

                            Console.Write("Enter Employee ID: ");
                            int empId = int.Parse(Console.ReadLine());

                            leaveService.ReloadFromFile();

                            var history = leaveService.GetLeaveHistoryByEmployee(empId);
                            Console.Write("Filter by Status (Approved, Cancelled, Pending, Rejected)? Leave blank to skip: ");
                            var statusInput = Console.ReadLine();

                            Console.Write("Filter by Reason keyword? Leave blank to skip: ");
                            var reasonInput = Console.ReadLine();

                            var filtered = history.AsEnumerable();

                            if (!string.IsNullOrWhiteSpace(statusInput) &&
                                Enum.TryParse(typeof(LeaveStatus), statusInput, true, out var statusEnum))
                            {
                                var parsedStatus = (LeaveStatus)statusEnum;
                                filtered = filtered.Where(l => l.Status == parsedStatus);
                            }

                            if (!string.IsNullOrWhiteSpace(reasonInput))
                            {
                                filtered = filtered.Where(l => l.Reason.Contains(reasonInput, StringComparison.OrdinalIgnoreCase));
                            }

                            Console.WriteLine("\nFiltered Leave History:");
                            foreach (var request in history)
                            {
                                Console.WriteLine($"{request.StartDate:d} to {request.EndDate:d} - {request.Status}");
                            }
                            break;
                        #endregion

                        #region Member2
                        case 3: // Cancel Leave
                            Console.Write("Enter Employee ID: ");
                            int cancelEmpId = int.Parse(Console.ReadLine());

                            Console.WriteLine("Here are your leaves :");
                            leaveService.ReloadFromFile();
                            var leaves = leaveService.GetLeaveHistoryByEmployee(cancelEmpId);
                            foreach (var request in leaves)
                            {
                                Console.WriteLine($"ID-{request.LeaveRequestId}  {request.StartDate:d} to {request.EndDate:d} - {request.Status}");
                            }
                            Console.Write("Enter Leave Request ID to cancel: ");
                            int leaveId = int.Parse(Console.ReadLine());

                            if (leaveService.CancelLeaveRequest(leaveId, cancelEmpId))
                            {
                                await fileSaveService.SaveAllLeavesAsync(leaveService.GetLeaveHistoryByEmployee(cancelEmpId));
                                Console.WriteLine("Leave request cancelled successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Unable to cancel leave request. It may already be processed.");
                            }
                            break;
                        #endregion

                        #region Member4
                        case 4: // Manager Approvals
                            Console.Write("Enter Manager ID: ");
                            int managerId = int.Parse(Console.ReadLine());

                            var pendingRequests = managerService.GetPendingRequestsAsync(managerId).Result;
                            if (pendingRequests.Count == 0)
                            {
                                Console.WriteLine("No pending leave requests.");
                                break;
                            }

                            Console.WriteLine("\nPending Leave Requests:");
                            foreach (var req in pendingRequests)
                            {
                                Console.WriteLine($"ID: {req.LeaveRequestId}, Employee: {req.EmployeeId}, Dates: {req.StartDate:d} to {req.EndDate:d}");
                            }

                            Console.Write("Enter Leave Request ID to process: ");
                            int reqId = int.Parse(Console.ReadLine());

                            Console.Write("Approve (A) or Reject (R)? ");
                            var decision = Console.ReadLine().ToUpper();

                            bool success = false;
                            if (decision == "A")
                            {
                                success = await managerService.ApproveLeaveAsync(reqId, managerId);
                                Console.WriteLine(success ? "Leave approved." : "Approval failed.");
                            }
                            else if (decision == "R")
                            {
                                success = await managerService.RejectLeaveAsync(reqId, managerId);
                                Console.WriteLine(success ? "Leave rejected." : "Rejection failed.");
                            }
                            break;
                        #endregion



                        case 5: // Exit
                            return;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}