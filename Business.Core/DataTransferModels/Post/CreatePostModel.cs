using System.Collections.Generic;

namespace Totem.Business.Core.DataTransferModels.Post
{
    public class CreatePostModel
    {
        public long PostId { get; set; }
        public string Caption { get; set; }
        public long EventId { get; set; }
        public long Id { get; set; }
        public List<long> TagUserID { get; set; }
        public List<postFileUploadModel> postFiles { get; set; }
    }

    public class postFileUploadModel
    {
        public long PostFileId { get; set; }
        public long PostId { get; set; }
        public string FileName { get; set; }
        public string MediaType { get; set; }
        public string Video { get; set; }
    }

    public class CreateMemorieModel
    {
        public long MemorieId { get; set; }
        public string Caption { get; set; }
        public long EventId { get; set; }
        public long Id { get; set; }

        public List<CreateMemorieFiles> MemorieFiles { get; set; }
        // public List<long> TagUserID { get; set; }
    }
}
