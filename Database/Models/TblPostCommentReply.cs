using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblPostCommentReply
    {
        public long ReplyId { get; set; }
        public long PostCommentId { get; set; }
        public long Id { get; set; }
        public string ReplyBody { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual TblPostComment PostComment { get; set; }
    }
}
