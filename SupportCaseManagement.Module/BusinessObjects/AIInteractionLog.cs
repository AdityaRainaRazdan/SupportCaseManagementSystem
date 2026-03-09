using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.DC;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.Xpo;

namespace SupportCaseManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class AIInteractionLog : BaseObject
    {
        public virtual Guid SupportCaseId { get; set; }

    [ForeignKey(nameof(SupportCaseId))]
        [Association("SupportCase-AIInteractionLogs")]
        public virtual SupportCase Case { get; set; }

        public virtual string UserName { get; set; }

        public virtual string UserMessage { get; set; }

        public virtual string AIResponse { get; set; }

        public virtual DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
