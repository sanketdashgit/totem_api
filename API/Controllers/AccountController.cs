using Totem.Business.Core.DataTransferModels.Account;
using Totem.Business.Helpers;
using Totem.Business.RepositoryInterface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Documents;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Totem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        #region Constructor
        private readonly TokenManager _tokenManger;
        private readonly IAccountRepository _accountRepo;
        private readonly IDocumentRepository _documentRepo;
        public AccountController(TokenManager tokenManager, IAccountRepository accountRepo, IDocumentRepository DocRepository)
        {
            _accountRepo = accountRepo;
            _tokenManger = tokenManager;
            _documentRepo = DocRepository;
        }
        #endregion

        /// <summary>
        /// CreateAccount
        /// </summary>
        /// <param name="createAccountModel"></param>
        /// <returns></returns>
        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount(CreateAccountRequestModel createAccountModel)
        {
            return FromExecutionResult(await _accountRepo.CreateAccount(createAccountModel));
        }

        [HttpGet("GetDastbord")]
        public async Task<IActionResult> GetDastbord()
        {
            return FromExecutionResult(await _accountRepo.GetDastbord());
        }


        [HttpPost("UpdateUser")]
        public async Task<IActionResult> Updateuser(UserProfile userprofile)
        {
            return FromExecutionResult(await _accountRepo.Updateuser(userprofile));
        }

        [HttpPost("AdminUserUpdate")]
        public async Task<IActionResult> AdminUserUpdate(AdminUserProfile userprofile)
        {
            return FromExecutionResult(await _accountRepo.AdminUserUpdate(userprofile));
        }


        [HttpGet("UpdateBussnessUser")]
        public async Task<IActionResult> UpdateBussnessUser(long Id, bool status)
        {
            return FromExecutionResult(await _accountRepo.UpdateBussnessUser(Id, status));
        }


        [HttpGet("UpdateProfileVerified")]
        public async Task<IActionResult> UpdateProfileVerified(long Id, bool status)
        {
            return FromExecutionResult(await _accountRepo.UpdateProfileVerified(Id, status));
        }

        [HttpGet("EventActive")]
        public async Task<IActionResult> EventActive(long EventId, bool status)
        {
            return FromExecutionResult(await _accountRepo.EventActive(EventId, status));
        }


        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return FromExecutionResult(await _accountRepo.GetAllUsers());
        }

        [HttpGet("GetAllReferedUsers")]
        public async Task<IActionResult> GetAllReferedUsers()
        {
            return FromExecutionResult(await _accountRepo.GetAllReferedUsers());
        }

        [HttpGet("GetAllReferedUsersByuserId")]
        public async Task<IActionResult> GetAllReferedUsersByuserId(long Id)
        {
            return FromExecutionResult(await _accountRepo.GetAllReferedUsersByuserId(Id));
        }

        [HttpGet("GetAllUserStatus")]
        public async Task<IActionResult> GetAllUserStatus(int id)
        {
            return FromExecutionResult(await _accountRepo.GetAllUserStatus(id));
        }

        [HttpGet("GetAllsupport")]
        public async Task<IActionResult> GetAllsupport()
        {
            return FromExecutionResult(await _accountRepo.GetAllsupport());
        }

        //GetProfileVerifyReq
        [HttpGet("GetProfileVerifyReq")]
        public async Task<IActionResult> GetProfileVerifyReq(long Id)
        {
            return FromExecutionResult(await _accountRepo.GetProfileVerifyReq(Id));
        }

       


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="EmailId"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login(Login login)
        {
            return FromExecutionResult(_accountRepo.AdminLogin(login));
            //ExecutionResult<LoginResponseModel> LoginResponseModel = _accountRepo.Login(login.EmailId, login.Password);
            //return new JsonResult(new { Status = LoginResponseModel.Success, Message = LoginResponseModel.Messages, Value = LoginResponseModel.Value });

        }

        /// <summary>
        /// ForgetPassword
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        [HttpGet("ForgetPassword/{EmailId}")]
        public IActionResult ForgetPassword(string EmailId)
        {
            return FromExecutionResult(_accountRepo.ForgetPassword(EmailId).Result);
        }


        /// <summary>
        /// ForgetPassword
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPassword resetPassword)
        {
            return FromExecutionResult(_accountRepo.ResetPassword(resetPassword.EmailToken, resetPassword.Password).Result);
        }

        [HttpGet("Activate")]
        public IActionResult Activate(string EmailToken)
        {

            return FromExecutionResult(_accountRepo.EmailConfirmation(EmailToken).Result);
        }

        

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(ChangePassword changePassword)
        {
            return FromExecutionResult(_accountRepo.ChangePassword(changePassword.accountId, changePassword.OldPassword, changePassword.Password));
        }


        /// <summary>
        /// InsertFAQ
        /// </summary>
        /// <param name="FAQ"></param>
        /// <returns></returns>
        [HttpPost("InsertFAQ")]
        public async Task<IActionResult> InsertFAQ(FAQ FAQ)
        {
            return FromExecutionResult(await _accountRepo.InsertFAQ(FAQ));

        }



        [HttpPost("FAQUpdate")]
        public async Task<IActionResult> FAQUpdate(FAQDetails FAQDetails)
        {
            return FromExecutionResult(await _accountRepo.FAQUpdate(FAQDetails));
        }


        [HttpGet("FAQDelete")]
        public async Task<IActionResult> FAQDelete(int Id)
        {
            return FromExecutionResult(await _accountRepo.FAQDelete(Id));
        }



        [HttpGet("GetAllFAQ")]
        public async Task<IActionResult> GetAllFAQ()
        {
            return FromExecutionResult(await _accountRepo.GetAllFAQ());
        }



        [HttpPost("ContactUs")]
        public async Task<IActionResult> ContactUs(ContactUs contactUs)
        {
            return FromExecutionResult(await _accountRepo.Contactus(contactUs));
        }

        [HttpGet("GetAllContactUs")]
        public async Task<IActionResult> GetAllContactUs()
        {
            return FromExecutionResult(await _accountRepo.GetAllContactUs());
        }

        [HttpPost("AdminProfileImageUpload")]
        public async Task<IActionResult> ProfileImageUpload([FromForm] ProfileFileUploadRequestModel fileUpload)
        {
            var files = Request.Form.Files;

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileUpload.FileName = file;
                        fileUpload.Id = 1;
                    }
                }
            }
            //return FromExecutionResult(await _documentRepo.ProfileFileUpload(fileUpload));
            ExecutionResult RequestModel = await _documentRepo.ProfileFileUpload(fileUpload);
            string ResponceMessage = "";
            if (RequestModel.Success)
            {

                foreach (InfoMessage lst in RequestModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
            }
            else
            {
                foreach (ErrorInfo lst in RequestModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
            }
            return new JsonResult(new { Result = "", meta = new { Status = RequestModel.Success, message = ResponceMessage } });
        }


        [HttpGet("AdminGetUserPosts")]
        public async Task<IActionResult> AdminGetUserPosts()
        {

            return FromExecutionResult(await _accountRepo.AdminGetUserPosts());

        }

        [HttpGet("AdminGetPostByID/{PostId}")]
        public async Task<IActionResult> AdminGetPostByID(long PostId)
        {

            return FromExecutionResult(await _accountRepo.AdminGetPostByID(PostId));

        }
    }
}
