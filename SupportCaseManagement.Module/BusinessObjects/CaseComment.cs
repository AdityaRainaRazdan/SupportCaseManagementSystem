using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupportCaseManagement.Module.BusinessObjects
{
    public enum CommentType
    {
        Public,
        Internal,
        AI
    }

    [DefaultClassOptions]
    public class CaseComment : BaseObject
    {
        public CaseComment()
        {
            CreatedDate = DateTime.UtcNow;
        }

        //[Key]
        //public virtual int Id { get; set; }

        [Required]
        public virtual Guid SupportCaseId { get; set; }

        [ForeignKey("SupportCaseId")]
        public virtual SupportCase Case { get; set; }

        [Required]
        public virtual string Text { get; set; }

        public virtual CommentType CommentTypes { get; set; }

        [StringLength(100)]
        public virtual ApplicationUser CreatedBy { get; set; }

        public virtual DateTime CreatedDate { get; set; }
    }
}