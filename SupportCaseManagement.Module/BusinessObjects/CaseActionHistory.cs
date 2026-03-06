using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupportCaseManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class CaseActionHistory : BaseObject
    {
        //[Key]
        //public virtual int Id { get; set; }

        [Required]
        public virtual Guid SupportCaseId { get; set; }

        [ForeignKey("SupportCaseId")]
        public virtual SupportCase Case { get; set; }

        [Required, StringLength(100)]
        public virtual string ActionType { get; set; } // e.g., StatusChange, PriorityChange

        [StringLength(100)]
        public virtual string OldValue { get; set; }

        [StringLength(100)]
        public virtual string NewValue { get; set; }

        [StringLength(100)]
        public virtual string PerformedBy { get; set; }

        public virtual DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}