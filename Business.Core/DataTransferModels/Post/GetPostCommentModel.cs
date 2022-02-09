using System.Collections.Generic;
using Totem.Business.Core.DataTransferModels.Event;

namespace Totem.Business.Core.DataTransferModels.Post
{
    public class GetPostCommentModel : PaginationBaseRequestModel
    {
        public long PostId { get; set; }
        public long Id { get; set; }
       
    }

    public partial class GetPostCommentsModel
    {
        public long PostId { get; set; }
        public string Id { get; set; }
        public List<UserGetComments> UsersEventsComments { get; set; }
    }
}
