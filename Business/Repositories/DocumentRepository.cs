using AutoMapper;
using Totem.Business.Core.AppSettings;
using Totem.Business.Core.Consts;
using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Documents;
using Totem.Business.RepositoryInterface;
using Totem.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels.Account;
using Totem.Business.Core.DataTransferModels.Post;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Totem.Business.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        #region Constructor
        private readonly TotemDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAmazonS3Repository _amazonRepo;
        private readonly AWS _awsSettings;
        public DocumentRepository(TotemDBContext dbContext, IMapper mapper, IOptions<AppSettings> appSettings, IAmazonS3Repository amazonRepo)
        {
            _awsSettings = appSettings.Value.AWS;
            _amazonRepo = amazonRepo;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        #endregion


        #region Profile FileUpload in AWS

        public async Task<ExecutionResult> TestingUploadfile(TestModelModel fileUpload)
        {
            HttpWebRequest httpRequest = WebRequest.Create(fileUpload.Url) as HttpWebRequest;
            httpRequest.Method = "PUT";
            using (Stream dataStream = httpRequest.GetRequestStream())
            {
                IFormFile file = fileUpload.FileName;

                long length = file.Length;
                if (length < 0)
                    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));

                using var fileStream = file.OpenReadStream();
                byte[] bytes = new byte[length];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(bytes, 0, (int)file.Length)) > 0)
                {
                    dataStream.Write(bytes, 0, bytesRead);
                }

            }
            HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse;

            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Document", "Uploaded", "")));
        }

        public async Task<ExecutionResult<TestResponceModel>> ReadAccessFile(TestRequestModel fileUpload)
        {
            TestResponceModel account = new TestResponceModel();
           


            if (!string.IsNullOrWhiteSpace(fileUpload.FileName))
            {


                var awsUpload = await _amazonRepo.ReadAccessFile(fileUpload.FileName);
                if (awsUpload.Errors.Any())
                {
                    return new ExecutionResult<TestResponceModel>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
                }
                //account.UploadURL = awsUpload.Value.UploadURL;

                account.DownloadURL = string.Format(_awsSettings.BaseUrl, fileUpload.FileName);

               

                return account != null ? new ExecutionResult<TestResponceModel>(account) :
          new ExecutionResult<TestResponceModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Uploaded Document")));
                // return new ExecutionResult<UsermanagementDetailsID>(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Document", "uploaded", "")));
            }
            else
                return new ExecutionResult<TestResponceModel>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));

        }
        public async Task<ExecutionResult<TestResponceModel>> TestingPathReturn(TestRequestModel fileUpload)
        {
            if (fileUpload == null)
            {
                return new ExecutionResult<TestResponceModel>();
            }


            TestResponceModel account = new TestResponceModel();
            string fileName = "";

            fileName = "Testing/" + Path.GetFileName(DateTime.UtcNow.ToString("ddmmyyhhss") + fileUpload.FileName);



            if (!string.IsNullOrWhiteSpace(fileName))
            {


                var awsUpload = await _amazonRepo.TestingFile(fileName);
                if (awsUpload.Errors.Any())
                {
                    return new ExecutionResult<TestResponceModel>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
                }
                account.UploadURL = awsUpload.Value.UploadURL;

                account.DownloadURL = string.Format(_awsSettings.BaseUrl, fileName);
                account.ACLAccessURL = fileName;
                return account != null ? new ExecutionResult<TestResponceModel>(account) :
          new ExecutionResult<TestResponceModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Uploaded Document")));
                // return new ExecutionResult<UsermanagementDetailsID>(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Document", "uploaded", "")));
            }
            else
                return new ExecutionResult<TestResponceModel>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
        }
        public async Task<ExecutionResult<UsermanagementDetailsID>> ProfileFileUpload(ProfileFileUploadRequestModel fileUpload)
        {
            if (fileUpload == null)
            {
                return new ExecutionResult<UsermanagementDetailsID>();
            }


            var account = _dbContext.Usermanagements.FirstOrDefault(x => x.Id == fileUpload.Id);
            using (MemoryStream stream = new MemoryStream())
            {
                string fileName = "";
                fileUpload.DocumentType = "Image";
                if (fileUpload.DocumentType.ToString().ToLower() == FileUploadModule.Image.ToString().ToLower())
                {
                    fileName = "Profile/" + Path.GetFileName(DateTime.UtcNow.ToString("ddmmyyhhss") + fileUpload.FileName.FileName);
                }


                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    fileUpload.FileName.CopyTo(stream);

                    var awsUpload = await _amazonRepo.UploadFile(stream, fileName);
                    if (awsUpload.Errors.Any())
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
                    }

                    account.Image = string.Format(_awsSettings.BaseUrl, fileName);
                    _dbContext.SaveChanges();
                    return account != null ? new ExecutionResult<UsermanagementDetailsID>(_mapper.Map<UsermanagementDetailsID>(account)) :
              new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Uploaded Document")));
                    // return new ExecutionResult<UsermanagementDetailsID>(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Document", "uploaded", "")));
                }
            }
            return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
        }
        #endregion


        #region FileDetailListBySectionId
        public ExecutionResult<PaginatedResponse<TblUserFile>> FileDetailListBySectionId(DocumentListRequestModel listModel)
        {
            if (listModel.Id <= 0 || string.IsNullOrWhiteSpace(listModel.FileType))
            {
                return new ExecutionResult<PaginatedResponse<TblUserFile>>();
            }

            PaginatedResponse<TblUserFile> response = new PaginatedResponse<TblUserFile>();
            response.Data = _dbContext.TblUserFiles
                .Where(x => x.Id == listModel.Id && x.FileType.Contains(listModel.FileType))
                                            .ToList();
            return response.Data.Count > 0
                ? new ExecutionResult<PaginatedResponse<TblUserFile>>(response)
                : new ExecutionResult<PaginatedResponse<TblUserFile>>();
        }
        #endregion




        #region GetFileDetailsByFileId
        public ExecutionResult<List<FileUploadResponseModel>> GetFileDetailsByFileId(int FileId)
        {
            if (FileId <= 0 || FileId == null)
            {
                return new ExecutionResult<List<FileUploadResponseModel>>(new ErrorInfo(string.Format(MessageHelper.PassParaMessage, "file id")));
            }

            var fileDetails = _dbContext.TblUserFiles.Where(x => x.FileId == FileId).ToList();

            return fileDetails.Count > 0
                ? new ExecutionResult<List<FileUploadResponseModel>>(_mapper.Map<List<FileUploadResponseModel>>(fileDetails))
                : new ExecutionResult<List<FileUploadResponseModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "file details")));
        }
        #endregion



        #region FileUpload in AWS
        public async Task<ExecutionResult> FileUpload(FileUploadRequestModel fileUpload)
        {
            if (fileUpload == null)
            {
                return new ExecutionResult();
            }
            bool isDuplicate = await _dbContext.TblUserFiles
                .AnyAsync(f => f.Title == fileUpload.Title &&
                f.Id == fileUpload.Id && f.FileType.ToLower() == fileUpload.DocumentType.ToLower());

            if (isDuplicate)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.Document.DuplicateTitle)));
            }

            TblUserFile file = new TblUserFile();
            using (MemoryStream stream = new MemoryStream())
            {
                string fileName = "";

                if (fileUpload.DocumentType.ToString().ToLower() == FileUploadModule.Image.ToString().ToLower())
                {
                    fileName = "CMSImage/" + Path.GetFileName(fileUpload.FileName.FileName);
                }


                if (!string.IsNullOrWhiteSpace(fileName))
                {

                    fileUpload.FileName.CopyTo(stream);
                    var awsUpload = await _amazonRepo.UploadFile(stream, fileName);
                    if (awsUpload.Errors.Any())
                    {
                        return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
                    }
                    file.FileName = fileUpload.FileName.FileName;
                    file.FileType = fileUpload.DocumentType;
                    file.Title = fileUpload.Title;
                    file.Id = fileUpload.Id;
                    file.Downloadlink = string.Format(_awsSettings.BaseUrl, fileName);
                    file.CreatedDate = DateTime.UtcNow;
                    await _dbContext.TblUserFiles.AddAsync(file);
                    await _dbContext.SaveChangesAsync();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Document", "uploaded", "")));
                }



            }
            return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
        }
        #endregion




        #region EventFileUpload in AWS
        public async Task<ExecutionResult> EventFileUpload(FileUploadRequestModel fileUpload)
        {
            if (fileUpload == null)
            {
                return new ExecutionResult();
            }
            var isDuplicate = _dbContext.TblEventUserFiles.Where(f => f.EventId == fileUpload.Id && f.FileType.ToLower() == fileUpload.DocumentType.ToLower()).FirstOrDefault();

            if (isDuplicate != null)
            {
                //TblEventUserFile file = new TblEventUserFile();
                using (MemoryStream stream = new MemoryStream())
                {
                    string fileName = "";
                    fileName = "Event/" + Path.GetFileName(fileUpload.Id + fileUpload.DocumentType + fileUpload.FileName.FileName);
                    if (!string.IsNullOrWhiteSpace(fileName))
                    {

                        fileUpload.FileName.CopyTo(stream);
                        var awsUpload = await _amazonRepo.UploadFile(stream, fileName);
                        if (awsUpload.Errors.Any())
                        {
                            return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
                        }
                        isDuplicate.FileName = fileUpload.FileName.FileName;
                        isDuplicate.FileType = fileUpload.DocumentType;
                        isDuplicate.Title = fileUpload.Title;
                        isDuplicate.EventId = fileUpload.Id;
                        isDuplicate.Downloadlink = string.Format(_awsSettings.BaseUrl, fileName);
                        isDuplicate.CreatedDate = DateTime.UtcNow;
                        _dbContext.TblEventUserFiles.Update(isDuplicate);
                        await _dbContext.SaveChangesAsync();
                        return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Document", "Updated", "")));
                    }



                }
            }
            else
            {
                TblEventUserFile file = new TblEventUserFile();
                using (MemoryStream stream = new MemoryStream())
                {
                    string fileName = "";
                    fileName = "Event/" + Path.GetFileName(fileUpload.Id + fileUpload.DocumentType + fileUpload.FileName.FileName);
                    if (!string.IsNullOrWhiteSpace(fileName))
                    {

                        fileUpload.FileName.CopyTo(stream);
                        var awsUpload = await _amazonRepo.UploadFile(stream, fileName);
                        if (awsUpload.Errors.Any())
                        {
                            return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
                        }
                        file.FileName = fileUpload.FileName.FileName;
                        file.FileType = fileUpload.DocumentType;
                        file.Title = fileUpload.Title;
                        file.EventId = fileUpload.Id;
                        file.Downloadlink = string.Format(_awsSettings.BaseUrl, fileName);
                        file.CreatedDate = DateTime.UtcNow;
                        await _dbContext.TblEventUserFiles.AddAsync(file);
                        await _dbContext.SaveChangesAsync();
                        return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Document", "uploaded", "")));
                    }



                }
            }
            return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
        }
        #endregion


        #region Multi FileUpload in AWS
        public async Task<ExecutionResult> MultiFileUpload(MultiFileUploadRequestModel fileUpload)
        {
            foreach (var IMGitem in fileUpload.FileName)
            {


                TblUserFile file = new TblUserFile();
                using (MemoryStream stream = new MemoryStream())
                {
                    string fileName = "";

                    fileUpload.DocumentType = "Image";

                    if (fileUpload.DocumentType.ToString().ToLower() == FileUploadModule.Image.ToString().ToLower())
                    {
                        fileName = "CMSImage/" + Path.GetFileName(IMGitem.FileName);
                    }


                    if (!string.IsNullOrWhiteSpace(fileName))
                    {

                        IMGitem.CopyTo(stream);
                        var awsUpload = await _amazonRepo.UploadFile(stream, fileName);
                        if (awsUpload.Errors.Any())
                        {
                            return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
                        }
                        file.FileName = IMGitem.FileName;
                        file.FileType = fileUpload.DocumentType;
                        file.Title = fileUpload.Title;
                        file.Id = fileUpload.Id;
                        file.Downloadlink = string.Format(_awsSettings.BaseUrl, fileName);
                        file.CreatedDate = DateTime.UtcNow;
                        await _dbContext.TblUserFiles.AddAsync(file);
                        await _dbContext.SaveChangesAsync();


                    }


                }
            }
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Document", "uploaded", "")));

        }



        /// <summary>
        /// Multiple Images can be save with this method 
        /// </summary>
        /// <param name="fileUpload">Upload file details</param>
        /// <returns>Images added to AWS and made database entry</returns>
        public async Task<ExecutionResult<PostMediaModel>> MultiFileUploadForCreatePost(CreatePostMultiFileUploadRequestModel fileUpload)
        {
            if (fileUpload.PostId > 0)
            {
                if (fileUpload.FileName != null)
                {

                    TblPostFile file = new TblPostFile();
                    using (MemoryStream stream = new MemoryStream())
                    {

                        string fileName = "Post/" + Path.GetFileName(DateTime.UtcNow.ToString("ddmmyyhhss") + "_" + fileUpload.FileName.FileName);

                        if (!string.IsNullOrWhiteSpace(fileName))
                        {

                            fileUpload.FileName.CopyTo(stream);
                            var awsUpload = await _amazonRepo.UploadFile(stream, fileName);
                            if (awsUpload.Errors.Any())
                            {
                                return new ExecutionResult<PostMediaModel>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
                            }

                            if (fileUpload.MediaType != null)
                            {
                                if (fileUpload.MediaType.ToLower() == "video")
                                {
                                    string VideoName = "Post/" + Path.GetFileName(DateTime.UtcNow.ToString("ddmmyyhhss") + "_" + fileUpload.Video.FileName);
                                    MemoryStream Videostream = new MemoryStream();
                                    fileUpload.Video.CopyTo(Videostream);
                                    var awsVideoUpload = await _amazonRepo.UploadFile(Videostream, VideoName);
                                    if (awsVideoUpload.Errors.Any())
                                    {
                                        return new ExecutionResult<PostMediaModel>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
                                    }
                                    file.Videolink = string.Format(_awsSettings.BaseUrl, VideoName);
                                }
                            }

                            file.PostId = fileUpload.PostId;
                            file.Downloadlink = string.Format(_awsSettings.BaseUrl, fileName);
                            if (fileUpload.MediaType != null)
                            {
                                file.MediaType = fileUpload.MediaType.ToLower();
                            }
                            else
                            {
                                file.MediaType = "";
                            }
                            await _dbContext.TblPostFiles.AddAsync(file);
                            await _dbContext.SaveChangesAsync();
                        }
                    }


                    return file != null ? new ExecutionResult<PostMediaModel>(_mapper.Map<PostMediaModel>(file)) :
               new ExecutionResult<PostMediaModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "User Post file")));

                }

                return new ExecutionResult<PostMediaModel>(new ErrorInfo(string.Format(MessageHelper.FilesMissing)));
            }

            return new ExecutionResult<PostMediaModel>(new ErrorInfo(string.Format(MessageHelper.PostIdMissing)));

        }


        public async Task<ExecutionResult<MemorieMediaModel>> MultiFileUploadForCreateMemorie(CreateMemorieMultiFileUploadRequestModel fileUpload)
        {
            if (fileUpload.MemorieId > 0)
            {
                if (fileUpload.FileName != null)
                {

                    TblMemorieFile file = new TblMemorieFile();
                    using (MemoryStream stream = new MemoryStream())
                    {

                        string fileName = "Memorie/" + Path.GetFileName(DateTime.UtcNow.ToString("ddmmyyhhss") + "_" + fileUpload.FileName.FileName);

                        if (!string.IsNullOrWhiteSpace(fileName))
                        {

                            fileUpload.FileName.CopyTo(stream);
                            var awsUpload = await _amazonRepo.UploadFile(stream, fileName);
                            if (awsUpload.Errors.Any())
                            {
                                return new ExecutionResult<MemorieMediaModel>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
                            }

                            if (fileUpload.MediaType != null)
                            {
                                if (fileUpload.MediaType.ToLower() == "video")
                                {
                                    string VideoName = "Memorie/" + Path.GetFileName(DateTime.UtcNow.ToString("ddmmyyhhss") + "_" + fileUpload.Video.FileName);
                                    MemoryStream Videostream = new MemoryStream();
                                    fileUpload.Video.CopyTo(Videostream);
                                    var awsVideoUpload = await _amazonRepo.UploadFile(Videostream, VideoName);
                                    if (awsVideoUpload.Errors.Any())
                                    {
                                        return new ExecutionResult<MemorieMediaModel>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
                                    }
                                    file.Videolink = string.Format(_awsSettings.BaseUrl, VideoName);
                                }
                            }

                            file.MemorieId = fileUpload.MemorieId;
                            file.Downloadlink = string.Format(_awsSettings.BaseUrl, fileName);
                            if (fileUpload.MediaType != null)
                            {
                                file.MediaType = fileUpload.MediaType.ToLower();
                            }
                            else
                            {
                                file.MediaType = "";
                            }
                            await _dbContext.TblMemorieFiles.AddAsync(file);
                            await _dbContext.SaveChangesAsync();


                        }
                    }


                    return file != null ? new ExecutionResult<MemorieMediaModel>(_mapper.Map<MemorieMediaModel>(file)) :
               new ExecutionResult<MemorieMediaModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "User Memorie file")));



                }

                return new ExecutionResult<MemorieMediaModel>(new ErrorInfo(string.Format(MessageHelper.FilesMissing)));
            }

            return new ExecutionResult<MemorieMediaModel>(new ErrorInfo(string.Format(MessageHelper.MemorieIdMissing)));

        }

        #endregion

        #region UpdateFileTitle
        public async Task<ExecutionResult> UpdateFileTitle(FileUploadResponseModel fileUpdate)
        {
            if (fileUpdate == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "model")));
            }
            bool isDuplicate = await _dbContext.TblUserFiles.AnyAsync(f => f.FileId != fileUpdate.FileId && f.Title == fileUpdate.Title);

            if (isDuplicate)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.Document.DuplicateTitle)));
            }
            var fileData = _dbContext.TblUserFiles.FirstOrDefault(x => x.FileId == fileUpdate.FileId);
            if (fileData == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "file data")));
            }
            fileData.Title = fileUpdate.Title;
            //fileData.UpdatedDate = DateTime.UtcNow;
            _dbContext.TblUserFiles.Update(fileData);
            await _dbContext.SaveChangesAsync();
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "File name", "updated", "")));
        }
        #endregion

        #region DeleteFile
        public async Task<ExecutionResult> DeleteFile(int FileId)
        {
            if (FileId <= 0 || FileId == null)
            {
                return new ExecutionResult();
            }
            var fileData = _dbContext.TblUserFiles.FirstOrDefault(x => x.FileId == FileId);
            if (fileData == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "file data")));
            }
            string fileName = "";

            if (fileData.FileType.ToLower() == FileUploadModule.Image.ToString().ToLower())
            {
                fileName = "CMSImage/" + Path.GetFileName(fileData.FileName);
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.PassParaMessage, "filename")));
            }
            var delete = await _amazonRepo.DeleteFile(fileName);
            if (delete.Errors.Count > 0)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
            }
            _dbContext.TblUserFiles.Remove(fileData);
            await _dbContext.SaveChangesAsync();
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "File", "deleted", "")));
        }

        /// <summary>
        /// Used to delete media file from Post
        /// </summary>
        /// <param name="removePostMedia">Details to remove post media</param>
        /// <returns>Successfully delete or not</returns>
        public async Task<ExecutionResult> RemovePostMediaFile(RemovePostMediaModel removePostMedia)
        {
            if (removePostMedia.PostId == 0)
            {
                return new ExecutionResult();
            }
            var fileData = _dbContext.TblPostFiles.Where(x => removePostMedia.PostFileId.Contains(x.PostFileId));
            if (fileData == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "file data")));
            }

            foreach (var item in fileData)
            {
                string fileName = "Post/" + Path.GetFileName(item.Downloadlink);
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    var delete = await _amazonRepo.DeleteFile(fileName);
                    if (delete.Errors.Count > 0)
                    {
                        return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SomethingWrong)));
                    }
                }
                _dbContext.TblPostFiles.Remove(item);
            }
            await _dbContext.SaveChangesAsync();
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "File", "deleted", "")));
        }

        #endregion


        #region DeleteAllFileIntroduction
        public async Task<ExecutionResult> DeleteAllFileIntroduction()
        {
            var fileData = _dbContext.TblUserFiles.Where(x => x.Id == 2);
            foreach (var item in fileData)
            {
                _dbContext.TblUserFiles.Remove(item);

            }
            await _dbContext.SaveChangesAsync();
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "File", "deleted", "")));
        }
        #endregion
    }
}
