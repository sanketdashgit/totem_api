using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Event
{

    public class GetEvent 
    {
        public long EventId { get; set; }
        public long UserID { get; set; }
    }


    public class AddEventFeed
    {
        [Range(1, long.MaxValue, ErrorMessage = "Please enter a EventId value bigger than {1}")]
        public long EventId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Please enter a ID value bigger than {1}")]
        public long Id { get; set; }
        public bool Golive { get; set; }
        public bool Interest { get; set; }
        public bool Favorite { get; set; }
    }

    public class GetAllEvent : PaginationBaseRequestModel
    {
        public long EventId { get; set; }
        public long UserID { get; set; }
    }
    public class TblEventModel
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }      
        public string Details { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
       
    }

    public class TblEventwithvanueIdModel
    {
       
        public long EventId { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }       
        public string Address { get; set; }       
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string VanueId { get; set; }
        public string Image { get; set; }

    }

    public class TblEventByPostModel
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

    }


    public class EventModel
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public string EventName { get; set; }
       
        public string Address { get; set; }
       
        public string Details { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public List<TblEventFile> EventImages { get; set; }       
        public int IsActive { get; set; }


       
    }


    public partial class TblEventFile
    {
        public long FileId { get; set; }
        public long EventId { get; set; }
        public string Downloadlink { get; set; }
        public string Title { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
    }

    public partial class VEventDetai
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
       
        public string Details { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public bool? Status { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public bool? ProfileVerified { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool Golive { get; set; }
        public bool Interest { get; set; }
        public bool Favorite { get; set; }

        public int GoliveCount { get; set; }
        public int InterestCount { get; set; }
        public int FavoriteCount { get; set; }
        public string VanueId { get; set; }
        public List<TblEventFile> EventImages { get; set; }
    }








    #region Mohsin &Krupa
    //Add Event comment Mohsin
    public partial class EventAboutfeed
    {
        public long FeedId { get; set; }
        public long Id { get; set; }
        public long? CommentId { get; set; }
        public bool Like { get; set; }
    }
    //Add Event comment Mohsin
    public partial class GetUserListByEventType
    {
        public long EventId { get; set; }
        public string Type { get; set; }
    }

    //Add Event comment Mohsin
    public partial class UserEventFeed
    {
        public long EventId { get; set; }
        public string EventName { get; set; }
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public bool? ProfileVerified { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Golive { get; set; }
        public bool Interest { get; set; }
        public bool Favorite { get; set; }
        public string Image { get; set; }
    }



    //Add Event comment Krupa
    public class GetCommentsReply
    {
        public long CommentID { get; set; }
    }

    public partial class GetUserReplyonEventCommentsModel
    {
        public long CommentID { get; set; }
        public string CommentBody { get; set; }
        public long EventId { get; set; }
        public string EventName { get; set; }
        public List<UserDataforReply> UserDataforReply { get; set; }
    }

    public partial class UserDataforReply
    {
        public long ReplyID { get; set; }
        public string ReplyBody { get; set; }
        public long UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public bool? ProfileVerified { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
    }


    //Add supoort comment Krupa
    public partial class AddSupport
    {
        public long SupportID { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }

    public class GetSupportByUserId
    {
        public long UserId { get; set; }

    }

    public class ChangeEventActive 
    {
        public long EventId { get; set; }
    }

    public partial class GetAllSupportUser
    {
        public long SupportID { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
       
    }

    public partial class AddCommentOnEvent
    {
        public long CommentID { get; set; }
        public long Id { get; set; }
        public long EventId { get; set; }
        public string CommentBody { get; set; }
    }

    //Udpate Reply on comment Krupa
    public partial class UpdateCommentReply
    {
        public long ReplyID { get; set; }
        public long CommentID { get; set; }
        public long Id { get; set; }
        public string ReplyBody { get; set; }
    }

    public class RemoveEventCommentReplyModel
    {
        public long ReplyId { get; set; }
        public long Id { get; set; }
    }
    public class RemoveEventCommentModel
    {
        public long CommentId { get; set; }
        public long Id { get; set; }
    }

    //Get Event comments Krupa
    public partial class GetEventCommentsModel
    {
        public long EventId { get; set; }
        public string EventName { get; set; }
        public List<UserGetComments> UsersEventsComments { get; set; }
    }

    //Get Event comments Krupa
    public class GetEventCom : PaginationBaseRequestModel
    {
        public long EventId { get; set; }
        public long Id { get; set; }
    }

    //Get Event comments Krupa
    public partial class UserGetComments 
    {
        public long CommentID { get; set; }
        public string CommentBody { get; set; }
        public long UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public bool? ProfileVerified { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public bool likeStatus { get; set; }
        public long TotalLike { get; set; }
        public List<UserDataforReply> ReplyBody { get; set; }
    }
    #endregion
}
