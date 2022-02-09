using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totem.Business.Core.AppSettings;
using Totem.Business.Core.Consts;
using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Account;
using Totem.Business.Helpers;
using Totem.Business.RepositoryInterface;
using Totem.Database.Models;
using Totem.Externals.Caching;

namespace Totem.Business.Repositories
{
    public class FollowersRepository : IFollowersRepository
    {
        #region Constructor

        private readonly TotemDBContext _dbContext;
        private readonly INotificationRepository _NotificationRepo;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;
        private readonly TokenManager _tokenManger;
        private readonly CommonMethods _commonMethods;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly string _connectionString;

        public FollowersRepository(
            TotemDBContext dbContext,
            IMapper mapper,
            TokenManager tokenManger,
            IOptions<AppSettings> appSettings,
            CommonMethods commonMethods, INotificationRepository NotificationRepo, ICacheManager cacheManager)
        {
            _commonMethods = commonMethods;
            _appSettings = appSettings;
            _tokenManger = tokenManger;
            _mapper = mapper;
            _dbContext = dbContext;
            _NotificationRepo = NotificationRepo;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            _cacheManager = cacheManager;
        }

        public void SetCache<T>(string key, T value) where T : class
        {

            _cacheManager.Set(key, value);
        }
        public T GetCache<T>(string key) where T : class
        {
            T result = default(T);
            result = _cacheManager.Get<T>(key);
            return result;
        }

        #endregion


        #region Get Users Followers and Following count
        public async Task<ExecutionResult<FollowersCountModel>> GetfollowerCount(FollowersModel FollowersModel)
        {
            FollowersCountModel FollowersCountModel = new FollowersCountModel();

            var UserBlocked = _dbContext.TblBlockUsers.Where(x => x.Id == FollowersModel.ID && x.BlockId == FollowersModel.UserID).FirstOrDefault();
            if (UserBlocked != null)
            {
                return new ExecutionResult<FollowersCountModel>(new ErrorInfo(string.Format(MessageHelper.Account.UserBlocked)));
            }
            var AllUsers = _dbContext.Usermanagements.Where(x => x.Id == FollowersModel.ID && x.IsActive == true).FirstOrDefault();

            if (AllUsers == null)
            {
                return new ExecutionResult<FollowersCountModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
            }

            FollowersCountModel.ProfileDetails = _mapper.Map<UsermanagementDetailsID>(AllUsers);

            var Deletesugg = _dbContext.TblDeleteUsers
         .Where(o => o.UserId == FollowersModel.ID)
         .Select(o => o.DeleteUserId);

            FollowersCountModel.followers = _dbContext.TblFollowers
           .Where(o => o.FollowerId == FollowersModel.ID && o.RequestAccepted == true && !Deletesugg.Contains(o.UserId) && o.IsDeleted == false)// && o.RequestAccepted == true
           .Count();

            FollowersCountModel.following = _dbContext.TblFollowers
            .Where(o => o.UserId == FollowersModel.ID && o.RequestAccepted == true && !Deletesugg.Contains(o.FollowerId) && o.IsDeleted == false)// && o.RequestAccepted == true
            .Count();

            FollowersCountModel.PostCount = _dbContext.TblPosts
            .Where(o => o.Id == FollowersModel.ID && o.IsActive == 1)// && o.RequestAccepted == true
            .Count();

            var isfollow = _dbContext.TblFollowers.Where(x => x.UserId == FollowersModel.UserID && x.FollowerId == FollowersModel.ID && x.IsDeleted == false).FirstOrDefault(); ;
            if (isfollow != null)
            {
                if (isfollow.RequestAccepted == true)
                    FollowersCountModel.isfollow = 1;
                else
                    FollowersCountModel.isfollow = 2;
            }
            else
            {
                FollowersCountModel.isfollow = 0;
            }

            return new ExecutionResult<FollowersCountModel>(FollowersCountModel);
        }


        #endregion


