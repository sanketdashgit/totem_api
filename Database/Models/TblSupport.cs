using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblSupport
    {
        public long SupportId { get; set; }
        public long Id { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Usermanagement IdNavigation { get; set; }
    }
}
