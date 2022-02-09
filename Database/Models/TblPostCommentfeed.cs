using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblPostCommentfeed
    {
        public long FeedId { get; set; }
        public long Id { get; set; }
        public long? PostCommentId { get; set; }
        public bool Like { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool RequestAccepted { get; set; }
        public bool IsDeleted { get; set; }

        public virtual TblPostComment PostComment { get; set; }
    }
}
