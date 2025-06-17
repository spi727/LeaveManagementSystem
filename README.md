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
Member 1	Initialize project structure in Visual Studio and create models for Employee, LeaveRequest, and Manager. Setup Git repository.=>[Praveen Sagar Kyasani]                  
Member 2	Implement leave application logic (Apply, Cancel) using OOP principles and proper control flow.=>[Divyamshini Rebba]          
Member 3	Add async file operations to store leave requests and user data in a persistent JSON or text format.=> [Rishikesh]                      
Member 4	Develop manager actions logic to approve/reject leave requests, with validations.=> [Praveen Hatti]
Member 5	Use LINQ to generate leave history for an employee and view pending approvals by the manager.=>[Pritish Priyadasan]                               
Member 6	Write unit tests for all business logic scenarios (e.g., apply leave, cancel, reject, overlapping dates).=> [Pravardhan Pandana]
               

##  Structure
- `Program.cs` - Main logic
- `Models/` - Employee, Manager, LeaveRequest
- `Services/` - LeaveService
- `Tests/` - NUnit test cases

##  Sprint 1 Goal

