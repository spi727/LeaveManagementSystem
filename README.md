# LeaveManagementSystem
#  Leave Management System

A cloud-integrated leave management platform built with **C# 10** and **.NET 6**.

##  Features
- Employees can apply, cancel, and view leave history
- Managers can approve/reject leave requests
- JSON-based persistence (Sprint 1)
- LINQ & async/await support
- Unit tested with NUnit

##  Tech Stack
- C# 10, .NET 6
- JSON File Handling (Sprint 1)
- EF Core + SQL Server (future)
- Azure Functions, Logic Apps (future)
- GitHub for Collaboration

## Team Members & Tasks
Member 1	? Done — Project initialized, models created, GitHub repo set up
Member 2	Implement Leave Application logic (Apply functionality with validation)
Member 3	Implement Leave Cancellation logic (Cancel pending leave with checks)
Member 4	Add async file operations to save leave requests to JSON
Member 5	Add async file operations to load leave requests from JSON
Member 6	Implement Manager Actions (Approve/Reject with validations)
Member 7	Use LINQ to:
						View leave history for employee
						View manager's pending approvals |
Member 8	Write unit tests for business logic (Apply, Cancel, Approve, Reject) |

##  Structure
- `Program.cs` - Main logic
- `Models/` - Employee, Manager, LeaveRequest
- `Services/` - LeaveService
- `Tests/` - NUnit test cases

##  Sprint 1 Goal

