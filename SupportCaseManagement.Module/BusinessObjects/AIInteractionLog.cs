using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupportCaseManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class AIInteractionLog : BaseObject
    {
        //[Key]
        //public virtual int Id { get; set; }

        public virtual Guid SupportCaseId { get; set; }

        [ForeignKey("SupportCaseId")]
        public virtual SupportCase Case { get; set; }

        [StringLength(100)]
        public virtual string User { get; set; }

        public virtual string UserMessage { get; set; }

        public virtual string AIResponse { get; set; }

        public virtual string ProposedPlan { get; set; }

        public virtual string ExecutedPlan { get; set; }

        public virtual DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}