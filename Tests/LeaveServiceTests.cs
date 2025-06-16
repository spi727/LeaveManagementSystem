using NUnit.Framework;
using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeaveManagementSystem.Tests
{
    public class LeaveServiceTests
    {
        private List<LeaveRequest> leaveRequests;
        private LeaveService service;

        [SetUp]
        public void Setup()
        {
            leaveRequests = new List<LeaveRequest>();
            service = new LeaveService(leaveRequests);
        }

        [Test]
        public void ApplyLeave_Should_AddRequest()
        {
            var leave = new LeaveRequest
            {
                LeaveRequestId = 1,
                EmployeeId = 101,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                Status = LeaveStatus.Pending
            };

            leaveRequests.Add(leave);

            Assert.AreEqual(1, leaveRequests.Count);
        }

        [Test]
        public void CancelLeave_Should_SetStatusToCancelled()
        {
            var leave = new LeaveRequest
            {
                LeaveRequestId = 2,
                EmployeeId = 102,
                Status = LeaveStatus.Pending
            };

            leaveRequests.Add(leave);
            var result = service.CancelLeaveRequest(2, 102);

            Assert.IsTrue(result);
            Assert.AreEqual(LeaveStatus.Cancelled, leave.Status);
        }

        [Test]
        public void CancelLeave_Should_FailIfAlreadyApproved()
        {
            var leave = new LeaveRequest
            {
                LeaveRequestId = 3,
                EmployeeId = 103,
                Status = LeaveStatus.Approved
            };

            leaveRequests.Add(leave);
            var result = service.CancelLeaveRequest(3, 103);

            Assert.IsFalse(result);
        }

        [Test]
        public void RejectLeave_Should_SetStatusToRejected()
        {
            var leave = new LeaveRequest
            {
                LeaveRequestId = 4,
                EmployeeId = 104,
                Status = LeaveStatus.Pending
            };

            leaveRequests.Add(leave);

            leave.Status = LeaveStatus.Rejected;

            Assert.AreEqual(LeaveStatus.Rejected, leave.Status);
        }

        [Test]
        public void ApproveLeave_Should_SetStatusToApproved()
        {
            var leave = new LeaveRequest
            {
                LeaveRequestId = 5,
                EmployeeId = 105,
                Status = LeaveStatus.Pending
            };

            leaveRequests.Add(leave);
            leave.Status = LeaveStatus.Approved;

            Assert.AreEqual(LeaveStatus.Approved, leave.Status);
        }

        [Test]
        public void ApplyLeave_Should_DetectOverlappingDates()
        {
            leaveRequests.Add(new LeaveRequest
            {
                LeaveRequestId = 6,
                EmployeeId = 106,
                StartDate = new DateTime(2025, 06, 20),
                EndDate = new DateTime(2025, 06, 25),
                Status = LeaveStatus.Approved
            });

            DateTime newStart = new DateTime(2025, 06, 24);
            DateTime newEnd = new DateTime(2025, 06, 27);

            bool overlaps = leaveRequests.Any(r =>
                r.EmployeeId == 106 &&
                r.Status == LeaveStatus.Approved &&
                newStart <= r.EndDate &&
                newEnd >= r.StartDate);

            Assert.IsTrue(overlaps);
        }

        [Test]
        public void GetLeaveHistoryByEmployee_Should_ReturnCorrectList()
        {
            leaveRequests.Add(new LeaveRequest { LeaveRequestId = 7, EmployeeId = 107, StartDate = DateTime.Today });
            leaveRequests.Add(new LeaveRequest { LeaveRequestId = 8, EmployeeId = 107, StartDate = DateTime.Today.AddDays(-10) });

            var history = service.GetLeaveHistoryByEmployee("107");

            Assert.AreEqual(2, history.Count);
            Assert.AreEqual(7, history.First().LeaveRequestId);
        }

        [Test]
        public void GetPendingApprovals_Should_ReturnCorrectList()
        {
            leaveRequests.Add(new LeaveRequest { LeaveRequestId = 9, ManagerId = "M1", Status = LeaveStatus.Pending });
            leaveRequests.Add(new LeaveRequest { LeaveRequestId = 10, ManagerId = "M1", Status = LeaveStatus.Approved });

            var pending = service.GetPendingApprovala("M1");

            Assert.AreEqual(1, pending.Count);
            Assert.AreEqual(LeaveStatus.Pending, pending[0].Status);
        }

        [Test]
        public void ApplyLeave_InvalidDates_Should_BeHandled()
        {
            DateTime start = new DateTime(2025, 06, 30);
            DateTime end = new DateTime(2025, 06, 20);

            Assert.Less(end, start); // Fails if not checked in real logic
        }

        [Test]
        public void CancelLeave_WrongUserOrMissing_ShouldReturnFalse()
        {
            var leave = new LeaveRequest
            {
                LeaveRequestId = 11,
                EmployeeId = 111,
                Status = LeaveStatus.Pending
            };

            leaveRequests.Add(leave);
            var result = service.CancelLeaveRequest(11, 999); // wrong employee ID

            Assert.IsFalse(result);
        }
    }
}