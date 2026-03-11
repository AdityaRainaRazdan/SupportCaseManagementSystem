using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.DC;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.Xpo;
using SupportCaseManagement.Module.BusinessObjects;

namespace SupportCaseManagement.Module.AIBackend
{
    [DefaultClassOptions]
    public class AIInteractionLog : BaseObject
    {
        public virtual Guid SupportCaseId { get; set; }

        //[ForeignKey(nameof(SupportCaseId))]
        //[Association("SupportCase-AIInteractionLogs")]
        public virtual SupportCase Case { get; set; }

        public virtual string UserName { get; set; }
        //public virtual AIAssistantSession Session { get; set; }
        public virtual string UserMessage { get; set; }
        public virtual string ActionExecuted { get; set; }
        public virtual string AIResponse { get; set; }
        public virtual string AIModel { get; set; }
        public virtual bool PlanApplied { get; set; }
        public virtual DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
