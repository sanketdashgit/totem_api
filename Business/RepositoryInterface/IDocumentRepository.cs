
using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Documents;
using Totem.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels.Account;
using Totem.Business.Core.DataTransferModels.Post;

namespace Totem.Business.RepositoryInterface
{
    public interface IDocumentRepository
    {
        //FileUpload in AWS

        Task<ExecutionResult<TestResponceModel>> TestingPathReturn(TestRequestModel fileUpload);
        Task<ExecutionResult> FileUpload(FileUploadRequestModel fileUpload);

        Task<ExecutionResult> EventFileUpload(FileUploadRequestModel fileUpload);

        Task<ExecutionResult<TestResponceModel>> ReadAccessFile(TestRequestModel fileUpload);
        Task<ExecutionResult> TestingUploadfile(TestModelModel fileUpload);

        Task<ExecutionResult> MultiFileUpload(MultiFileUploadRequestModel fileUpload);

        Task<ExecutionResult<UsermanagementDetailsID>> ProfileFileUpload(ProfileFileUploadRequestModel fileUpload);
        //GetFileDetailsByFileId
        ExecutionResult<List<FileUploadResponseModel>> GetFileDetailsByFileId(int FileId);
        //UpdateFileTitle
        Task<ExecutionResult> UpdateFileTitle(FileUploadResponseModel fileUpdate);
        //DeleteFile
        Task<ExecutionResult> DeleteFile(int FileId);

        Task<ExecutionResult> DeleteAllFileIntroduction();
        //FileDetailListBySectionId
        ExecutionResult<PaginatedResponse<TblUserFile>> FileDetailListBySectionId(DocumentListRequestModel listModel);

        /// <summary>
        /// Multiple Images can be save with this method 
        /// </summary>
        /// <param name="fileUpload">Upload file details</param>
        /// <returns>Images added to AWS and made database entry</returns>
        Task<ExecutionResult<PostMediaModel>> MultiFileUploadForCreatePost(CreatePostMultiFileUploadRequestModel fileUpload);
        Task<ExecutionResult<MemorieMediaModel>> MultiFileUploadForCreateMemorie(CreateMemorieMultiFileUploadRequestModel fileUpload);

        /// <summary>
        /// Used to delete media file from Post
        /// </summary>
        /// <param name="removePostMedia">Details to remove post media</param>
        /// <returns>Successfully delete or not</returns>
        Task<ExecutionResult> RemovePostMediaFile(RemovePostMediaModel removePostMedia);
    }
}
