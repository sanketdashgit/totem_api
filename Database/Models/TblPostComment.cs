using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblPostComment
    {
        public TblPostComment()
        {
            TblPostCommentReplies = new HashSet<TblPostCommentReply>();
            TblPostCommentfeeds = new HashSet<TblPostCommentfeed>();
        }

        public long PostCommentId { get; set; }
        public string Comment { get; set; }
        public long? Reply { get; set; }
        public long PostId { get; set; }
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Usermanagement IdNavigation { get; set; }
        public virtual TblPost Post { get; set; }
        public virtual ICollection<TblPostCommentReply> TblPostCommentReplies { get; set; }
        public virtual ICollection<TblPostCommentfeed> TblPostCommentfeeds { get; set; }
    }
}
