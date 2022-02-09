using System.Collections.Generic;
using Totem.Business.Core.DataTransferModels.Event;

namespace Totem.Business.Core.DataTransferModels.Post
{
    public class PostCommentModel
    {
        public long PostCommentId { get; set; }
        public long PostId { get; set; }
        public long Id { get; set; }
        public string Comment { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public bool ProfileVerified { get; set; } 
        public string Image { get; set; }
        public bool likeStatus { get; set; }
        public long TotalLike { get; set; }
        public List<UserDataforReply> ReplyBody { get; set; }
    }

    public class AddPostCommentModel
    {
        public long PostCommentId { get; set; }
        public long PostId { get; set; }
        public long Id { get; set; }
        public string Comment { get; set; }        
    }

    public class RemovePostCommentModel
    {
        public long PostCommentId { get; set; }        
        public long Id { get; set; }
    }

    public class RemovePostCommentReplyModel
    {
        public long ReplyId { get; set; }
        public long Id { get; set; }
    }

    //Post Reply on comment 
    public partial class PostCommentReply
    {
        public long ReplyID { get; set; }
        public long PostCommentId { get; set; }
        public long Id { get; set; }
        public string ReplyBody { get; set; }
    }
}
