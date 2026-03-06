using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SupportCaseManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class KnowledgeBaseArticle : BaseObject
    {
        public KnowledgeBaseArticle()
        {
            Cases = new ObservableCollection<CaseKnowledgeLink>();
            LastUpdated = DateTime.UtcNow;
        }
       //[Key]
       // public virtual int Id { get; set; }
 

        [Required, StringLength(200)]
        public virtual string Title { get; set; }

        public virtual string Content { get; set; }

        [StringLength(100)]
        public virtual string Category { get; set; }

        public virtual string Tags { get; set; }

        public virtual bool IsPublished { get; set; }

        public virtual DateTime LastUpdated { get; set; }

        public virtual ICollection<CaseKnowledgeLink> Cases { get; set; }
    }
}