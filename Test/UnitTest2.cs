using NUnit.Framework;
using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
#region Member8
namespace LeaveManagementSystem.Test
{
    public class LeaveServiceAdditionalTests
    {
        private LeaveService _service;
        private List<LeaveRequest> _testRequests;

        [SetUp]
        public void Setup()
        {
            _testRequests = new List<LeaveRequest>();
            _service = new LeaveService();
            _service.GetType().GetField("_leaveRequests",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance)
                ?.SetValue(_service, _testRequests);
        }

        [Test]
        public void ApplyLeave_Should_Throw_WhenRequestDatesAreInThePast()
        {
            var leave = new LeaveRequest
            {
                EmployeeId = 200,
                StartDate = DateTime.Today.AddDays(-5),
                EndDate = DateTime.Today.AddDays(-2),
                Reason = "Invalid Past Leave"
            };

            try
            {
                _service.ApplyLeave(leave);
                throw new Exception("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Expected behavior
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected exception type: " + ex.GetType().Name);
            }
        }

        [Test]
        public void CancelLeave_Should_Fail_WhenRequestNotFound()
        {
            var result = _service.CancelLeaveRequest(999, 999);
            if (result != false)
                throw new Exception("Expected false for non-existent request, but got true.");
        }

        [Test]
        public void ApproveRequest_Should_Fail_WhenLeaveAlreadyRejected()
        {
            var leave = new LeaveRequest
            {
                LeaveRequestId = 301,
                EmployeeId = 201,
                ApprovedByManagerId = 2,
                Status = LeaveStatus.Rejected
            };
            _testRequests.Add(leave);

            var result = _service.ApproveRequest(301, 2);
            if (result != false)
                throw new Exception("ApproveRequest should fail if leave is already rejected.");
        }

        [Test]
        public void RejectRequest_Should_Fail_WhenRequestIsMissing()
        {
            var result = _service.RejectRequest(404, 1);
            if (result != false)
                throw new Exception("Expected false when rejecting a missing request.");
        }

        [Test]
        public void ApplyLeave_Should_Succeed_WhenStartAndEndDateSame()
        {
            var leave = new LeaveRequest
            {
                EmployeeId = 202,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(1),
                Reason = "One-day event"
            };

            _service.ApplyLeave(leave);

            if (_testRequests.Count != 1)
                throw new Exception("Expected 1 leave request in the list.");
            if (_testRequests[0].Status != LeaveStatus.Pending)
                throw new Exception("Expected leave status to be Pending.");
        }

        [Test]
        public void ApproveRequest_Should_Fail_WhenManagerIdDoesNotMatch()
        {
            var leave = new LeaveRequest
            {
                LeaveRequestId = 401,
                EmployeeId = 203,
                ApprovedByManagerId = 5,
                Status = LeaveStatus.Pending
            };
            _testRequests.Add(leave);

            var result = _service.ApproveRequest(401, 999);
            if (result != false)
                throw new Exception("Expected false when manager ID does not match.");
        }

        [Test]
        public void GetPendingApprovals_Should_ReturnEmpty_WhenNoManagerMatches()
        {
            _testRequests.Add(new LeaveRequest
            {
                LeaveRequestId = 402,
                ApprovedByManagerId = 3,
                Status = LeaveStatus.Pending
            });

            var result = _service.GetPendingApprovals(99); // Manager 99 has no requests
            if (result.Any())
                throw new Exception("Expected no pending approvals for manager 99.");
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
#endregion