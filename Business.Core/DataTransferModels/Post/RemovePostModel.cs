namespace Totem.Business.Core.DataTransferModels.Post
{
    public class RemovePostModel
    {
        public long Id { get; set; }
        public long PostId { get; set; }
    }

    public class EditPostprivacyModel : RemovePostModel
    {
        public bool IsPrivate { get; set; }
    }

    public class RemoveMemorieModel
    {
        public long Id { get; set; }
        public long MemorieId { get; set; }
    }

    public class EditMemorieprivacyModel : RemoveMemorieModel
    {
        public bool IsPrivate { get; set; }
    }

    public class EditMemorieFileprivacyModel 
    {
        public long MemorieFileId { get; set; }
        public bool IsPrivate { get; set; }
    }


    public class AcceptTagPostModel
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public bool Status { get; set; }
    }
}
