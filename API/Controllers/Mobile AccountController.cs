using Totem.Business.Core.DataTransferModels.Account;
using Totem.Business.Helpers;
using Totem.Business.RepositoryInterface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels;
using System.Text.Json;
using System.Collections.Generic;
using Newtonsoft.Json;
using Totem.Business.Core.DataTransferModels.Documents;
using Totem.Business.Core.DataTransferModels.Spotify;
using Microsoft.AspNetCore.Authorization;

namespace Totem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Mobile_AccountController : BaseController
    {
        #region Constructor
        private readonly TokenManager _tokenManger;
        private readonly IAccountRepository _accountRepo;
        private readonly IDocumentRepository _documentRepo;
        public Mobile_AccountController(TokenManager tokenManager, IAccountRepository accountRepo, IDocumentRepository DocRepository)
        {
            _accountRepo = accountRepo;
            _tokenManger = tokenManager;
            _documentRepo = DocRepository;
        }
        #endregion



        /// <summary>
        /// CheckMailExist
        /// </summary>
        /// <param name="CheckMailExist"></param>
        /// <returns></returns>
        [HttpPost("CheckMailExist")]
        public async Task<IActionResult> CheckMailExist(checkmail checkmail)
        {


            ExecutionResult RequestModel = await _accountRepo.CheckMailExist(checkmail);
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
            return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });


        }

        /// <summary>
        /// GetFCMDetails
        /// </summary>
        /// <param name="GetFcmModel"></param>
        /// <returns></returns>
        [HttpPost("GetFCMDetails")]
        public async Task<IActionResult> GetFCMDetails(GetFcmModel GetFcmModel)
        {


            ExecutionResult<List<UserFcmModel>> RequestModel = await _accountRepo.GetFCMDetails(GetFcmModel);
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
            return new JsonResult(new { Result = RequestModel.Value, meta = new { status = RequestModel.Success, message = ResponceMessage } });


        }

        /// <summary>
        /// LogoutFCM
        /// </summary>
        /// <param name="LogoutFCM"></param>
        /// <returns></returns>
        [HttpPost("LogoutFCM")]
        public async Task<IActionResult> LogoutFCM(UserFcmModel UserFcmModel)
        {


            ExecutionResult RequestModel = await _accountRepo.LogoutFCM(UserFcmModel);
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

            return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });


        }

        /// <summary>
        /// CreateAccount
        /// </summary>
        /// <param name="createAccountModel"></param>
        /// <returns></returns>
        [HttpPost("CreateAccount")]
        public async Task<IActionResult> Signup(UsermanagementDetails createAccountModel)
        {
            InsertUsermanagement InsertUser = new InsertUsermanagement();
            InsertUser.Firstname = createAccountModel.Firstname;
            InsertUser.Lastname = createAccountModel.Lastname;
            InsertUser.Username = createAccountModel.Username;
            InsertUser.Email = createAccountModel.Email;
            InsertUser.Phone = createAccountModel.Phone;
            InsertUser.BirthDate = createAccountModel.BirthDate;
            InsertUser.Password = createAccountModel.Password;
            InsertUser.SignInType = createAccountModel.SignInType;
            InsertUser.ReferredBy = createAccountModel.ReferredBy;
            InsertUser.IsMobileVerified = true;
            InsertUser.Role = 0;
            InsertUser.CreatedBy = "";
            InsertUser.Address = "";
            InsertUser.Longitude = 0;
            InsertUser.Latitude = 0;
            InsertUser.Bio = "";
            InsertUser.Image = "";
            InsertUser.MobileOtp = 1;
            ExecutionResult RequestModel = await _accountRepo.Signup(InsertUser);



            string ResponceMessage = "";
            if (RequestModel.Success)
            {
                Login login = new Login();
                login.EmailId = InsertUser.Email;
                login.Password = InsertUser.Password;
                login.Fcm = createAccountModel.Fcm;
                login.SignInType = InsertUser.SignInType;
                ExecutionResult<UsermanagementDetailsID> LoginResponseModel = _accountRepo.Login(login);
                foreach (InfoMessage lst in RequestModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
                return new JsonResult(new { Result = LoginResponseModel.Value, meta = new { status = RequestModel.Success, message = ResponceMessage } });
            }
            else
            {
                foreach (ErrorInfo lst in RequestModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
                return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });
            }


            // return new JsonResult(new { Status = RequestModel.Success, Data = RequestModel.Errors, Message = "" });


            // return FromExecutionResult(await _accountRepo.CreateAccount(createAccountModel));          
        }


        /// <summary>
        /// Config Notefication
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="AddUserNotification"></param>
        /// <returns></returns>
        [HttpPost("ConfigNotefication")]
        //[Authorize]
        public async Task<IActionResult> ConfigNotefication(AddUserNotification AddUserNotification)
        {

            ExecutionResult<GetAllUserStatuswithNotification> RequestModel = await _accountRepo.ConfigNotefication(AddUserNotification);
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
            return new JsonResult(new { Result = RequestModel.Value, meta = new { status = RequestModel.Success, message = ResponceMessage } });

        }

        /// <summary>
        /// Invalid Login Attempts Block
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="AddUserNotification"></param>
        /// <returns></returns>
        [HttpPost("InvalidLoginAttempts")]
        //[Authorize]
        public async Task<IActionResult> InvalidLoginAttempts(EmailModel EmailId)
        {

            ExecutionResult RequestModel = await _accountRepo.InvalidLoginAttempts(EmailId);
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
            return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });

        }


        /// <summary>
        /// EditPrivacy
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="IsPrivate"></param>
        /// <returns></returns>
        [HttpPost("EditPrivacy")]
        public async Task<IActionResult> EditPrivacy(EditprivacyModel EditprivacyModel)
        {

            ExecutionResult RequestModel = await _accountRepo.editprivacy(EditprivacyModel);



            string ResponceMessage = "";
            if (RequestModel.Success)
            {


                foreach (InfoMessage lst in RequestModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
                return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });
            }
            else
            {
                foreach (ErrorInfo lst in RequestModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
                return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });
            }



        }


        /// <summary>
        /// Block User By User
        /// </summary>
        /// <param name="BlockUserModel"></param>
        /// <returns></returns>
        [HttpPost("BlockUser")]
        public async Task<IActionResult> BlockUser(BlockUserModel BlockUserModel)
        {

            ExecutionResult RequestModel = await _accountRepo.BlockUser(BlockUserModel);
            string ResponceMessage = "";
            if (RequestModel.Success)
            {

                foreach (InfoMessage lst in RequestModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
                return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });
            }
            else
            {
                foreach (ErrorInfo lst in RequestModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
                return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });
            }

        }


        /// <summary>
        /// Users Present Live Status
        /// </summary>
        /// <param name="PresentLiveStatus"></param>
        /// <returns></returns>
        [HttpPost("PresentLiveStatus")]
        public async Task<IActionResult> PresentLiveStatus(PresentLiveStatusModel PresentLiveStatusModel)
        {

            ExecutionResult RequestModel = await _accountRepo.PresentLiveStatus(PresentLiveStatusModel);
            string ResponceMessage = "";
            if (RequestModel.Success)
            {

                foreach (InfoMessage lst in RequestModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
                return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });
            }
            else
            {
                foreach (ErrorInfo lst in RequestModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
                return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });
            }

        }

        // [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

        [HttpPost("GetBlockuser")]
        public async Task<IActionResult> GetBlockuser(FollowModel FollowModel)
        {
            //return FromExecutionResult(await _accountRepo.GetAllfollow(ID));
            ExecutionResult<PaginatedResponse<FollowUsers>> RequestModel = await _accountRepo.GetBlockuser(FollowModel);
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
            //return FromExecutionResult(await _accountRepo.GetfollowerCount(ID));
            return new JsonResult(new { Result = RequestModel.Value, meta = new { status = RequestModel.Success, message = ResponceMessage } });
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
            //return FromExecutionResult(_accountRepo.Login(EmailId, Password));
            ExecutionResult<UsermanagementDetailsID> LoginResponseModel = _accountRepo.Login(login);

            string ResponceMessage = "";
            if (LoginResponseModel.Success)
            {

                foreach (InfoMessage lst in LoginResponseModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
            }
            else
            {
                foreach (ErrorInfo lst in LoginResponseModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
            }
            if (LoginResponseModel.Success)
            {
                return new JsonResult(new { Result = LoginResponseModel.Value, meta = new { status = LoginResponseModel.Success, message = ResponceMessage } });
            }
            else
                return new JsonResult(new { Result = "", meta = new { status = LoginResponseModel.Success, message = ResponceMessage } });

            //return new JsonResult(new { Status = LoginResponseModel.Success, Message = LoginResponseModel.Messages, Data = LoginResponseModel.Value });
        }

        /// <summary>
        /// GetUpdatedVersion
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Version"></param>
        /// <returns></returns>
        [HttpPost("GetUpdatedVersion")]
        public IActionResult GetUpdatedVersion(GetVersionmodel GetVersionmodel)
        {
            //return FromExecutionResult(_accountRepo.Login(EmailId, Password));
            Versionmodel versionmodel = new Versionmodel();
            versionmodel.Ismandatory = true;
            versionmodel.UpdateRequired = true;
            ExecutionResult<Versionmodel> LoginResponseModel = _accountRepo.GetUpdatedVersion(GetVersionmodel, versionmodel);

            string ResponceMessage = "";
            if (LoginResponseModel.Success)
            {

                foreach (InfoMessage lst in LoginResponseModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
            }
            else
            {
                foreach (ErrorInfo lst in LoginResponseModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
            }
            if (LoginResponseModel.Success)
            {
                return new JsonResult(new { Result = LoginResponseModel.Value, meta = new { status = LoginResponseModel.Success, message = ResponceMessage } });
            }
            else
                return new JsonResult(new { Result = "", meta = new { status = LoginResponseModel.Success, message = ResponceMessage } });

            //return new JsonResult(new { Status = LoginResponseModel.Success, Message = LoginResponseModel.Messages, Data = LoginResponseModel.Value });
        }

        /// <summary>
        /// ForgetPassword
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(checkmail EmailId)
        {

            // return FromExecutionResult(_accountRepo.ForgetPassword(EmailId).Result);
            ExecutionResult RequestModel = _accountRepo.ForgetPassword(EmailId.EmailId).Result;
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

            //return new JsonResult(new { Status = RequestModel.Success, Message = RequestModel.Messages, Data = "" });

            return new JsonResult(new { Result = "", meta = new { Status = RequestModel.Success, message = ResponceMessage } });
        }

        /// <summary>
        /// ForgetPassword
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPassword resetPassword)
        {

            //return FromExecutionResult(_accountRepo.ResetPassword(EmailToken, Password).Result);
            ExecutionResult RequestModel = _accountRepo.ResetPassword(resetPassword.EmailToken, resetPassword.Password).Result;
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

            // return new JsonResult(new { Status = RequestModel.Success, Message = RequestModel.Messages, Data = "" });
        }

        [HttpGet("Activate")]
        public IActionResult Activate(checkEmailToken EmailToken)
        {
            //return FromExecutionResult(_accountRepo.EmailConfirmation(EmailToken).Result);
            ExecutionResult RequestModel = _accountRepo.EmailConfirmation(EmailToken.EmailToken).Result;
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

            // return new JsonResult(new { Status = RequestModel.Success, Message = RequestModel.Messages, Data = "" });
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(ChangePassword ChangePassword)
        {
            //return FromExecutionResult(_accountRepo.ChangePassword(accountId, OldPassword, Password));
            ExecutionResult RequestModel = _accountRepo.ChangePassword(ChangePassword.accountId, ChangePassword.OldPassword, ChangePassword.Password);
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

            //return new JsonResult(new { Status = RequestModel.Success, Message = RequestModel.Messages, Data = "" });
        }


        [HttpPost("ProfileImageUpload")]
        public async Task<IActionResult> ProfileImageUpload([FromForm] ProfileFileUploadRequestModel fileUpload)
        {
            //return FromExecutionResult(await _documentRepo.ProfileFileUpload(fileUpload));
            ExecutionResult<UsermanagementDetailsID> RequestModel = await _documentRepo.ProfileFileUpload(fileUpload);
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
            return new JsonResult(new { Result = RequestModel.Value, meta = new { Status = RequestModel.Success, message = ResponceMessage } });
        }

        [HttpPost("GetAllUserStatus")]
        public async Task<IActionResult> GetAllUserStatus(GetSpotify GetSpotify)
        {
            //return FromExecutionResult(await _accountRepo.GetAllUserStatus(id));
            //return FromExecutionResult(await _documentRepo.ProfileFileUpload(fileUpload));
            ExecutionResult<GetAllUserStatuswithNotification> RequestModel = await _accountRepo.GetAllUserStatus(GetSpotify.Id);
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
            return new JsonResult(new { Result = RequestModel.Value, meta = new { Status = RequestModel.Success, message = ResponceMessage } });

        }


        [HttpPost("GetNotification")]
        public async Task<IActionResult> GetNotification(FollowModel FollowModel)
        {
            //return FromExecutionResult(await _accountRepo.GetAllUserStatus(id));
            //return FromExecutionResult(await _documentRepo.ProfileFileUpload(fileUpload));
            ExecutionResult<PaginatedResponse<NotificationModel>> RequestModel = await _accountRepo.GetNotification(FollowModel);
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
            return new JsonResult(new { Result = RequestModel.Value, meta = new { Status = RequestModel.Success, message = ResponceMessage } });

        }


        [HttpPost("ClearNotification")]
        public async Task<IActionResult> ClearNotification(UserIdModel UserIdModel)
        {
            //return FromExecutionResult(await _accountRepo.GetAllUserStatus(id));
            //return FromExecutionResult(await _documentRepo.ProfileFileUpload(fileUpload));
            ExecutionResult RequestModel = await _accountRepo.ClearNotification(UserIdModel);
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



        [HttpPost("ResendMailverify")]
        public async Task<IActionResult> ResendMailverify(GetSpotify GetSpotify)
        {
            ExecutionResult RequestModel = await _accountRepo.ResendMailverify(GetSpotify);
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




        [HttpPost("Deleteuser")]
        public async Task<IActionResult> Deleteuser(Login Login)
        {
            // return FromExecutionResult(await _accountRepo.Updateuser(userprofile));
            ExecutionResult RequestModel = await _accountRepo.Deleteuser(Login);
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

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> Updateuser(UserProfile userprofile)
        {
            // return FromExecutionResult(await _accountRepo.Updateuser(userprofile));
            ExecutionResult RequestModel = await _accountRepo.Updateuser(userprofile);
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


        [HttpPost("Spotify")]
        public async Task<IActionResult> Spotify(SpotifyModel SpotifyModel)
        {

            ExecutionResult LoginResponseModel = await _accountRepo.Spotify(SpotifyModel);

            string ResponceMessage = "";
            if (LoginResponseModel.Success)
            {

                foreach (InfoMessage lst in LoginResponseModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
            }
            else
            {
                foreach (ErrorInfo lst in LoginResponseModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
            }

            return new JsonResult(new { Result = "", meta = new { status = LoginResponseModel.Success, message = ResponceMessage } });

        }



        [HttpPost("GetSpotify")]
        public async Task<IActionResult> GetSpotify(GetSpotify GetSpotify)
        {

            ExecutionResult<GetSpotifyModel> ResponseModel = await _accountRepo.GetSpotify(GetSpotify);

            string ResponceMessage = "";
            if (ResponseModel.Success)
            {

                foreach (InfoMessage lst in ResponseModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
            }
            else
            {
                foreach (ErrorInfo lst in ResponseModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
            }

            return new JsonResult(new { Result = ResponseModel.Value, meta = new { status = ResponseModel.Success, message = ResponceMessage } });

        }


        [HttpPost("GetTopSpotify")]
        public async Task<IActionResult> GetTopSpotify()
        {

            ExecutionResult<GetSpotifyModel> ResponseModel = await _accountRepo.GetTopSpotify();

            string ResponceMessage = "";
            if (ResponseModel.Success)
            {

                foreach (InfoMessage lst in ResponseModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
            }
            else
            {
                foreach (ErrorInfo lst in ResponseModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
            }

            return new JsonResult(new { Result = ResponseModel.Value, meta = new { status = ResponseModel.Success, message = ResponceMessage } });

        }

        [HttpPost("FavSongs")]
        public async Task<IActionResult> FavSongs(Spotifysongs Spotifysongs)
        {

            ExecutionResult LoginResponseModel = await _accountRepo.FavSongs(Spotifysongs);

            string ResponceMessage = "";
            if (LoginResponseModel.Success)
            {

                foreach (InfoMessage lst in LoginResponseModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
            }
            else
            {
                foreach (ErrorInfo lst in LoginResponseModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
            }

            return new JsonResult(new { Result = "", meta = new { status = LoginResponseModel.Success, message = ResponceMessage } });

        }

        [HttpPost("GetTopSongs")]
        public async Task<IActionResult> GetTopSongs()
        {

            ExecutionResult<List<Songs>> ResponseModel = await _accountRepo.GetTopSongs();

            string ResponceMessage = "";
            if (ResponseModel.Success)
            {

                foreach (InfoMessage lst in ResponseModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
            }
            else
            {
                foreach (ErrorInfo lst in ResponseModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
            }

            return new JsonResult(new { Result = ResponseModel.Value, meta = new { status = ResponseModel.Success, message = ResponceMessage } });

        }

        [HttpPost("GetFavSongs")]
        public async Task<IActionResult> GetFavSongs(GetSpotify GetSpotify)
        {

            ExecutionResult<List<Songs>> ResponseModel = await _accountRepo.GetFavSongs(GetSpotify);

            string ResponceMessage = "";
            if (ResponseModel.Success)
            {

                foreach (InfoMessage lst in ResponseModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
            }
            else
            {
                foreach (ErrorInfo lst in ResponseModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
            }

            return new JsonResult(new { Result = ResponseModel.Value, meta = new { status = ResponseModel.Success, message = ResponceMessage } });

        }





        [HttpPost("BusinessUserReq")]
        public async Task<IActionResult> BusinessUserReq(BusinessModel BusinessModel)
        {

            ExecutionResult LoginResponseModel = await _accountRepo.BusinessUserReq(BusinessModel);

            string ResponceMessage = "";
            if (LoginResponseModel.Success)
            {

                foreach (InfoMessage lst in LoginResponseModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
            }
            else
            {
                foreach (ErrorInfo lst in LoginResponseModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
            }

            return new JsonResult(new { Result = "", meta = new { status = LoginResponseModel.Success, message = ResponceMessage } });

        }


        [HttpPost("ProfileVerifyReq")]
        public async Task<IActionResult> ProfileVerifyReq(VerifyProfile VerifyProfile)
        {

            ExecutionResult LoginResponseModel = await _accountRepo.ProfileVerifyReq(VerifyProfile);

            string ResponceMessage = "";
            if (LoginResponseModel.Success)
            {
                foreach (InfoMessage lst in LoginResponseModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
            }
            else
            {
                foreach (ErrorInfo lst in LoginResponseModel.Errors)
                {
                    ResponceMessage = lst.ErrorMessage;
                }
            }

            return new JsonResult(new { Result = "", meta = new { status = LoginResponseModel.Success, message = ResponceMessage } });

        }


    }
}
