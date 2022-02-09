using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblBlockUser
    {
        public long Id { get; set; }
        public long BlockId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool RequestAccepted { get; set; }
        public bool IsDeleted { get; set; }
        public long Blockuserid { get; set; }
    }
}
