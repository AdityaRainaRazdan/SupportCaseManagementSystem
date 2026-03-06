using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        }

        //[Key]
        //public virtual int Id { get; set; }

        [Required, StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual string CaseNumber { get; set; } // Auto-generated

        [Required, StringLength(200)]
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

        [NotMapped]
        public virtual bool IsOverdue => DueDate.HasValue && DateTime.UtcNow > DueDate.Value;

        // Navigation properties
        public virtual ICollection<CaseComment> Comments { get; set; }
        public virtual ICollection<CaseKnowledgeLink> KnowledgeLinks { get; set; }
        public virtual ICollection<CaseActionHistory> ActionHistory { get; set; }
    }
}