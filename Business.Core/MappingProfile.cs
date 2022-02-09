using AutoMapper;
using Totem.Business.Core.DataTransferModels.Account;
using Totem.Business.Core.DataTransferModels.WebsiteDesign;
using Totem.Business.Core.DataTransferModels.Admin.Innovation;
using Totem.Business.Core.DataTransferModels.Admin.ProcedureType;
using Totem.Business.Core.DataTransferModels.Admin.ProjectType;
using Totem.Business.Core.DataTransferModels.Admin.Questionary;

using Totem.Database.Models;
using Totem.Business.Core.DataTransferModels.Documents;
using Totem.Business.Core.DataTransferModels.Event;
using Totem.Business.Core.DataTransferModels.Spotify;
using Totem.Database.StoreProcedure;
using Totem.Business.Core.DataTransferModels.Post;

namespace Totem.Business.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Account
            CreateMap<Usermanagement, CreateAccountRequestModel>().ReverseMap();
            CreateMap<Usermanagement, UsermanagementDetailsID>().ReverseMap();
            CreateMap < Usermanagement, GetAllUserStatuswithNotification>().ReverseMap();
            CreateMap<Usermanagement, GetAllUserStatusModelMobile>().ReverseMap();
            CreateMap<Usermanagement, UsermanagementDetails>().ReverseMap();
            CreateMap<Usermanagement, InsertUsermanagement>().ReverseMap();
            CreateMap<Usermanagement, FollowUsers>().ReverseMap();
            CreateMap<Usermanagement, PostLikedUsers>().ReverseMap();
            CreateMap<VGetUserdetail, V_UserdetailModel>().ReverseMap();
            CreateMap<Sp_GetUserDetails, GetAllUserStatusModel>().ReverseMap();
            CreateMap<TblFollower, Follow>().ReverseMap();
            CreateMap<ContactU, ContactUs>().ReverseMap();
            CreateMap<Faq, FAQDetails>().ReverseMap();
            CreateMap<Faq, FAQ>().ReverseMap();
            CreateMap<VDashbord, DashbordModel>().ReverseMap();
            CreateMap<TblBlockUser, BlockUserModel>().ReverseMap();
            CreateMap<TblNotification, NotificationModel>().ReverseMap();
            CreateMap<NotificationModel, TblNotification>().ReverseMap();
            CreateMap<TblUserFcm, UserFcmModel>().ReverseMap();
            CreateMap<TblEarlyToParty, EarlyToPartyModel>().ReverseMap();

            CreateMap<TblProfile, VerifyProfile>().ReverseMap();
            CreateMap<TblBusiness, BusinessModel>().ReverseMap();
            
            CreateMap<GetVersionmodel, TblVersion>().ReverseMap();
            CreateMap<TblVersion, GetVersionmodel>().ReverseMap();
            #endregion
            CreateMap<WebsiteContent, WebsiteConten>().ReverseMap();
            CreateMap<WebsiteContent, WebsiteContenID>().ReverseMap();
            CreateMap<TblUpdateEmail, TblUpdatEmail>().ReverseMap();
            CreateMap<TblUpdateEmail, TblUpdatEmail>().ReverseMap();
            CreateMap <TblDeleteUser, DeleteSuggested>().ReverseMap();

            #region Post
            CreateMap<TblPostFile, PostMediaModel>().ReverseMap();
            CreateMap<TblMemorieFile, MemorieMediaModel>().ReverseMap();
            CreateMap<MemorieMediaModel, TblMemorieFile>().ReverseMap();
            CreateMap<TblMemorie, UserMemorysDetailsModel>().ReverseMap();
            CreateMap<UserMemorysDetailsModel, TblMemorie>().ReverseMap();
            CreateMap<VAdminPostDetail, AdminUserPostsModel>().ReverseMap();
            CreateMap<TblPostCommentReply, PostCommentReply>().ReverseMap();
            CreateMap<TblPostComment, PostCommentModel>().ReverseMap();
            CreateMap<TblBlockPost, BlockPostResponce>().ReverseMap();
            CreateMap<CreateMemorieFiles,TblMemorieFile>().ReverseMap();
            CreateMap<TblMemorieFile, CreateMemorieFiles>().ReverseMap();

            #endregion

            #region Event

            CreateMap<TblEvent, TblEventwithvanueIdModel>().ReverseMap();
            CreateMap<TblEventwithvanueIdModel, TblEvent>().ReverseMap();
            CreateMap<TblEvent, TblEventModel>().ReverseMap();
            CreateMap<TblEventModel, TblEvent>().ReverseMap();
            CreateMap<TblEventByPostModel, TblEvent>().ReverseMap();            
            CreateMap<TblEvent, EventModel>().ReverseMap();
            CreateMap<TblEventUserFile, TblEventFile>().ReverseMap();
            CreateMap<VEventDetail, VEventDetai>().ReverseMap();

            CreateMap<Sp_GetEvent, VEventDetai>().ReverseMap();
            CreateMap<AddEventFeed, TblEventfeed>().ReverseMap();
            CreateMap<TblEventfeed, AddEventFeed>().ReverseMap();

            //AddEventFeed

            #endregion


            #region Spotify
            CreateMap<TblArtist, Artists>().ReverseMap();
            CreateMap<TblFavouriteEvent, FavouriteEvents>().ReverseMap();
            CreateMap<TblGenre, Genres>().ReverseMap();
            CreateMap<TblSong, Songs>().ReverseMap();

            CreateMap<Artists, TblArtist>().ReverseMap();
            CreateMap<FavouriteEvents, TblFavouriteEvent>().ReverseMap();
            CreateMap<Genres, TblGenre>().ReverseMap();
            CreateMap<Songs, TblSong>().ReverseMap();

            #endregion


            #region file upload
            CreateMap<TblUserFile, FileUploadRequestModel>();
            CreateMap<FileUploadRequestModel, TblUserFile>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<TblUserFile, FileUploadResponseModel>();
            CreateMap<FileUploadResponseModel, TblUserFile>().ForMember(x => x.Id, opt => opt.Ignore());
            #endregion






            #region Mohsin &Krupa


            //Tbl_EventAboutfeeds Mohsin
            CreateMap<TblEventAboutfeed, EventAboutfeed>().ReverseMap();
            CreateMap<TblEventAboutfeed, EventAboutfeed>().ReverseMap();
            CreateMap<EventAboutfeed, TblEventAboutfeed>().ReverseMap();

            //Tbl_Eventfeeds Mohsin
            CreateMap<TblEventfeed, UserEventFeed>().ReverseMap();
            CreateMap<TblEventfeed, UserEventFeed>().ReverseMap();
            CreateMap<UserEventFeed, TblEventfeed>().ReverseMap();

            //AddEventComment Krupa
            CreateMap<AddCommentOnEvent, TblEventComment>().ReverseMap();
            CreateMap<TblEventComment, AddCommentOnEvent>().ReverseMap();

            //UpdateEventCommentResultBody Krupa
            CreateMap<UpdateCommentReply, TblEventCommentsReply>().ReverseMap();
            CreateMap<TblEventCommentsReply, UpdateCommentReply>().ReverseMap();

            //GetUserReplysonEventComments Krupa
            CreateMap<GetUserReplyonEventCommentsModel, TblEventCommentsReply>().ReverseMap();
            CreateMap<TblEventCommentsReply, GetUserReplyonEventCommentsModel>().ReverseMap();


            //Add Support Krupa
            CreateMap<AddSupport, TblSupport>().ReverseMap();
            CreateMap<TblSupport, AddSupport>().ReverseMap();
            //Get Supports Krupa
            CreateMap<GetAllSupportUser, TblSupport>().ReverseMap();
            CreateMap<TblSupport, GetAllSupportUser>().ReverseMap();


            //GetEventComments Krupa
            CreateMap<GetEventCommentsModel, TblEventComment>().ReverseMap();
            CreateMap<TblEventComment, GetEventCommentsModel>().ReverseMap();

            #endregion

        }
    }
}
