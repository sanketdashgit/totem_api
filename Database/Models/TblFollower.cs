using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblFollower
    {
        public long UserId { get; set; }
        public long FollowerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool RequestAccepted { get; set; }
        public bool IsDeleted { get; set; }
        public long FId { get; set; }

        public virtual Usermanagement Follower { get; set; }
        public virtual Usermanagement User { get; set; }
    }
}
