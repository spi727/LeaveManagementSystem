using NUnit.Framework;
using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeaveManagementSystem.Tests
{
    public class LeaveServiceTests
    {
        private LeaveService _service;
        private List<LeaveRequest> _testRequests;

        [SetUp]
        public void Setup()
        {
            _testRequests =  new List<LeaveRequest>();
            _service = new LeaveService();
            _service.GetType().GetField("_leaveRequests",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance)
                ?.SetValue(_service, _testRequests);
        }

        [Test]
        public void ApplyLeave_Should_AddRequest()
        {
            var leave = new LeaveRequest
            {
                EmployeeId = 101,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                Reason = "Vacation"
            };

            _service.ApplyLeave(leave);

            Assert.AreEqual(1, _testRequests.Count);
            Assert.AreEqual(LeaveStatus.Pending, _testRequests[0].Status);
        }

        [Test]
        public void ApplyLeave_Should_ThrowForOverlappingDates()
        {
            _testRequests.Add(new LeaveRequest
            {
                EmployeeId = 102,
                StartDate = new DateTime(2025, 06, 20),
                EndDate = new DateTime(2025, 06, 25),
                Status = LeaveStatus.Approved
            });

            var newLeave = new LeaveRequest
            {
                EmployeeId = 102,
                StartDate = new DateTime(2025, 06, 24),
                EndDate = new DateTime(2025, 06, 27),
                Reason = "Overlapping"
            };

            Assert.Throws<InvalidOperationException>(() => _service.ApplyLeave(newLeave));
        }

        [Test]
        public void CancelLeave_Should_SetStatusToCancelled()
        {
            var leave = new LeaveRequest
            {
                LeaveRequestId = 1,
                EmployeeId = 103,
                Status = LeaveStatus.Pending
            };
            _testRequests.Add(leave);

            var result = _service.CancelLeaveRequest(1, 103);

            Assert.IsTrue(result);
            Assert.AreEqual(LeaveStatus.Cancelled, leave.Status);
        }

        [Test]
        public void CancelLeave_Should_FailIfAlreadyApproved()
        {
            var leave = new LeaveRequest
            {
                LeaveRequestId = 2,
                EmployeeId = 104,
                Status = LeaveStatus.Approved
            };
            _testRequests.Add(leave);

            var result = _service.CancelLeaveRequest(2, 104);

            Assert.IsFalse(result);
        }

        [Test]
        public void ApproveRequest_Should_SetStatusToApproved()
        {
            var leave = new LeaveRequest
            {
                LeaveRequestId = 3,
                EmployeeId = 105,
                ApprovedByManagerId = 1,
                Status = LeaveStatus.Pending
            };
            _testRequests.Add(leave);

            var result = _service.ApproveRequest(3, 1);

            Assert.IsTrue(result);
            Assert.AreEqual(LeaveStatus.Approved, leave.Status);
        }

        [Test]
        public void RejectRequest_Should_SetStatusToRejected()
        {
            var leave = new LeaveRequest
            {
                LeaveRequestId = 4,
                EmployeeId = 106,
                ApprovedByManagerId = 1,
                Status = LeaveStatus.Pending
            };
            _testRequests.Add(leave);

            var result = _service.RejectRequest(4, 1);

            Assert.IsTrue(result);
            Assert.AreEqual(LeaveStatus.Rejected, leave.Status);
        }

        [Test]
        public void GetLeaveHistoryByEmployee_Should_ReturnCorrectList()
        {
            _testRequests.Add(new LeaveRequest
            {
                LeaveRequestId = 5,
                EmployeeId = 107,
                StartDate = DateTime.Today
            });
            _testRequests.Add(new LeaveRequest
            {
                LeaveRequestId = 6,
                EmployeeId = 107,
                StartDate = DateTime.Today.AddDays(-10)
            });

            var history = _service.GetLeaveHistoryByEmployee(107);

            Assert.AreEqual(2, history.Count);
            Assert.AreEqual(5, history[0].LeaveRequestId); // Should be ordered by date
        }

        [Test]
        public void GetPendingApprovals_Should_ReturnCorrectList()
        {
            _testRequests.Add(new LeaveRequest
            {
                LeaveRequestId = 7,
                ApprovedByManagerId = 1,
                Status = LeaveStatus.Pending
            });
            _testRequests.Add(new LeaveRequest
            {
                LeaveRequestId = 8,
                ApprovedByManagerId = 1,
                Status = LeaveStatus.Approved
            });

            var pending = _service.GetPendingApprovals(1);

            Assert.AreEqual(1, pending.Count);
            Assert.AreEqual(7, pending[0].LeaveRequestId);
        }
        [TearDown]
        public void TearDown()
        {
            _service?.Dispose();
            _service = null;
            _testRequests = null;
        }

    }
}