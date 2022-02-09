using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels.Account;
using Totem.Business.Core.DataTransferModels.Event;

namespace Totem.Business.Core.DataTransferModels.Post
{
    public class PostDetailsModel 
    {
        public long PostId { get; set; }
        public string Caption { get; set; }
        public int NoOfLikes { get; set; }
        public int NoOfThumbs { get; set; }
        public int NoOfComments { get; set; }
        public int SelfLiked { get; set; }
        public bool SelfThumbed { get; set; }
        public bool SelfCommented { get; set; }
        public List<PostMediaModel> PostMediaLinks { get; set; }
        public long EventId { get; set; }
        public long Id { get; set; }
        public int IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPrivate { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public bool? ProfileVerified { get; set; }
        public string Image { get; set; }
        public bool? UserIsActive { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }
        public DateTime EventCreatdate { get; set; }
        public int EventisActive { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string VanueId { get; set; }
        public List<TblEventFile> EventImages { get; set; }
        public List<PostCommentModel> PostComments { get; set; }
        public List<FollowUsers> TagUsers { get; set; }

    }
}
