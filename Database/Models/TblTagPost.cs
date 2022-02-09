using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblTagPost
    {
        public long PostId { get; set; }
        public long TagUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long TagId { get; set; }
        public int RequestAccepted { get; set; }
    }
}
