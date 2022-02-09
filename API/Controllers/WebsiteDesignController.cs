using Totem.Business.Core.DataTransferModels.WebsiteDesign;
using Totem.Business.Helpers;
using Totem.Business.RepositoryInterface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Documents;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Totem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteDesignController : BaseController
    {
        #region Constructor
        private readonly TokenManager _tokenManger;
        private readonly IWebsiteRepository _accountRepo;
        private readonly IDocumentRepository _documentRepo;
        public WebsiteDesignController(TokenManager tokenManager, IWebsiteRepository accountRepo, IDocumentRepository DocRepository)
        {
            _accountRepo = accountRepo;
            _tokenManger = tokenManager;
            _documentRepo = DocRepository;
        }
        #endregion

        [HttpPost("Create Web Content")]
        public async Task<IActionResult> CreateWebContent(WebsiteConten websiteConten)
        {

            return FromExecutionResult(await _accountRepo.CreateWebContent(websiteConten));
        }


        [HttpPost("EarlyToParty")]
        public async Task<IActionResult> EarlyToParty(EarlyToPartyModel EarlyToPartyModel)
        {
            //TblUpdatEmail TblUpdatEmail = new TblUpdatEmail();
            //TblUpdatEmail.Email = Email;
            return FromExecutionResult(await _accountRepo.EarlyToParty(EarlyToPartyModel));
        }


        [HttpPost("ReceiveEmailUpdatesSave")]
        public async Task<IActionResult> ReceiveEmailUpdatesSave(TblUpdatEmail TblUpdatEmail)
        {
            //TblUpdatEmail TblUpdatEmail = new TblUpdatEmail();
            //TblUpdatEmail.Email = Email;
            return FromExecutionResult(await _accountRepo.ReceiveEmailUpdatesSave(TblUpdatEmail));
        }

        [HttpPost("Update Web Content")]
        public async Task<IActionResult> Updateuser(WebsiteContenID WebsiteContenID)
        {
            return FromExecutionResult(await _accountRepo.UpdateWebContent(WebsiteContenID));
        }


   
        [HttpGet("GetAll Web Content")]
        public async Task<IActionResult> GetAllUsers()
        {
            return FromExecutionResult(await _accountRepo.GetAll());
        }

        [HttpPost("Section")]
        public async Task<IActionResult> Getsection()
        {
            return FromExecutionResult(await _accountRepo.Getsection());
        }

        [HttpPost("Section1")]
        public async Task<IActionResult> GetSession1()
        {
            return FromExecutionResult(await _accountRepo.GetSession1());
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> FileUpload([FromForm] FileUploadRequestModel fileUpload)
        {
            return FromExecutionResult(await _documentRepo.FileUpload(fileUpload));
        }


        [HttpPost("MultiFileUpload")]
        public async Task<IActionResult> MultiFileUpload([FromForm] MultiFileUploadRequestModel fileUpload)
        {
             var files = Request.Form.Files;
            List<IFormFile> formfiles = new List<IFormFile>();
            foreach (var file in files)
            {

                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);                       
                        formfiles.Add(file);
                     
                    }
                }
                   
            }
            fileUpload.FileName = formfiles;
            return FromExecutionResult(await _documentRepo.MultiFileUpload(fileUpload));
        }

        /// <summary>
        /// GetFileDetailsByFileId
        /// </summary>
        /// <param name="FileId"></param>
        /// <returns></returns>
        [HttpGet("GetFileDetailsByFileId/{FileId}")]
        public IActionResult GetFileDetailsByFileId(int FileId)
        {
            return FromExecutionResult(_documentRepo.GetFileDetailsByFileId(FileId));
        }

        /// <summary>
        /// UpdateFileTitle
        /// </summary>
        /// <param name="fileUpdate"></param>
        /// <returns></returns>
        [HttpPost("UpdateFileTitle")]
        public async Task<IActionResult> UpdateFileTitle(FileUploadResponseModel fileUpdate)
        {
            return FromExecutionResult(await _documentRepo.UpdateFileTitle(fileUpdate));
        }

        /// <summary>
        /// DeleteFile
        /// </summary>
        /// <param name="FileId"></param>
        /// <returns></returns>
        [HttpGet("DeleteFile/{FileId}")]
        public async Task<IActionResult> DeleteFile(int FileId)
        {
            return FromExecutionResult(await _documentRepo.DeleteFile(FileId));
        }

        [HttpGet("DeleteAllFileIntroduction")]
        public async Task<IActionResult> DeleteAllFileIntroduction()
        {
            return FromExecutionResult(await _documentRepo.DeleteAllFileIntroduction());
        }

        /// <summary>
        /// FileDetailListByAccountId
        /// </summary>
        /// <returns></returns>
        [HttpPost("FileDetailListBysectionId")]
        public IActionResult FileDetailListByAccountId(DocumentListRequestModel listModel)
        {
            return FromExecutionResult(_documentRepo.FileDetailListBySectionId(listModel));
        }

        /// <summary>
        /// FileDetailListByAccountId
        /// </summary>
        /// <returns></returns>
        //[HttpPost("GetIntroductionImages")]
        [HttpGet("GetIntroductionImages")]
        public IActionResult GetIntroductionImages()
        {
            DocumentListRequestModel listModel = new DocumentListRequestModel();
            listModel.Id = 2;
            listModel.FileType = "Image";
            return FromExecutionResult(_documentRepo.FileDetailListBySectionId(listModel));
        }

        /// <summary>
        /// FileDetailListByAccountId
        /// </summary>
        /// <returns></returns>
        //[HttpPost("InsertToGetUpdates")]
        //public IActionResult InsertToGetUpdates(string Email)
        //{
           
        //}

    }
}
