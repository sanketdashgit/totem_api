using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Account
{
    public class FollowersModel
    {
        public int UserID { get; set; }
        public int ID { get; set; }
    }


    public class FollowersCountModel
    {
        public UsermanagementDetailsID ProfileDetails { get; set; }
        public int followers { get; set; }
        public int following { get; set; }
        public int PostCount { get; set; }
        public int isfollow { get; set; }


    }

    public class FollowUsers
    {
        public long Id { get; set; }
        public string Image { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public bool? ProfileVerified { get; set; }
        public bool? IsPrivate { get; set; }
        public int mutualCount { get; set; }
        public int isfollow { get; set; }
        public int PresentLiveStatus { get; set; }

    }

    public class PostLikedUsers : FollowUsers
    {
        public int SelfLiked { get; set; }

    }


    public class FollowModel : PaginationBaseRequestModel
    {
        public long ID { get; set; }
    }

    public class GetFollowModel : PaginationBaseRequestModel
    {
        public long ID { get; set; }
        public string SessionId { get; set; }
    }


    public class GetUserDistence
    {
        public long Id { get; set; }
        public decimal Location { get; set; }
    }

    public class Follow
    {
        public long UserId { get; set; }
        public long FollowerId { get; set; }
        // public bool isFollow { get; set; }

    }

    public class DeleteSuggested
    {
        public long UserId { get; set; }
        public long DeleteUserId { get; set; }
        // public bool isFollow { get; set; }

    }
}