        #region Get Users Followers and Following count
        public async Task<ExecutionResult<PaginatedResponse<FollowUsers>>> GetfollowerRequest(FollowModel FollowModel)
        {
            PaginatedResponse<FollowUsers> response = new PaginatedResponse<FollowUsers>();
            var count = _dbContext.TblFollowers
            .Where(o => o.FollowerId == FollowModel.ID && o.RequestAccepted == false && o.IsDeleted == false)
            .Select(o => o.UserId);

            var follow = _dbContext.TblFollowers
       .Where(o => o.UserId == FollowModel.ID && o.RequestAccepted == true && o.IsDeleted == false)
       .Select(o => o.FollowerId);

            if (FollowModel.PageSize != 0 && FollowModel.PageNumber != 0)
            {
                var AllUsers = _dbContext.Usermanagements.Where(x => count.Contains(x.Id) && x.IsActive == true)
                .Skip((FollowModel.PageNumber - 1) * FollowModel.PageSize)
                                                .Take(FollowModel.PageSize)
                .OrderBy(x => x.Username).ToList();
                response.Data = _mapper.Map<List<FollowUsers>>(AllUsers);
            }
            else
            {
                var AllUsers = _dbContext.Usermanagements.Where(x => count.Contains(x.Id) && x.IsActive == true)
               .OrderBy(x => x.Username).ToList();
                response.Data = _mapper.Map<List<FollowUsers>>(AllUsers);
            }

            foreach (var item in response.Data)
            {
                item.mutualCount = _dbContext.TblFollowers.Where(x => x.UserId == item.Id && follow.Contains(x.UserId) && x.FollowerId == FollowModel.ID && x.RequestAccepted == true && x.IsDeleted == false).Count();
            }

            response.PageNumber = FollowModel.PageNumber;
            response.PageSize = FollowModel.PageSize;
            response.TotalRecords = FollowModel.TotalRecords > 0 ? FollowModel.TotalRecords :
                _dbContext.Usermanagements.Where(x => count.Contains(x.Id) && x.IsActive == true).Count();
            return response.Data.Count > 0 ? new ExecutionResult<PaginatedResponse<FollowUsers>>(_mapper.Map<PaginatedResponse<FollowUsers>>(response)) :
               new ExecutionResult<PaginatedResponse<FollowUsers>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }


        #endregion


