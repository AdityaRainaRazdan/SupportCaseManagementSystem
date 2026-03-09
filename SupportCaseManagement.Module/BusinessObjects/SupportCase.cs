using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Xpo;

namespace SupportCaseManagement.Module.BusinessObjects
{
    public enum CaseStatus
    {
        New,
        Triage,
        InProgress,
        WaitingCustomer,
        Resolved,
        Closed
    }

    public enum CasePriority
    {
        P1,
        P2,
        P3
    }

    [DefaultClassOptions]
    public class SupportCase : BaseObject
    {
        public SupportCase()
        {
            CreatedDate = DateTime.UtcNow;
            Status = CaseStatus.New;
            Comments = new ObservableCollection<CaseComment>();
            KnowledgeLinks = new ObservableCollection<CaseKnowledgeLink>();
            ActionHistory = new ObservableCollection<CaseActionHistory>();
            Title = $"Case {DateTime.UtcNow:yyyyMMddHHmmss}";
            CaseNumber = $"CASE-{DateTime.Now:yyyyMMddHHmmss}";
        }
        public override void OnSaving()
        {
            base.OnSaving();

            DateTime? newDueDate = null;

            switch (Priority)
            {
                case CasePriority.P1:
                    newDueDate = CreatedDate.AddDays(1);
                    break;

                case CasePriority.P2:
                    newDueDate = CreatedDate.AddDays(3);
                    break;

                case CasePriority.P3:
                    newDueDate = CreatedDate.AddDays(7);
                    break;
            }

            // If SLA changed → update due date and log history
            if (newDueDate.HasValue && DueDate != newDueDate)
            {
                DueDate = newDueDate;

                var history = ObjectSpace.CreateObject<CaseActionHistory>();
                history.Case = this;
                history.ActionType = $"SLA recalculated because priority changed to {Priority}. New Due Date: {DueDate:yyyy-MM-dd}";
                history.Timestamp = DateTime.UtcNow;
            }
        }
        //[Key]
        //public virtual int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required, StringLength(50)]

        public virtual string CaseNumber { get; set; } // Auto-generated

        [System.ComponentModel.DataAnnotations.Required, StringLength(200)]
        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual CaseStatus Status { get; set; }

        public virtual CasePriority Priority { get; set; }

        [StringLength(100)]
        public virtual string Category { get; set; }

        [StringLength(100)]
        public virtual string CreatedBy { get; set; }

        [StringLength(100)]
        public virtual string AssignedTo { get; set; }

        [StringLength(100)]
        public virtual string AssignedTeam { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual DateTime? DueDate { get; set; }
        [NonPersistent]
        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "6")]

        public virtual string ChatInput { get; set; }

        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "12")]
        [ModelDefault("AllowEdit", "False")]
        public virtual string ChatHistory { get; set; }

        [NotMapped]
        public virtual bool IsOverdue => DueDate.HasValue && DateTime.UtcNow > DueDate.Value;

        // Navigation properties
        public virtual ICollection<CaseComment> Comments { get; set; }
        public virtual ICollection<CaseKnowledgeLink> KnowledgeLinks { get; set; }
        public virtual ICollection<CaseActionHistory> ActionHistory { get; set; }

        [DevExpress.Xpo.Association("SupportCase-AIInteractionLogs")]
        public virtual ObservableCollection<AIInteractionLog> AIInteractionLogs { get; set; }

    }
}