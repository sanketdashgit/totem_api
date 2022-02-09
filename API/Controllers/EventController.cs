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
using Totem.Business.Core.DataTransferModels.Event;
using Microsoft.AspNetCore.Authorization;

namespace Totem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : BaseController
    {
        #region Constructor
        private readonly TokenManager _tokenManger;
        private readonly IEventRepository _accountRepo;
        private readonly IDocumentRepository _documentRepo;
        public EventController(TokenManager tokenManager, IEventRepository accountRepo, IDocumentRepository DocRepository)
        {
            _accountRepo = accountRepo;
            _tokenManger = tokenManager;
            _documentRepo = DocRepository;
        }
        #endregion


        [HttpPost("CreateEvent")]


        public async Task<IActionResult> CreateEvent(TblEventModel TblEventModel)
        {
            //return FromExecutionResult(await _accountRepo.CreateEvent(TblEventModel));

            ExecutionResult<TblEventModel> RequestModel = await _accountRepo.CreateEvent(TblEventModel);

            string ResponceMessage = "";
            if (RequestModel.Success)
            {

                foreach (InfoMessage lst in RequestModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
                return new JsonResult(new { Result = RequestModel.Value, meta = new { status = RequestModel.Success, message = ResponceMessage } });
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

        [HttpPost("CreateEventwithvanueId")]


        public async Task<IActionResult> CreateEventwithvanueId(TblEventwithvanueIdModel TblEventModel)
        {
            //return FromExecutionResult(await _accountRepo.CreateEvent(TblEventModel));

            ExecutionResult<TblEventModel> RequestModel = await _accountRepo.CreateEventwithvanueId(TblEventModel);

            string ResponceMessage = "";
            if (RequestModel.Success)
            {

                foreach (InfoMessage lst in RequestModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
                return new JsonResult(new { Result = RequestModel.Value, meta = new { status = RequestModel.Success, message = ResponceMessage } });
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



        [HttpPost("CreateEventByPost")]
        public async Task<IActionResult> CreateEventByPost(TblEventByPostModel TblEventModel)
        {
            //return FromExecutionResult(await _accountRepo.CreateEvent(TblEventModel));

            ExecutionResult<TblEventByPostModel> RequestModel = await _accountRepo.CreateEventByPost(TblEventModel);

            string ResponceMessage = "";
            if (RequestModel.Success)
            {

                foreach (InfoMessage lst in RequestModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
                return new JsonResult(new { Result = RequestModel.Value, meta = new { status = RequestModel.Success, message = ResponceMessage } });
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

        [HttpPost("EventFileUpload")]
        public async Task<IActionResult> EventFileUpload([FromForm] FileUploadRequestModel fileUpload)
        {
            return FromExecutionResult(await _documentRepo.EventFileUpload(fileUpload));

        }
        [HttpPost("TestingUploadfile")]
        public async Task<IActionResult> TestingUploadfile([FromForm] TestModelModel fileUpload)
        {
            return FromExecutionResult(await _documentRepo.TestingUploadfile(fileUpload));

        }
        [HttpPost("TestingPathReturn")]
        public async Task<IActionResult> TestingPathReturn(TestRequestModel fileUpload)
        {
            //return FromExecutionResult(await _documentRepo.TestingPathReturn(fileUpload));

            ExecutionResult<TestResponceModel> RequestModel = await _documentRepo.TestingPathReturn(fileUpload);

            string ResponceMessage = "";
            if (RequestModel.Success)
            {

                foreach (InfoMessage lst in RequestModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
                return new JsonResult(new { Result = RequestModel.Value, meta = new { status = RequestModel.Success, message = ResponceMessage } });
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
        //ReadAccessFile
        [HttpPost("ReadAccessFile")]
        public async Task<IActionResult> ReadAccessFile(TestRequestModel fileUpload)
        {
            //return FromExecutionResult(await _documentRepo.TestingPathReturn(fileUpload));

            ExecutionResult<TestResponceModel> RequestModel = await _documentRepo.ReadAccessFile(fileUpload);

            string ResponceMessage = "";
            if (RequestModel.Success)
            {

                foreach (InfoMessage lst in RequestModel.Messages)
                {
                    ResponceMessage = lst.MessageText;
                }
                return new JsonResult(new { Result = RequestModel.Value, meta = new { status = RequestModel.Success, message = ResponceMessage } });
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

        [HttpPost("GetByEventID")]
        //[Authorize]
        public IActionResult GetByEventID(GetEvent GetEvent)
        {
            // return FromExecutionResult(_accountRepo.GetEvent(listModel));

            ExecutionResult<VEventDetai> RequestModel = _accountRepo.GetByEventID(GetEvent);
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

        [HttpPost("GetAllByPostEvent")]
        //[Authorize]
        public IActionResult GetAllByPostEvent(GetAllEvent GetAllEvent)
        {
            // return FromExecutionResult(_accountRepo.GetEvent(listModel));

            ExecutionResult<PaginatedResponse<VEventDetai>> RequestModel = _accountRepo.GetAllByPostEvent(GetAllEvent);
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





        [HttpPost("GetAllEvent")]
        public IActionResult GetAllEvent(GetAllEvent GetAllEvent)
        {
            // return FromExecutionResult(_accountRepo.GetEvent(listModel));

            ExecutionResult<PaginatedResponse<VEventDetai>> RequestModel = _accountRepo.GetAllEvent(GetAllEvent);
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


        [HttpPost("GetAllUpcomingEvent")]
        public IActionResult GetAllUpcomingEvent(GetAllEvent GetAllEvent)
        {
            // return FromExecutionResult(_accountRepo.GetEvent(listModel));

            ExecutionResult<PaginatedResponse<VEventDetai>> RequestModel = _accountRepo.GetAllUpcomingEvent(GetAllEvent);
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

        [HttpPost("GoNextEvent")]
        public IActionResult GoNextEvent(GetAllEvent GetAllEvent)
        {
            // return FromExecutionResult(_accountRepo.GetEvent(listModel));

            ExecutionResult<PaginatedResponse<VEventDetai>> RequestModel = _accountRepo.GoNextEvent(GetAllEvent);
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


        [HttpGet("AdminGetAllEvent")]
        public IActionResult AdminGetAllEvent()
        {
            return FromExecutionResult(_accountRepo.AdminGetAllEvent());
        }

        [HttpPost("AdminGetByEventID")]
        public IActionResult AdminGetByEventID(GetEvent GetEvent)
        {
            return FromExecutionResult(_accountRepo.GetByEventID(GetEvent));
        }



        [HttpPost("AddEventFeed")]
        //[Authorize]
        public async Task<IActionResult> AddEventFeed(AddEventFeed AddEventFeed)
        {
            // return FromExecutionResult(_accountRepo.GetEvent(listModel));

            ExecutionResult<AddEventFeed> RequestModel = await _accountRepo.AddEventFeed(AddEventFeed);
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
        /// Used to delete Post
        /// </summary>
        /// <param name="removePostModel">Details to remove post</param>
        /// <returns>Successfully delete or not</returns>
        [HttpPost("RemoveEvent")]
        public async Task<IActionResult> RemoveEvent(GetEvent GetEvent)
        {
            ExecutionResult RequestModel = await _accountRepo.RemoveEvent(GetEvent);
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




        #region Mohsin &Krupa
        //Add Event comment Mohsin
        [HttpPost("CreateLikeoncomment")]
        public async Task<IActionResult> CreateLikeoncomment(EventAboutfeed obj)
        {
            ExecutionResult<EventAboutfeed> RequestModel = await _accountRepo.CreateLikeoncomment(obj);
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

        //Get User list Mohsin
        [HttpPost("GetUserListByEventType")]
        public IActionResult GetUserListByEventType(GetUserListByEventType obj)
        {
            //return FromExecutionResult(await _accountRepo.GetUserListByEventType(obj));

            ExecutionResult<List<UserEventFeed>> RequestModel = _accountRepo.GetUserListByEventType(obj);
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


        //Add Event comment Krupa
        [HttpPost("AddCommentOnEvent")]
        public async Task<IActionResult> AddCommentOnEvent(AddCommentOnEvent AddCommentOnEvent)
        {
            ExecutionResult<AddCommentOnEvent> RequestModel = await _accountRepo.AddCommentOnEvent(AddCommentOnEvent);
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
        //Add Event comment Krupa
        //Add Reply on Event Comments Krupa
        [HttpPost("UpdateCommentReply")]
        public async Task<IActionResult> UpdateCommentReply(UpdateCommentReply UpdateCommentReply)
        {
            ExecutionResult<UpdateCommentReply> RequestModel = await _accountRepo.UpdateCommentReply(UpdateCommentReply);
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

        [HttpPost("RemoveEventComments")]
        public async Task<IActionResult> RemoveEventComments(RemoveEventCommentModel RemoveEventCommentModel)
        {
            ExecutionResult RequestModel = await _accountRepo.RemoveEventComments(RemoveEventCommentModel);
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
        //RemoveEventCommentReply
        [HttpPost("RemoveEventCommentReply")]
        public async Task<IActionResult> RemoveEventCommentReply(RemoveEventCommentReplyModel RemoveCommentReply)
        {
            ExecutionResult RequestModel = await _accountRepo.RemoveEventCommentReply(RemoveCommentReply);
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
        //Add Event comment Krupa
        [HttpPost("GetUserReplyOnEventComments")]
        public IActionResult GetUserReplyOnEventComments(GetCommentsReply obj)
        {
            ExecutionResult<List<GetUserReplyonEventCommentsModel>> RequestModel = _accountRepo.GetUserReplyOnEventComments(obj);
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


        //Add Support Krupa
        [HttpPost("AddSupport")]
        public async Task<IActionResult> AddSupport(AddSupport obj)
        {
            ExecutionResult<AddSupport> RequestModel = await _accountRepo.AddSupport(obj);
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

        //Get Supports by Krupa
        [HttpPost("GetSupportByUserId")]
        public IActionResult GetSupportByUserId(GetSupportByUserId obj)
        {
            ExecutionResult<List<GetAllSupportUser>> RequestModel = _accountRepo.GetSupportByUserId(obj);
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

        //Add Event comment Krupa
        [HttpPost("GetEventComments")]
        public IActionResult GetEventComments(GetEventCom obj)
        {
            ExecutionResult<PaginatedResponse<GetEventCommentsModel>> RequestModel = _accountRepo.GetEventComments(obj);
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
        #endregion

    }
}