        #region Follow request add and remove
        public async Task<ExecutionResult> Follow(Follow Follow, SendNotification sendNotification)
        {
            if (Follow == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            var account = _dbContext.TblFollowers.Where(x => x.UserId == Follow.UserId && x.FollowerId == Follow.FollowerId).Any();
            if (account)
            {
                var followchange = _dbContext.TblFollowers.Where(x => x.UserId == Follow.UserId && x.FollowerId == Follow.FollowerId);
                foreach (var item in followchange)
                {
                    _dbContext.TblFollowers.Remove(item);

                }
                await _dbContext.SaveChangesAsync();


                var notificationremove = _dbContext.TblNotifications.Where(x => x.NotificationTypeId == Follow.UserId && x.Id == Follow.FollowerId).ToList();
                foreach (var item in notificationremove)
                {
                    item.Readflag = "3";                   
                }
                await _dbContext.SaveChangesAsync();

                var notificationupdate = _dbContext.TblNotifications.Where(x => x.NotificationTypeId == Follow.FollowerId && x.NotificationType == "FOLLOW" && x.Id == Follow.UserId).ToList();
                foreach (var item in notificationupdate)
                {
                    item.RequestAccepted = 0;
                }
                await _dbContext.SaveChangesAsync();

                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Unfollow", "Requested", "")));
            }
            else
            {
                var isPrivate = _dbContext.Usermanagements.Where(x => x.Id == Follow.FollowerId).FirstOrDefault();
                var appuser = _dbContext.Usermanagements.Where(x => x.Id == Follow.UserId).FirstOrDefault();

                if (isPrivate != null)
                {
                    var createAcc = _mapper.Map<TblFollower>(Follow);
                    createAcc.CreatedDate = DateTime.UtcNow;
                    createAcc.ModifiedDate = DateTime.UtcNow;
                    if (isPrivate.IsPrivate == false)
                    {
                        createAcc.RequestAccepted = true;
                    }
                    await _dbContext.TblFollowers.AddAsync(createAcc);
                    await _dbContext.SaveChangesAsync();

                    if (isPrivate.FollowNotification == true)
                    {
                        var fcmdetails = _dbContext.TblUserFcms.Where(x => x.Id == isPrivate.Id && x.Login == true).ToList();
                        sendNotification.notification.title = "Follow Request";
                        if (isPrivate.IsPrivate == true)
                            sendNotification.notification.body = appuser.Firstname + " " + appuser.Lastname + " has sent you a follow request.";
                        else
                            sendNotification.notification.body = appuser.Firstname + " " + appuser.Lastname + " is following you now.";
                        sendNotification.notification.image = appuser.Image;
                        sendNotification.data.conversationId = appuser.Id;
                        sendNotification.data.conversationInfo = "";
                        sendNotification.data.type = 1;
                        foreach (var item in fcmdetails)
                        {
                            sendNotification.registration_ids.Add(item.Fcm);
                        }
                        await _NotificationRepo.NotifyAsync(sendNotification);

                    }
                    TblNotification tblNotification = new TblNotification();
                    tblNotification.Ssrno = 0;
                    tblNotification.Date = DateTime.UtcNow;
                    tblNotification.Id = isPrivate.Id;
                    tblNotification.Image = appuser.Image;
                    tblNotification.NuserName = appuser.Firstname + " " + appuser.Lastname;
                    tblNotification.Readflag = "0";
                    tblNotification.Title = "Follow Request";
                    if (isPrivate.IsPrivate == true)
                    {
                        tblNotification.Descp = " has sent you a follow request.";
                        tblNotification.NotificationType = "FOLLOW_REQUEST";
                    }
                    else
                    {
                        tblNotification.Descp = " is following you now.";
                        tblNotification.NotificationType = "FOLLOW";
                    }
                    tblNotification.NotificationTypeId = appuser.Id;
                    await _dbContext.TblNotifications.AddAsync(tblNotification);
                    await _dbContext.SaveChangesAsync();

                    var notificationupdate = _dbContext.TblNotifications.Where(x => x.NotificationTypeId == Follow.FollowerId && (x.NotificationType == "FOLLOW" || x.NotificationType == "FOLLOW_REQUEST") && x.Id == Follow.UserId).ToList();
                    foreach (var item in notificationupdate)
                    {
                        item.RequestAccepted = 1;
                    }
                    await _dbContext.SaveChangesAsync();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Follow", "Requested", "")));
                }
                else
                {
                    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SuccessMessage, "Follow", "Requested", "Not Sent User Not found")));
                }


            }
        }

        #endregion


        #region Delete from suggested
        public async Task<ExecutionResult> Deletesuggested(DeleteSuggested DeleteSuggested)
        {
            if (DeleteSuggested == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            var account = _dbContext.TblDeleteUsers.Where(x => x.UserId == DeleteSuggested.UserId && x.DeleteUserId == DeleteSuggested.DeleteUserId).Any();
            if (account)
            {
                var followchange = _dbContext.TblDeleteUsers.Where(x => x.UserId == DeleteSuggested.UserId && x.DeleteUserId == DeleteSuggested.DeleteUserId);
                foreach (var item in followchange)
                {
                    _dbContext.TblDeleteUsers.Remove(item);

                }
                await _dbContext.SaveChangesAsync();
                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Delete Suggested", "Removed", "Successfully")));
            }
            else
            {

                var createAcc = _mapper.Map<TblDeleteUser>(DeleteSuggested);
                createAcc.CreatedDate = DateTime.UtcNow;
                await _dbContext.TblDeleteUsers.AddAsync(createAcc);
                await _dbContext.SaveChangesAsync();
                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Delete Suggested", "added", "Successfully")));
            }
        }
        #endregion


        #region Get Users  Deleted from suggested 
        public async Task<ExecutionResult<PaginatedResponse<FollowUsers>>> GetAllDeletesuggested(FollowModel FollowModel)
        {
            PaginatedResponse<FollowUsers> response = new PaginatedResponse<FollowUsers>();

            var Deletesugg = _dbContext.TblDeleteUsers
       .Where(o => o.UserId == FollowModel.ID)
       .Select(o => o.DeleteUserId);

            if (FollowModel.PageSize != 0 && FollowModel.PageNumber != 0)
            {
                var AllUsers = _dbContext.Usermanagements.Where(x => Deletesugg.Contains(x.Id) && (x.Firstname.Contains(FollowModel.Search) || x.Username.Contains(FollowModel.Search) || x.Phone.Contains(FollowModel.Search) || x.Email.Contains(FollowModel.Search)))
                .Skip((FollowModel.PageNumber - 1) * FollowModel.PageSize)
                                                .Take(FollowModel.PageSize)
                                                .OrderBy(x => x.Username).ToList();
                response.Data = _mapper.Map<List<FollowUsers>>(AllUsers);
            }
            else
            {
                var AllUsers = _dbContext.Usermanagements.Where(x => Deletesugg.Contains(x.Id) && (x.Firstname.Contains(FollowModel.Search) || x.Username.Contains(FollowModel.Search) || x.Phone.Contains(FollowModel.Search) || x.Email.Contains(FollowModel.Search)))
                                               .OrderBy(x => x.Username).ToList();
                response.Data = _mapper.Map<List<FollowUsers>>(AllUsers);
            }


            response.PageNumber = FollowModel.PageNumber;
            response.PageSize = FollowModel.PageSize;
            response.TotalRecords = FollowModel.TotalRecords > 0 ? FollowModel.TotalRecords :
                _dbContext.Usermanagements.Where(x => Deletesugg.Contains(x.Id) && (x.Firstname.Contains(FollowModel.Search) || x.Username.Contains(FollowModel.Search) || x.Phone.Contains(FollowModel.Search) || x.Email.Contains(FollowModel.Search))).Count();

            foreach (var item in response.Data)
            {
                item.mutualCount = _dbContext.TblFollowers.Where(x => x.UserId == item.Id && x.FollowerId == FollowModel.ID && x.RequestAccepted == true && x.IsDeleted == false).Count();
            }


            return response.Data.Count > 0 ? new ExecutionResult<PaginatedResponse<FollowUsers>>(response) :
               new ExecutionResult<PaginatedResponse<FollowUsers>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }


        #endregion

        #region Approve Follow
        public async Task<ExecutionResult> ApproveFollow(Follow Follow)
        {
            if (Follow == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            var account = _dbContext.TblFollowers.Where(x => x.UserId == Follow.UserId && x.FollowerId == Follow.FollowerId).FirstOrDefault();
            if (account != null)
            {

                account.RequestAccepted = true;
                account.ModifiedDate = DateTime.UtcNow;
                _dbContext.SaveChanges();
                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Follow", "Approved", "")));

                var notificationremove = _dbContext.TblNotifications.Where(x => x.NotificationTypeId == Follow.UserId && x.Id == Follow.FollowerId).ToList();
                foreach (var item in notificationremove)
                {
                    item.Readflag = "3";                   
                }
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "Follow")));
            }
        }

        #endregion

        #region Get Users Followers 
        public async Task<ExecutionResult<PaginatedResponse<FollowUsers>>> GetAllfollower(FollowModel FollowModel)
        {
            PaginatedResponse<FollowUsers> response = new PaginatedResponse<FollowUsers>();


            var Deletesugg = _dbContext.TblDeleteUsers
           .Where(o => o.UserId == FollowModel.ID)
           .Select(o => o.DeleteUserId);

            var following = _dbContext.TblFollowers
            .Where(o => o.FollowerId == FollowModel.ID && o.RequestAccepted == true && !Deletesugg.Contains(o.UserId) && o.IsDeleted == false)
            .Select(o => o.UserId);


            var follow = _dbContext.TblFollowers
           .Where(o => o.UserId == FollowModel.ID && o.RequestAccepted == true && !Deletesugg.Contains(o.FollowerId) && o.IsDeleted == false)
           .Select(o => o.FollowerId);
            if (FollowModel.PageSize != 0 && FollowModel.PageNumber != 0)
            {
                var AllUsers = _dbContext.Usermanagements.Where(x => following.Contains(x.Id) && !Deletesugg.Contains(x.Id) && (x.Firstname.Contains(FollowModel.Search) || x.Username.Contains(FollowModel.Search) || x.Phone.Contains(FollowModel.Search) || x.Email.Contains(FollowModel.Search)))
                .Skip((FollowModel.PageNumber - 1) * FollowModel.PageSize)
                                                .Take(FollowModel.PageSize)
                .OrderBy(x => x.Username).ToList();

                response.Data = _mapper.Map<List<FollowUsers>>(AllUsers);
               
            }
            else
            {
                var AllUsers = _dbContext.Usermanagements.Where(x => following.Contains(x.Id) && !Deletesugg.Contains(x.Id) && (x.Firstname.Contains(FollowModel.Search) || x.Username.Contains(FollowModel.Search) || x.Phone.Contains(FollowModel.Search) || x.Email.Contains(FollowModel.Search)))
               .OrderBy(x => x.Username).ToList();

                response.Data = _mapper.Map<List<FollowUsers>>(AllUsers);
            }


            response.PageNumber = FollowModel.PageNumber;
            response.PageSize = FollowModel.PageSize;
            response.TotalRecords = FollowModel.TotalRecords > 0 ? FollowModel.TotalRecords :
                _dbContext.Usermanagements.Where(x => following.Contains(x.Id) && !Deletesugg.Contains(x.Id) && (x.Firstname.Contains(FollowModel.Search) || x.Username.Contains(FollowModel.Search) || x.Phone.Contains(FollowModel.Search) || x.Email.Contains(FollowModel.Search))).Count();


            foreach (var item in response.Data)
            {
                item.mutualCount = _dbContext.TblFollowers.Where(x => x.UserId == item.Id && follow.Contains(x.UserId) && x.FollowerId == FollowModel.ID && x.RequestAccepted == true && x.IsDeleted == false).Count();
            }

            foreach (var item in response.Data)
            {
                var isfollow = _dbContext.TblFollowers.Where(x => x.UserId == FollowModel.ID && x.FollowerId == item.Id && x.IsDeleted == false).FirstOrDefault(); ;
                if (isfollow != null)
                {
                    if (isfollow.RequestAccepted == true)
                        item.isfollow = 1;
                    else
                        item.isfollow = 2;
                }
                else
                {
                    item.isfollow = 0;
                }
            }

            return response.Data.Count > 0 ? new ExecutionResult<PaginatedResponse<FollowUsers>>(response) :
               new ExecutionResult<PaginatedResponse<FollowUsers>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }


        #endregion


        #region Get Users Follow
        public async Task<ExecutionResult<PaginatedResponse<FollowUsers>>> GetAllfollow(FollowModel FollowModel)
        {
            PaginatedResponse<FollowUsers> response = new PaginatedResponse<FollowUsers>();

            var Deletesugg = _dbContext.TblDeleteUsers
    .Where(o => o.UserId == FollowModel.ID)
    .Select(o => o.DeleteUserId);

            var follow = _dbContext.TblFollowers
            .Where(o => o.UserId == FollowModel.ID && o.RequestAccepted == true && !Deletesugg.Contains(o.FollowerId) && o.IsDeleted == false)
            .Select(o => o.FollowerId);



            if (FollowModel.PageSize != 0 && FollowModel.PageNumber != 0)
            {
                var AllUsers = _dbContext.Usermanagements.Where(x => follow.Contains(x.Id) && !Deletesugg.Contains(x.Id) && (x.Firstname.Contains(FollowModel.Search) || x.Username.Contains(FollowModel.Search) || x.Phone.Contains(FollowModel.Search) || x.Email.Contains(FollowModel.Search)))
                .Skip((FollowModel.PageNumber - 1) * FollowModel.PageSize)
                                                .Take(FollowModel.PageSize)
                                                .OrderBy(x => x.Username).ToList();
                response.Data = _mapper.Map<List<FollowUsers>>(AllUsers);
            }
            else
            {
                var AllUsers = _dbContext.Usermanagements.Where(x => follow.Contains(x.Id) && !Deletesugg.Contains(x.Id) && (x.Firstname.Contains(FollowModel.Search) || x.Username.Contains(FollowModel.Search) || x.Phone.Contains(FollowModel.Search) || x.Email.Contains(FollowModel.Search)))
                                               .OrderBy(x => x.Username).ToList();
                response.Data = _mapper.Map<List<FollowUsers>>(AllUsers);
            }


            response.PageNumber = FollowModel.PageNumber;
            response.PageSize = FollowModel.PageSize;
            response.TotalRecords = FollowModel.TotalRecords > 0 ? FollowModel.TotalRecords :
                _dbContext.Usermanagements.Where(x => follow.Contains(x.Id) && !Deletesugg.Contains(x.Id) && (x.Firstname.Contains(FollowModel.Search) || x.Username.Contains(FollowModel.Search) || x.Phone.Contains(FollowModel.Search) || x.Email.Contains(FollowModel.Search))).Count();

            foreach (var item in response.Data)
            {
                item.mutualCount = _dbContext.TblFollowers.Where(x => x.UserId == item.Id && follow.Contains(x.UserId) && x.FollowerId == FollowModel.ID && x.RequestAccepted == true && x.IsDeleted == false).Count();
            }


            return response.Data.Count > 0 ? new ExecutionResult<PaginatedResponse<FollowUsers>>(response) :
               new ExecutionResult<PaginatedResponse<FollowUsers>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }


        public async Task<ExecutionResult<PaginatedResponse<FollowUsers>>> TagAllfollow(FollowModel FollowModel)
        {
            PaginatedResponse<FollowUsers> response = new PaginatedResponse<FollowUsers>();

            var Deletesugg = _dbContext.TblDeleteUsers
    .Where(o => o.UserId == FollowModel.ID)
    .Select(o => o.DeleteUserId);

            var follow = _dbContext.TblFollowers
            .Where(o => o.UserId == FollowModel.ID && o.RequestAccepted == true && !Deletesugg.Contains(o.FollowerId) && o.IsDeleted == false)
            .Select(o => o.FollowerId);



            if (FollowModel.PageSize != 0 && FollowModel.PageNumber != 0)
            {
                var AllUsers = _dbContext.Usermanagements.Where(x => follow.Contains(x.Id) && !Deletesugg.Contains(x.Id) && (x.Firstname.Contains(FollowModel.Search) || x.Username.Contains(FollowModel.Search) || x.Phone.Contains(FollowModel.Search) || x.Email.Contains(FollowModel.Search)))
                .Skip((FollowModel.PageNumber - 1) * FollowModel.PageSize)
                                                .Take(FollowModel.PageSize)
                                                .OrderBy(x => x.Username).ToList();
                response.Data = _mapper.Map<List<FollowUsers>>(AllUsers);
            }
            else
            {
                var AllUsers = _dbContext.Usermanagements.Where(x => follow.Contains(x.Id) && !Deletesugg.Contains(x.Id) && (x.Firstname.Contains(FollowModel.Search) || x.Username.Contains(FollowModel.Search) || x.Phone.Contains(FollowModel.Search) || x.Email.Contains(FollowModel.Search)))
                                               .OrderBy(x => x.Username).ToList();
                response.Data = _mapper.Map<List<FollowUsers>>(AllUsers);
            }


            response.PageNumber = FollowModel.PageNumber;
            response.PageSize = FollowModel.PageSize;
            response.TotalRecords = FollowModel.TotalRecords > 0 ? FollowModel.TotalRecords :
                _dbContext.Usermanagements.Where(x => follow.Contains(x.Id) && !Deletesugg.Contains(x.Id) && (x.Firstname.Contains(FollowModel.Search) || x.Username.Contains(FollowModel.Search) || x.Phone.Contains(FollowModel.Search) || x.Email.Contains(FollowModel.Search))).Count();




            return response.Data.Count > 0 ? new ExecutionResult<PaginatedResponse<FollowUsers>>(response) :
               new ExecutionResult<PaginatedResponse<FollowUsers>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }


        #endregion


        #region Get All Users


        public ExecutionResult<PaginatedResponse<FollowUsers>> GetAllUsers(FollowModel listModel)
        {
            PaginatedResponse<FollowUsers> response = new PaginatedResponse<FollowUsers>();

            var Deletesugg = _dbContext.TblDeleteUsers
        .Where(o => o.UserId == listModel.ID)
        .Select(o => o.DeleteUserId);

            var Blockeduser = _dbContext.TblBlockUsers
        .Where(o => o.Id == listModel.ID)
        .Select(o => o.BlockId);

            var AlreadFollowed = _dbContext.TblFollowers
        .Where(o => o.UserId == listModel.ID && o.IsDeleted == false)
        .Select(o => o.FollowerId);
            var userdetails = _dbContext.Usermanagements.Where(x => x.IsActive == true && x.IsDeleted == false && !AlreadFollowed.Contains(x.Id) && !Blockeduser.Contains(x.Id) && !Deletesugg.Contains(x.Id) && (x.Firstname.Contains(listModel.Search) || x.Username.Contains(listModel.Search) || x.Phone.Contains(listModel.Search) || x.Email.Contains(listModel.Search)) && x.Id != listModel.ID).ToList();
            if (listModel.PageSize != 0 && listModel.PageNumber != 0)
            {
                var userdata = userdetails.Skip((listModel.PageNumber - 1) * listModel.PageSize)
                                                 .Take(listModel.PageSize)
                     .OrderBy(x => x.Username).ToList();
                response.Data = _mapper.Map<List<FollowUsers>>(userdata);
            }
            else
            {
                var userdata = userdetails.OrderBy(x => x.Username).ToList();
                response.Data = _mapper.Map<List<FollowUsers>>(userdata);

            }

            var follow = _dbContext.TblFollowers
          .Where(o => o.UserId == listModel.ID && o.RequestAccepted == true && o.IsDeleted == false)
          .Select(o => o.FollowerId);

            foreach (var item in response.Data)
            {
                item.mutualCount = _dbContext.TblFollowers.Where(x => x.UserId == item.Id && follow.Contains(x.UserId) && x.FollowerId == listModel.ID && x.RequestAccepted == true && x.IsDeleted == false).Count();


                var isfollow = _dbContext.TblFollowers.Where(x => x.UserId == listModel.ID && x.FollowerId == item.Id && x.IsDeleted == false).FirstOrDefault(); ;
                if (isfollow != null)
                {
                    if (isfollow.RequestAccepted == true)
                        item.isfollow = 1;
                    else
                        item.isfollow = 2;
                }
                else
                {
                    item.isfollow = 0;
                }
            }

            //response.Data =   _dbContext.Usermanagements.Where(x => x.IsActive == true)
            //     .Skip((listModel.PageNumber - 1) * listModel.PageSize)
            //                                .Take(listModel.PageSize)
            //    .OrderBy(x => x.Username).ToList();

            response.PageNumber = listModel.PageNumber;
            response.PageSize = listModel.PageSize;
            response.TotalRecords = listModel.TotalRecords > 0 ? listModel.TotalRecords :
              userdetails.Count();



            //return response.Data.Count > 0
            //    ? new ExecutionResult<PaginatedResponse<FollowUsers>>(response)
            //    : new ExecutionResult<PaginatedResponse<FollowUsers>>();

            return response.Data.Count > 0 ? new ExecutionResult<PaginatedResponse<FollowUsers>>(_mapper.Map<PaginatedResponse<FollowUsers>>(response)) :
               new ExecutionResult<PaginatedResponse<FollowUsers>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }


        public ExecutionResult<PaginatedResponse<FollowUsers>> GetAllExploreUsers(FollowModel listModel)
        {
            PaginatedResponse<FollowUsers> response = new PaginatedResponse<FollowUsers>();

            var Deletesugg = _dbContext.TblDeleteUsers
        .Where(o => o.UserId == listModel.ID)
        .Select(o => o.DeleteUserId);

            var Blockeduser = _dbContext.TblBlockUsers
        .Where(o => o.Id == listModel.ID)
        .Select(o => o.BlockId);

           
            var userdetails = _dbContext.Usermanagements.Where(x => x.IsActive == true && x.IsDeleted == false && !Blockeduser.Contains(x.Id) && !Deletesugg.Contains(x.Id) && (x.Firstname.Contains(listModel.Search) || x.Username.Contains(listModel.Search) || x.Phone.Contains(listModel.Search) || x.Email.Contains(listModel.Search)) && x.Id != listModel.ID && x.Id != 1).ToList();
            if (listModel.PageSize != 0 && listModel.PageNumber != 0)
            {
                var userdata = userdetails.Skip((listModel.PageNumber - 1) * listModel.PageSize)
                                                 .Take(listModel.PageSize)
                     .OrderBy(x => x.Username).ToList();
                response.Data = _mapper.Map<List<FollowUsers>>(userdata);
            }
            else
            {
                var userdata = userdetails.OrderBy(x => x.Username).ToList();
                response.Data = _mapper.Map<List<FollowUsers>>(userdata);

            }

            var follow = _dbContext.TblFollowers
          .Where(o => o.UserId == listModel.ID && o.RequestAccepted == true && o.IsDeleted == false)
          .Select(o => o.FollowerId);

            foreach (var item in response.Data)
            {
                item.mutualCount = _dbContext.TblFollowers.Where(x => x.UserId == item.Id && follow.Contains(x.UserId) && x.FollowerId == listModel.ID && x.RequestAccepted == true && x.IsDeleted == false).Count();


                var isfollow = _dbContext.TblFollowers.Where(x => x.UserId == listModel.ID && x.FollowerId == item.Id && x.IsDeleted == false).FirstOrDefault(); ;
                if (isfollow != null)
                {
                    if (isfollow.RequestAccepted == true)
                        item.isfollow = 1;
                    else
                        item.isfollow = 2;
                }
                else
                {
                    item.isfollow = 0;
                }
            }

            //response.Data =   _dbContext.Usermanagements.Where(x => x.IsActive == true)
            //     .Skip((listModel.PageNumber - 1) * listModel.PageSize)
            //                                .Take(listModel.PageSize)
            //    .OrderBy(x => x.Username).ToList();

            response.PageNumber = listModel.PageNumber;
            response.PageSize = listModel.PageSize;
            response.TotalRecords = listModel.TotalRecords > 0 ? listModel.TotalRecords :
              userdetails.Count();



            //return response.Data.Count > 0
            //    ? new ExecutionResult<PaginatedResponse<FollowUsers>>(response)
            //    : new ExecutionResult<PaginatedResponse<FollowUsers>>();

            return response.Data.Count > 0 ? new ExecutionResult<PaginatedResponse<FollowUsers>>(_mapper.Map<PaginatedResponse<FollowUsers>>(response)) :
               new ExecutionResult<PaginatedResponse<FollowUsers>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }
        public ExecutionResult<PaginatedResponse<FollowUsers>> UpdatedGetAllUsers(GetFollowModel listModel)
        {
            PaginatedResponse<FollowUsers> response = new PaginatedResponse<FollowUsers>();
            List<GetUserDistence> result = new List<GetUserDistence>();

            result = GetCache<List<GetUserDistence>>(listModel.SessionId);
            if (result == null)
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SP_UsersByNearLocation";


                    var table = new DataTable();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", listModel.ID);
                        command.Parameters.AddWithValue("@Search", listModel.Search);
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        // this will query your database and return the result to your datatable
                        DataSet ds = new DataSet();

                        da.Fill(table);


                        result = (from DataRow dr in table.Rows
                                  select new GetUserDistence()
                                  {
                                      Id = Convert.ToInt64(dr["Id"]),
                                      Location = Convert.ToDecimal(dr["Location"])
                                  }).ToList();
                        SetCache(listModel.SessionId, result);

                    }
                }
            }

            var Locationuserdata = result.Skip((listModel.PageNumber - 1) * listModel.PageSize)
                                                 .Take(listModel.PageSize)
                     .OrderBy(x => x.Location).Select(o => o.Id).ToList();
            var userdetailbyLocation = _dbContext.Usermanagements.Where(x => Locationuserdata.Contains(x.Id)).ToList();
            response.Data = _mapper.Map<List<FollowUsers>>(userdetailbyLocation);




            //    var Deletesugg = _dbContext.TblDeleteUsers
            //.Where(o => o.UserId == listModel.ID)
            //.Select(o => o.DeleteUserId);

            //    var Blockeduser = _dbContext.TblBlockUsers
            //.Where(o => o.Id == listModel.ID)
            //.Select(o => o.BlockId);

            //    var AlreadFollowed = _dbContext.TblFollowers
            //.Where(o => o.UserId == listModel.ID && o.IsDeleted == false)
            //.Select(o => o.FollowerId);
            //    var userdetails = _dbContext.Usermanagements.Where(x => x.IsActive == true && x.IsDeleted == false && !AlreadFollowed.Contains(x.Id) && !Blockeduser.Contains(x.Id) && !Deletesugg.Contains(x.Id) && (x.Firstname.Contains(listModel.Search) || x.Username.Contains(listModel.Search) || x.Phone.Contains(listModel.Search) || x.Email.Contains(listModel.Search)) && x.Id != listModel.ID).ToList();
            //    if (listModel.PageSize != 0 && listModel.PageNumber != 0)
            //    {
            //        var userdata = userdetails.Skip((listModel.PageNumber - 1) * listModel.PageSize)
            //                                         .Take(listModel.PageSize)
            //             .OrderBy(x => x.Username).ToList();
            //        response.Data = _mapper.Map<List<FollowUsers>>(userdata);                
            //    }
            //    else
            //    {
            //        var userdata = userdetails.OrderBy(x => x.Username).ToList();
            //        response.Data = _mapper.Map<List<FollowUsers>>(userdata);              

            //    }

            var follow = _dbContext.TblFollowers
          .Where(o => o.UserId == listModel.ID && o.RequestAccepted == true && o.IsDeleted == false)
          .Select(o => o.FollowerId);

            foreach (var item in response.Data)
            {
                item.mutualCount = _dbContext.TblFollowers.Where(x => x.UserId == item.Id && follow.Contains(x.UserId) && x.FollowerId == listModel.ID && x.RequestAccepted == true && x.IsDeleted == false).Count();


                var isfollow = _dbContext.TblFollowers.Where(x => x.UserId == listModel.ID && x.FollowerId == item.Id && x.IsDeleted == false).FirstOrDefault(); ;
                if (isfollow != null)
                {
                    if (isfollow.RequestAccepted == true)
                        item.isfollow = 1;
                    else
                        item.isfollow = 2;
                }
                else
                {
                    item.isfollow = 0;
                }
            }

            //response.Data =   _dbContext.Usermanagements.Where(x => x.IsActive == true)
            //     .Skip((listModel.PageNumber - 1) * listModel.PageSize)
            //                                .Take(listModel.PageSize)
            //    .OrderBy(x => x.Username).ToList();

            response.PageNumber = listModel.PageNumber;
            response.PageSize = listModel.PageSize;
            response.TotalRecords = listModel.TotalRecords > 0 ? listModel.TotalRecords :
              result.Count();



            //return response.Data.Count > 0
            //    ? new ExecutionResult<PaginatedResponse<FollowUsers>>(response)
            //    : new ExecutionResult<PaginatedResponse<FollowUsers>>();

            return response.Data.Count > 0 ? new ExecutionResult<PaginatedResponse<FollowUsers>>(_mapper.Map<PaginatedResponse<FollowUsers>>(response)) :
               new ExecutionResult<PaginatedResponse<FollowUsers>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }

        #endregion
    }
}


