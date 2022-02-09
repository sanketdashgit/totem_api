using System;
using System.Collections.Generic;

namespace Totem.Business.Core.DataTransferModels.Post
{
    public class LikeStatusModel
    {
        public long PostId { get; set; }
        public int TotalLikes { get; set; }
        public int TotalThumbs { get; set; }
        public int SelfLiked { get; set; }
        public bool SelfThumbed { get; set; }
       
    }

    public class FileLikeStatusModel
    {
        public long PostId { get; set; }
        public long PostFileId { get; set; }
        public int TotalLikes { get; set; }
        public int TotalThumbs { get; set; }
        public int SelfLiked { get; set; }
        public bool SelfThumbed { get; set; }
       
    }

    public class spExplorepostfilesModel
    {
        public long? PostFileId { get; set; }
        public string? Downloadlink { get; set; }
        public string? MediaType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Videolink { get; set; }
        public long LikeCount { get; set; }
        public long? PostId { get; set; }
        public string? Caption { get; set; }
        public long? EventId { get; set; }
        public long? Id { get; set; }
        public int? IsActive { get; set; }
        public bool? IsPrivate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }
        public int LikeType { get; set; }



    }


}
