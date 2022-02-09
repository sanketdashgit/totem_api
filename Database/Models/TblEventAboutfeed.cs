using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblEventAboutfeed
    {
        public long FeedId { get; set; }
        public long Id { get; set; }
        public long? CommentId { get; set; }
        public bool Like { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool RequestAccepted { get; set; }
        public bool IsDeleted { get; set; }

        public virtual TblEventComment Comment { get; set; }
    }
}
