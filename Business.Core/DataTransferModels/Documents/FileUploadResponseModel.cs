using System;

namespace Totem.Business.Core.DataTransferModels.Documents
{
    public class FileUploadResponseModel
    {
        public long FileId { get; set; }
        public string Downloadlink { get; set; }
        public string Title { get; set; }
        public string FileType { get; set; }
        public long? AccountId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
