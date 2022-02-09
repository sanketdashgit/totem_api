using System.Collections.Generic;
using Totem.Business.Core.DataTransferModels.Account;

namespace Totem.Business.Core.DataTransferModels.Post
{
    public class UserPostsModel
    {
        public long PostId { get; set; }

        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string Caption { get; set; }
        public long EventId { get; set; }
        public bool IsPrivate { get; set; }
        public List<PostMediaModel> PostMediaLinks { get; set; }
    }

    public class AdminUserPostsModel
    {
        public long PostId { get; set; }
        public string Caption { get; set; }
        public long? EventId { get; set; }
        public long Id { get; set; }
        public int IsActive { get; set; }       
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public bool? ProfileVerified { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public string Image { get; set; }
        public List<PostMediaModel> PostMediaLinks { get; set; }
        public List<FollowUsers> BlockedUsers { get; set; }
        public int? BlockedCount { get; set; }
    }
}
