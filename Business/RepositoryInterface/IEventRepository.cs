using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Account;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Totem.Business.Core.DataTransferModels.WebsiteDesign;
using Totem.Business.Core.Consts;
using Totem.Business.Core.DataTransferModels.Event;
using Totem.Business.Core.DataTransferModels.Documents;

namespace Totem.Business.RepositoryInterface
{
    public interface IEventRepository
    {
        // Create Event
        Task<ExecutionResult<TblEventModel>> CreateEvent(TblEventModel TblEventModel);
        Task<ExecutionResult<TblEventModel>> CreateEventwithvanueId(TblEventwithvanueIdModel TblEventModel);
        Task<ExecutionResult<TblEventByPostModel>> CreateEventByPost(TblEventByPostModel TblEventModel);
        ExecutionResult<VEventDetai> GetByEventID(GetEvent GetEvent);

        ExecutionResult<List<VEventDetai>> AdminGetAllEvent();

        // ExecutionResult<PaginatedResponse<EventModel>> GetAllEvent(GetAllEvent GetAllEvent);
        ExecutionResult<PaginatedResponse<VEventDetai>> GetAllEvent(GetAllEvent GetAllEvent);
        ExecutionResult<PaginatedResponse<VEventDetai>> GetAllUpcomingEvent(GetAllEvent GetAllEvent);

        ExecutionResult<PaginatedResponse<VEventDetai>> GetAllByPostEvent(GetAllEvent GetAllEvent);
        ExecutionResult<PaginatedResponse<VEventDetai>> GoNextEvent(GetAllEvent GetAllEvent);

        Task<ExecutionResult<AddEventFeed>> AddEventFeed(AddEventFeed AddEventFeed);

        Task<ExecutionResult> RemoveEvent(GetEvent GetEvent);

        #region Mohsin &Krupa

        //Add Event comment Mohsin
        Task<ExecutionResult<EventAboutfeed>> CreateLikeoncomment(EventAboutfeed obj);
        //Get User list Mohsin 
        ExecutionResult<List<UserEventFeed>> GetUserListByEventType(GetUserListByEventType obj);



        //Add Event comment Krupa
        Task<ExecutionResult<AddCommentOnEvent>> AddCommentOnEvent(AddCommentOnEvent AddCommentOnEvent);
        //Udpate Reply on comment Krupa
        Task<ExecutionResult<UpdateCommentReply>> UpdateCommentReply(UpdateCommentReply UpdateCommentReply);

        Task<ExecutionResult> RemoveEventComments(RemoveEventCommentModel RemoveComment);
        Task<ExecutionResult> RemoveEventCommentReply(RemoveEventCommentReplyModel CommentsReply);
        //Get User Reply on EventComments Krupa
        ExecutionResult<List<GetUserReplyonEventCommentsModel>> GetUserReplyOnEventComments(GetCommentsReply obj);
        //Get EventComments Krupa
        ExecutionResult<PaginatedResponse<GetEventCommentsModel>> GetEventComments(GetEventCom obj);


        //Add Support Krupa
        Task<ExecutionResult<AddSupport>> AddSupport(AddSupport obj);
        //Get Supports By user Krupa
        ExecutionResult<List<GetAllSupportUser>> GetSupportByUserId(GetSupportByUserId obj);
        #endregion
    }
}
