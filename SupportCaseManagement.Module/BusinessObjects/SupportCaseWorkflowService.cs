using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using SupportCaseManagement.Module.BusinessObjects;

namespace SupportCaseManagement.Module.Services
{
    public class SupportCaseWorkflowService
    {
        private readonly IObjectSpaceProvider objectSpaceProvider;

        //public SupportCaseWorkflowService(IObjectSpaceProvider objectSpaceProvider)
        //{
        //    this.objectSpaceProvider = objectSpaceProvider;
        //}

        // Allowed workflow transitions
        private static readonly Dictionary<CaseStatus, CaseStatus[]> AllowedTransitions = new()
        {
            { CaseStatus.New, new[] { CaseStatus.Triage } },
            { CaseStatus.Triage, new[] { CaseStatus.InProgress } },
            { CaseStatus.InProgress, new[] { CaseStatus.WaitingCustomer, CaseStatus.Resolved } },
            { CaseStatus.WaitingCustomer, new[] { CaseStatus.InProgress } },
            { CaseStatus.Resolved, new[] { CaseStatus.Closed } }
        };

        // SLA due days mapping
        private static readonly Dictionary<CasePriority, int> PrioritySLA = new()
        {
            { CasePriority.P1, 1 },
            { CasePriority.P2, 3 },
            { CasePriority.P3, 7 }
        };

        public bool CanTransition(SupportCase supportCase, CaseStatus newStatus)
        {
            return AllowedTransitions.TryGetValue(supportCase.Status, out var allowed) && allowed.Contains(newStatus);
        }

        public void ChangeStatus(SupportCase supportCase, CaseStatus newStatus, string performedBy)
        {
            if (!CanTransition(supportCase, newStatus))
                throw new InvalidOperationException($"Cannot transition from {supportCase.Status} to {newStatus}");

            var oldStatus = supportCase.Status;
            supportCase.Status = newStatus;

            // Log action history
            var history = new CaseActionHistory
            {
                Case = supportCase,
                ActionType = "StatusChange",
                OldValue = oldStatus.ToString(),
                NewValue = newStatus.ToString(),
                PerformedBy = performedBy,
                Timestamp = DateTime.UtcNow
            };
            supportCase.ActionHistory.Add(history);
        }

        public void ChangePriority(SupportCase supportCase, CasePriority newPriority, string performedBy)
        {
            var oldPriority = supportCase.Priority;
            supportCase.Priority = newPriority;

            // Recalculate due date based on SLA
            supportCase.DueDate = DateTime.UtcNow.AddDays(PrioritySLA[newPriority]);

            // Log action history
            var history = new CaseActionHistory
            {
                Case = supportCase,
                ActionType = "PriorityChange",
                OldValue = oldPriority.ToString(),
                NewValue = newPriority.ToString(),
                PerformedBy = performedBy,
                Timestamp = DateTime.UtcNow
            };
            supportCase.ActionHistory.Add(history);
        }

        public void RecalculateDueDate(SupportCase supportCase)
        {
            if (PrioritySLA.TryGetValue(supportCase.Priority, out var days))
            {
                supportCase.DueDate = supportCase.CreatedDate.AddDays(days);
            }
        }
    }
}