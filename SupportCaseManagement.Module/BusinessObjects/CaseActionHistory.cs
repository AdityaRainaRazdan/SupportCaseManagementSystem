using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Xpo;

namespace SupportCaseManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class CaseActionHistory : BaseObject
    {
        public CaseActionHistory() 
        {
            PerformedBy= SecuritySystem.CurrentUserName;
        }
        //[Key]
        //public virtual int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Browsable(false)]
        public virtual Guid SupportCaseId { get; set; }

        [ForeignKey("SupportCaseId")]
        public virtual SupportCase Case { get; set; }

        [System.ComponentModel.DataAnnotations.Required, StringLength(100)]
        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "12")]
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