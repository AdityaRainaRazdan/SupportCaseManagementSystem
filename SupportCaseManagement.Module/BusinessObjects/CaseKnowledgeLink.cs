using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupportCaseManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class CaseKnowledgeLink : BaseObject
    {
        //[Key]
        //public virtual int Id { get; set; }

        [Required]
        public virtual Guid SupportCaseId { get; set; }

        [ForeignKey("SupportCaseId")]
        public virtual SupportCase Case { get; set; }

        [Required]
        public virtual Guid KnowledgeBaseArticleId { get; set; }

        [ForeignKey("KnowledgeBaseArticleId")]
        public virtual KnowledgeBaseArticle Article { get; set; }
    }
}