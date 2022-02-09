using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblMemorie
    {
        public TblMemorie()
        {
            TblMemorieFiles = new HashSet<TblMemorieFile>();
        }

        public long MemorieId { get; set; }
        public string Caption { get; set; }
        public long? EventId { get; set; }
        public long Id { get; set; }
        public int IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsPrivate { get; set; }

        public virtual TblEvent Event { get; set; }
        public virtual Usermanagement IdNavigation { get; set; }
        public virtual ICollection<TblMemorieFile> TblMemorieFiles { get; set; }
    }
}
