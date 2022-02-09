namespace Totem.Business.Core.DataTransferModels.Post
{
    public class PostMediaModel
    {
        public long PostFileId { get; set; }
        public string Downloadlink { get; set; }
        public string MediaType { get; set; }
        public string Videolink { get; set; }
    }

    public class MemorieMediaModel
    {
        public long MemorieFileId { get; set; }
        public string Downloadlink { get; set; }
        public string MediaType { get; set; }
        public string Videolink { get; set; }
        public bool? IsPrivate { get; set; }
    }
}
