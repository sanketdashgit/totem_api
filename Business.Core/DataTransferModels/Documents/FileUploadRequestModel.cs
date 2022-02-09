using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Totem.Business.Core.DataTransferModels.Documents
{
    public class FileUploadRequestModel
    {
        // [Required(ErrorMessage = "FileName is required field")]
        public IFormFile FileName { get; set; }
        // [Required(ErrorMessage = "Title is required field")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Id is required field")]
        public long Id { get; set; }
        [Required(ErrorMessage = "DocumentType is required field")]
        public string DocumentType { get; set; }
    }


    public class TestModelModel
    {
        // [Required(ErrorMessage = "FileName is required field")]
        public IFormFile FileName { get; set; }
        // [Required(ErrorMessage = "Title is required field")]
        public string Url { get; set; }
        
    }

    public class MultiFileUploadRequestModel
    {
        // [Required(ErrorMessage = "FileName is required field")]
        public List<IFormFile> FileName { get; set; }
        // [Required(ErrorMessage = "Title is required field")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Id is required field")]
        public long Id { get; set; }
        //[Required(ErrorMessage = "DocumentType is required field")]
        public string DocumentType { get; set; }
    }

    public class CreatePostMultiFileUploadRequestModel
    {
        public long PostId { get; set; }
        public IFormFile FileName { get; set; }
        public string MediaType { get; set; }
        public IFormFile Video { get; set; }
    }

    public class CreateMemorieMultiFileUploadRequestModel
    {
        public long MemorieId { get; set; }
        public IFormFile FileName { get; set; }
        public string MediaType { get; set; }
        public IFormFile Video { get; set; }
    }

    public class CreatePostFileDeleteRequestModel
    {
        public long PostFileId { get; set; }

    }

    public class TestRequestModel
    {
        public string FileName { get; set; }
    }

    public class TestResponceModel
    {
        public string UploadURL { get; set; }
        public string DownloadURL { get; set; }
        public string ACLAccessURL { get; set; }
    }

    public class ProfileFileUploadRequestModel
    {
        // [Required(ErrorMessage = "FileName is required field")]
        public IFormFile FileName { get; set; }
        // [Required(ErrorMessage = "Id is required field")]
        public long Id { get; set; }
        //[Required(ErrorMessage = "DocumentType is required field")]
        public string DocumentType { get; set; }
    }
}
