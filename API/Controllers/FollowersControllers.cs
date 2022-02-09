using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Account;
using Totem.Business.Helpers;
using Totem.Business.RepositoryInterface;

namespace Totem.API.Controllers
{
    public class FollowersControllers : BaseController
    {
        #region Constructor
        private readonly TokenManager _tokenManger;
        private readonly IFollowersRepository _accountRepo;
        public FollowersControllers(TokenManager tokenManager, IFollowersRepository accountRepo)
        {
            _accountRepo = accountRepo;
            _tokenManger = tokenManager;
        }
        #endregion


        /// <summary>
        /// Insert Follow
        /// </summary>
        /// <param name="createAccountModel"></param>
        /// <returns></returns>
        [HttpPost("Follow")]
        public async Task<IActionResult> Follow(Follow Follow)
        {
            SendNotification sendNotification = new SendNotification();
            sendNotification.notification = new notification();
            sendNotification.data = new data();
            sendNotification.registration_ids = new List<string>();
            ExecutionResult RequestModel = await _accountRepo.Follow(Follow, sendNotification);



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
        /// Insert Delete Suggusted
        /// </summary>
        /// <param name="Deletesuggested"></param>
        /// <returns></returns>
        [HttpPost("Deletesuggested")]
        public async Task<IActionResult> Deletesuggested(DeleteSuggested DeleteSuggested)
        {


            ExecutionResult RequestModel = await _accountRepo.Deletesuggested(DeleteSuggested);



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

        [HttpPost("GetAllDeletesuggested")]
        public async Task<IActionResult> GetAllDeletesuggested(FollowModel FollowModel)
        {
            //return FromExecutionResult(await _accountRepo.GetAllfollow(ID));
            ExecutionResult<PaginatedResponse<FollowUsers>> RequestModel = await _accountRepo.GetAllfollow(FollowModel);
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

        [HttpPost("ApproveFollow")]
        public async Task<IActionResult> ApproveFollow(Follow Follow)
        {


            ExecutionResult RequestModel = await _accountRepo.ApproveFollow(Follow);



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

        [HttpPost("GetAllUsers")]
        public IActionResult GetAllUsers(FollowModel listModel)
        {
            // return FromExecutionResult(_accountRepo.GetAllUsers(listModel));
            // return FromExecutionResult(await _accountRepo.GetAllUsers());
            ExecutionResult<PaginatedResponse<FollowUsers>> RequestModel = _accountRepo.GetAllUsers(listModel);
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


        [HttpPost("GetAllExploreUsers")]
        public IActionResult GetAllExploreUsers(FollowModel listModel)
        {
            // return FromExecutionResult(_accountRepo.GetAllUsers(listModel));
            // return FromExecutionResult(await _accountRepo.GetAllUsers());
            ExecutionResult<PaginatedResponse<FollowUsers>> RequestModel = _accountRepo.GetAllExploreUsers(listModel);
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

        [HttpPost("UpdatedGetAllUsers")]
        public IActionResult UpdatedGetAllUsers(GetFollowModel listModel)
        {
            // return FromExecutionResult(_accountRepo.GetAllUsers(listModel));
            // return FromExecutionResult(await _accountRepo.GetAllUsers());
            ExecutionResult<PaginatedResponse<FollowUsers>> RequestModel = _accountRepo.UpdatedGetAllUsers(listModel);
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

        [HttpPost("GetfollowerRequest")]
        public async Task<IActionResult> GetfollowerRequest(FollowModel FollowModel)
        {
            // return FromExecutionResult(_accountRepo.GetAllUsers(listModel));
            // return FromExecutionResult(await _accountRepo.GetAllUsers());
            ExecutionResult<PaginatedResponse<FollowUsers>> RequestModel = await _accountRepo.GetfollowerRequest(FollowModel);
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


        [HttpPost("GetAllfollow")]
        public async Task<IActionResult> GetAllfollow(FollowModel FollowModel)
        {
            //return FromExecutionResult(await _accountRepo.GetAllfollow(ID));
            ExecutionResult<PaginatedResponse<FollowUsers>> RequestModel = await _accountRepo.GetAllfollow(FollowModel);
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


        [HttpPost("TagAllfollow")]
        public async Task<IActionResult> TagAllfollow(FollowModel FollowModel)
        {
            //return FromExecutionResult(await _accountRepo.GetAllfollow(ID));
            ExecutionResult<PaginatedResponse<FollowUsers>> RequestModel = await _accountRepo.TagAllfollow(FollowModel);
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

        [HttpPost("GetAllfollower")]
        public async Task<IActionResult> GetAllfollower(FollowModel FollowModel)
        {
            //return FromExecutionResult(await _accountRepo.GetAllfollower(ID));
            ExecutionResult<PaginatedResponse<FollowUsers>> RequestModel = await _accountRepo.GetAllfollower(FollowModel);
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




        [HttpPost("GetfollowerCount")]
        public async Task<IActionResult> GetfollowerCount(FollowersModel FollowersModel)
        {
            ExecutionResult<FollowersCountModel> RequestModel = await _accountRepo.GetfollowerCount(FollowersModel);
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
                foreach (ErrorInfo lst1 in RequestModel.Errors)
                {
                    ResponceMessage = lst1.ErrorMessage;
                }
            }
            //return FromExecutionResult(await _accountRepo.GetfollowerCount(ID));
            if(RequestModel.Value != null)
            {                
                return new JsonResult(new { Result = RequestModel.Value, meta = new { status = RequestModel.Success, message = ResponceMessage } });
            }           
            else
                return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });
        }
    }
}
