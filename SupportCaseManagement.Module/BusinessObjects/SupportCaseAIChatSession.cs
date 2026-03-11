//using System;
//using System.Collections.ObjectModel;
//using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl.EF;
//using DevExpress.Xpo;

//namespace SupportCaseManagement.Module.BusinessObjects
//{
//    [DefaultClassOptions]
//    public class SupportCaseAIChatSession : BaseObject
//    {
//        public SupportCaseAIChatSession()
//        {
//            CreatedDate = DateTime.UtcNow;
//            Messages = new ObservableCollection<AIChatMessage>();
//        }

//        //[Key]
//        //public virtual int Id { get; set; }

//        public virtual string SessionName { get; set; } = $"AI Chat {DateTime.UtcNow:yyyyMMddHHmmss}";

//        public virtual DateTime CreatedDate { get; set; }

//        public virtual SupportCase RelatedCase { get; set; }

//        [Association("Session-Messages")]
//        public virtual ObservableCollection<AIChatMessage> Messages { get; set; }
//    }

//    [DefaultClassOptions]
//    public class AIChatMessage : BaseObject
//    {
//        public AIChatMessage()
//        {
//            Timestamp = DateTime.UtcNow;
//        }

//        //[Key]
//        //public virtual int Id { get; set; }

//        public virtual string Text { get; set; }

//        public virtual bool IsAI { get; set; }

//        public virtual DateTime Timestamp { get; set; }

//        public virtual SupportCaseAIChatSession Session { get; set; }
//    }
//}