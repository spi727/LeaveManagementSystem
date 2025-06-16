using LeaveManagementSystem.Models;

namespace LeaveManagementSystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var managerService = new ManagerService();

            while (true)
            {
                Console.WriteLine("\n--- Manager Panel ---");
                Console.WriteLine("1. View Pending Leave Requests");
                Console.WriteLine("2. Approve Leave Request");
                Console.WriteLine("3. Reject Leave Request");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        var pendingRequests = await managerService.GetPendingRequestsAsync();
                        if (!pendingRequests.Any())
                        {
                            Console.WriteLine("No pending leave requests.");
                        }
                        else
                        {
                            foreach (var r in pendingRequests)
                            {
                                Console.WriteLine($"ID: {r.RequestId}, Name: {r.EmployeeName}, Dates: {r.StartDate:dd-MM-yyyy} to {r.EndDate:dd-MM-yyyy}");
                            }
                        }
                        break;

                    case "2":
                        Console.Write("Enter Request ID to approve: ");
                        if (int.TryParse(Console.ReadLine(), out int approveId))
                        {
                            bool success = await managerService.ApproveLeaveAsync(approveId);
                            Console.WriteLine(success ? "Leave Approved." : "Invalid request or already processed.");
                        }
                        break;

                    case "3":
                        Console.Write("Enter Request ID to reject: ");
                        if (int.TryParse(Console.ReadLine(), out int rejectId))
                        {
                            Console.Write("Enter rejection reason: ");
                            var reason = Console.ReadLine()!;
                            bool success = await managerService.RejectLeaveAsync(rejectId, reason);
                            Console.WriteLine(success ? "Leave Rejected." : "Invalid request or already processed.");
                        }
                        break;

                    case "4":
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}
