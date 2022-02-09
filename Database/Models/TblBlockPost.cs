using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblBlockPost
    {
        public long Id { get; set; }
        public long BlockId { get; set; }
        public long PostuserId { get; set; }
        public long PostId { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool RequestAccepted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
