namespace Totem.Business.Core.DataTransferModels.Post
{
    public class LikeThumbPostModel
    {
        public long PostId { get; set; }
        public long Id { get; set; }
        public bool LikeStatus { get; set; }
        public int LikeType { get; set; }
    }
    public class LikeThumbPostFilesModel
    {
        public long PostLikeId { get; set; }
        public long PostFileId { get; set; }
        public long PostId { get; set; }
        public long Id { get; set; }
        public bool LikeStatus { get; set; }
        public int LikeType { get; set; }
    }
}
