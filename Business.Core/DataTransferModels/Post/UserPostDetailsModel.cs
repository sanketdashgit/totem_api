using System;
using System.Collections.Generic;

namespace Totem.Business.Core.DataTransferModels.Post
{
    public class UserPostDetailsModel
    {
        public long PostId { get; set; }
        public string Caption { get; set; }
        public bool IsPrivate { get; set; }
        public long EventId { get; set; }
        public string EventName { get; set; }
        public int NoOfLikes { get; set; }
        public int NoOfThumbs { get; set; }
        public int NoOfComments { get; set; }
        public bool SelfLiked { get; set; }
        public bool SelfThumbed { get; set; }
        public bool SelfCommented { get; set; }
        public List<PostMediaModel> PostMediaLinks { get; set; }
    }

    public class UserMemorysDetailsModel
    {
        public long MemorieId { get; set; }
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string Caption { get; set; }
        public bool IsPrivate { get; set; }
        public long EventId { get; set; }
        public string EventName { get; set; }
        public DateTime? StartDate { get; set; }
        public List<MemorieMediaModel> MemorieMediaLinks { get; set; }
    }
}



