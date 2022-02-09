using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblEventComment
    {
        public TblEventComment()
        {
            TblEventAboutfeeds = new HashSet<TblEventAboutfeed>();
            TblEventCommentsReplies = new HashSet<TblEventCommentsReply>();
        }

        public long CommentId { get; set; }
        public long Id { get; set; }
        public long EventId { get; set; }
        public string CommentBody { get; set; }
        public string ReplyBody { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool RequestAccepted { get; set; }
        public bool IsDeleted { get; set; }

        public virtual TblEvent Event { get; set; }
        public virtual ICollection<TblEventAboutfeed> TblEventAboutfeeds { get; set; }
        public virtual ICollection<TblEventCommentsReply> TblEventCommentsReplies { get; set; }
    }
}
