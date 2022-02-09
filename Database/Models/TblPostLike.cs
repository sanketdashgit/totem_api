using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblPostLike
    {
        public long PostLikeId { get; set; }
        public long PostId { get; set; }
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LikeType { get; set; }

        public virtual Usermanagement IdNavigation { get; set; }
        public virtual TblPost Post { get; set; }
    }
}
