using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Post
{
    public class GetPostmodel : PaginationBaseRequestModel
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public long GetSelf { get; set; }

    }

    public class GetPostLikeUsers : PaginationBaseRequestModel
    {
        public long Id { get; set; }

        public long PostId { get; set; }

    }

    public class GetPostID 
    {
        public long Id { get; set; }
    }

    public class GetMemorieID:PaginationBaseRequestModel
    {
        public long Id { get; set; }
    }

    public class GetByPostID
    {
        public long PostId { get; set; }
    }
}
