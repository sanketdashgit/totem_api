using System;
using System.Collections.Generic;

namespace Totem.Business.Core.DataTransferModels.Post
{
    public class TblPostModel
    {
        public long PostId { get; set; }
        public string Caption { get; set; }
        public long? EventId { get; set; }

        public bool IsPrivate { get; set; }
        public long Id { get; set; }
        public List<long> TagUserID { get; set; }
        public List<postFileUploadModel> postFiles { get; set; }

    }

   
    public class TblMemorieModel
    {
        public long MemorieId { get; set; }
        public string Caption { get; set; }
        public long? EventId { get; set; }
        public bool IsPrivate { get; set; }
        public long Id { get; set; }
        public List<CreateMemorieFiles> MemorieFiles { get; set; }

    }

    public class CreateMemorieFiles
    {
        public long MemorieFileId { get; set; }
        public long MemorieId { get; set; }
        public string FileName { get; set; }
        public string MediaType { get; set; }
        public string Video { get; set; }

    }
   
}
