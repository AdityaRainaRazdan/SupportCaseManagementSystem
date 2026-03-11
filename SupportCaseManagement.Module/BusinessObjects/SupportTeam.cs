using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SupportCaseManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class SupportTeam : BaseObject
    {
        public SupportTeam()
        {
            Members = new ObservableCollection<ApplicationUser>();
        }

        [Required]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual ObservableCollection<ApplicationUser> Members { get; set; }
    }
}