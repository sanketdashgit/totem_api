using System.ComponentModel.DataAnnotations;

namespace Totem.Business.Core.DataTransferModels.Documents
{
    public class DocumentListRequestModel //: PaginationBaseRequestModel
    {
        [Required(ErrorMessage = "Id is required field")]
        public int Id { get; set; }
        [Required(ErrorMessage = "FileType is required field")]
        public string FileType { get; set; }
    }
}
