using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Totem.Business.Core.AppSettings;
using Totem.Business.Core.Consts;
using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Account;
using Totem.Business.Core.DataTransferModels.Event;
using Totem.Business.Core.DataTransferModels.Post;
using Totem.Business.RepositoryInterface;
using Totem.Database.Models;
using Totem.Database.StoreProcedure;

namespace Totem.Business.Repositories
{
    public class PostRepository : IPostRepository
    {

        #region Constructor

        private readonly TotemDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAmazonS3Repository _amazonRepo;
        private readonly AWS _awsSettings;
        private readonly string _connectionString;
        private readonly INotificationRepository _NotificationRepo;

        public PostRepository(TotemDBContext dbContext, IMapper mapper, IAmazonS3Repository amazonRepo, INotificationRepository NotificationRepo)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _amazonRepo = amazonRepo;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            _NotificationRepo = NotificationRepo;
        }

        #endregion




        #region edit Post privacy from mobile
        /// <summary>
        /// Used to Post privacy Edit 
        /// </summary>
        /// <param name="editPostprivacy">Edit Post privacy details</param>
        /// <returns></returns>
        public async Task<ExecutionResult> editPostprivacy(EditPostprivacyModel EditPostprivacyModel)
        {
            if (EditPostprivacyModel == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var account = _dbContext.TblPosts.Where(x => x.Id == EditPostprivacyModel.Id && x.PostId == EditPostprivacyModel.PostId).FirstOrDefault();
            if (account == null)
            {
                // ErrorInfo errorInfo = new ErrorInfo(string.Format(MessageHelper.NoFound));
                //return new ExecutionResult(errorInfo);
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post")));
            }
            else
            {
                account.IsPrivate = EditPostprivacyModel.IsPrivate;
                await _dbContext.SaveChangesAsync();
            }


            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Post", "privacy", "Changed")));
        }

        #endregion


        #region edit Memorie privacy from mobile
        /// <summary>
        /// Used to Memorie privacy Edit 
        /// </summary>
        /// <param name="editMemorieprivacy">Edit Memorie privacy details</param>
        /// <returns></returns>
        public async Task<ExecutionResult> editMemorieprivacy(EditMemorieprivacyModel EditMemorieprivacyModel)
        {
            if (EditMemorieprivacyModel == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var account = _dbContext.TblMemories.Where(x => x.Id == EditMemorieprivacyModel.Id && x.MemorieId == EditMemorieprivacyModel.MemorieId).FirstOrDefault();
            if (account == null)
            {
                // ErrorInfo errorInfo = new ErrorInfo(string.Format(MessageHelper.NoFound));
                //return new ExecutionResult(errorInfo);
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "Memorie")));
            }
            else
            {
                account.IsPrivate = EditMemorieprivacyModel.IsPrivate;
                await _dbContext.SaveChangesAsync();
            }


            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Memorie", "privacy", "Changed")));
        }

        #endregion


        #region edit Memorie privacy from mobile
        /// <summary>
        /// Used to Memorie privacy Edit 
        /// </summary>
        /// <param name="editMemorieprivacy">Edit Memorie privacy details</param>
        /// <returns></returns>
        public async Task<ExecutionResult> editMemorieFileprivacy(EditMemorieFileprivacyModel EditMemorieFileprivacyModel)
        {
            if (EditMemorieFileprivacyModel == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var account = _dbContext.TblMemorieFiles.Where(x => x.MemorieFileId == EditMemorieFileprivacyModel.MemorieFileId).FirstOrDefault();
            if (account == null)
            {
                // ErrorInfo errorInfo = new ErrorInfo(string.Format(MessageHelper.NoFound));
                //return new ExecutionResult(errorInfo);
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "Memorie File")));
            }
            else
            {
                account.IsPrivate = EditMemorieFileprivacyModel.IsPrivate;
                await _dbContext.SaveChangesAsync();
            }


            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Memorie File", "privacy", "Changed")));
        }

        #endregion



        /// <summary>
        /// Used to create post 
        /// </summary>
        /// <param name="createPost">Post Add/Edit details</param>
        /// <returns>New/Existing post Id</returns>
        /// 
        public async Task<ExecutionResult<TblPostModel>> UpdatedCreatePost(TblPostModel tblPostModel)
        {
            if (tblPostModel == null)
            {
                return new ExecutionResult<TblPostModel>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            if (tblPostModel.PostId == 0)
            {
                TblPost tblPost = new TblPost();
                tblPost.EventId = tblPostModel.EventId;
                tblPost.Caption = tblPostModel.Caption;
                tblPost.CreatedDate = DateTime.UtcNow;
                tblPost.Id = tblPostModel.Id;
                await _dbContext.TblPosts.AddAsync(tblPost);
                await _dbContext.SaveChangesAsync();
                tblPostModel.PostId = tblPost.PostId;
                tblPostModel.EventId = tblPost.EventId == null ? 0 : tblPost.EventId;
            }
            else
            {
                var getPost = _dbContext.TblPosts
                .FirstOrDefault(x => x.PostId == tblPostModel.PostId);

                if (getPost != null && getPost.PostId > 0)
                {
                    getPost.Caption = tblPostModel.Caption;
                    getPost.EventId = tblPostModel.EventId;
                    getPost.ModifiedDate = DateTime.UtcNow;
                    _dbContext.TblPosts.Update(getPost);
                    await _dbContext.SaveChangesAsync();
                    tblPostModel.EventId = getPost.EventId == null ? 0 : getPost.EventId;
                }
            }

            var gettblTagPost = _dbContext.TblTagPosts
                .Where(x => x.PostId == tblPostModel.PostId).ToList();
            foreach (var item in gettblTagPost)
            {
                _dbContext.TblTagPosts.Remove(item);
                _dbContext.SaveChanges();
            }
            SendNotification sendNotification = new SendNotification();
            foreach (var item in tblPostModel.TagUserID)
            {
                if (item != 0)
                {
                    TblTagPost tblTagPost = new TblTagPost();
                    tblTagPost.PostId = tblPostModel.PostId;
                    tblTagPost.TagUserId = item;
                    tblTagPost.CreatedDate = DateTime.UtcNow;
                    tblTagPost.ModifiedDate = DateTime.UtcNow;
                    await _dbContext.TblTagPosts.AddAsync(tblTagPost);
                    await _dbContext.SaveChangesAsync();
                }

            }

            foreach (var item in tblPostModel.postFiles)
            {
                TblPostFile file = new TblPostFile();
                file.Videolink = item.Video;//string.Format(_awsSettings.BaseUrl, VideoName);
                file.PostId = tblPostModel.PostId;
                file.Downloadlink = item.FileName;//string.Format(_awsSettings.BaseUrl, fileName);
                if (item.MediaType != null)
                {
                    file.MediaType = item.MediaType.ToLower();
                }
                else
                {
                    file.MediaType = "";
                }
                await _dbContext.TblPostFiles.AddAsync(file);
                await _dbContext.SaveChangesAsync();
                item.PostId = file.PostId;
                item.PostFileId = file.PostFileId;

            }


            int Prop = await Task.Run(() => sendPostNotification(tblPostModel, tblPostModel.TagUserID));




            return tblPostModel.PostId > 0 ? new ExecutionResult<TblPostModel>(_mapper.Map<TblPostModel>(tblPostModel)) :
               new ExecutionResult<TblPostModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post")));
        }

        public async Task<ExecutionResult<PaginatedResponse<spExplorepostfilesModel>>> GetExplorePost(PaginationFileIdModel PaginationFileIdModel)
        {
            PaginatedResponse<spExplorepostfilesModel> result = new PaginatedResponse<spExplorepostfilesModel>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "[Sp_ExplorePostview]";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", PaginationFileIdModel.Id);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable
                    DataSet ds = new DataSet();

                    da.Fill(table);

                    var responce = Constants.ConvertDataTable<spExplorepostfilesModel>(table).ToList();
                    result.Data = responce.Skip((PaginationFileIdModel.PageNumber - 1) * PaginationFileIdModel.PageSize)
                                                .Take(PaginationFileIdModel.PageSize).ToList();

                    foreach (var item in result.Data)
                    {
                        var SelfLik = _dbContext.TblPostFileLikes.Where(a => a.PostFileId == item.PostFileId && a.Id == item.Id).FirstOrDefault();
                        if (SelfLik != null)
                            item.LikeType = SelfLik.LikeType;
                    }

                    result.PageNumber = PaginationFileIdModel.PageNumber;
                    result.PageSize = PaginationFileIdModel.PageSize;
                    result.TotalRecords = PaginationFileIdModel.TotalRecords > 0 ? PaginationFileIdModel.TotalRecords :
                        responce.Count();
                    //try
                    //{

                    //    result.Data = (from DataRow dr in table.Rows
                    //              select new spExplorepostfilesModel()
                    //              {

                    //                  PostFileId      = Convert.ToInt64(dr["PostFileId"]),
                    //                  LikeCount       = Convert.ToInt32(dr["LikeCount"]),
                    //                  Downloadlink    = dr["Downloadlink"].ToString(), 
                    //                  MediaType       = dr["MediaType"].ToString(),
                    //                  CreatedDate     = Convert.ToDateTime(dr["CreatedDate"]),
                    //                  Videolink       = dr["Videolink"].ToString(),
                    //                  PostId          = Convert.ToInt64(dr["PostId"]),
                    //                  Caption         = dr["Caption"].ToString(),
                    //                  EventId         = Convert.ToInt64(dr["EventId"]),
                    //                  Id              = Convert.ToInt64(dr["Id"]),
                    //                  IsActive        = Convert.ToInt32(dr["IsActive"]),
                    //                  IsPrivate       = Convert.ToBoolean(dr["IsPrivate"]),
                    //                  IsDeleted       = Convert.ToBoolean(dr["IsDeleted"]),
                    //                  Firstname       = dr["Firstname"].ToString(),
                    //                  Lastname        = dr["Lastname"].ToString(),
                    //                  Username = dr["Username"].ToString()

                    //              }).ToList();


                    //}
                    //catch (Exception ex)
                    //{

                    //    throw;
                    //}


                }
            }

            return result != null ? new ExecutionResult<PaginatedResponse<spExplorepostfilesModel>>(_mapper.Map<PaginatedResponse<spExplorepostfilesModel>>(result)) :
               new ExecutionResult<PaginatedResponse<spExplorepostfilesModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post Files")));



        }
        public async Task<ExecutionResult<TblPostModel>> CreatePost(TblPostModel tblPostModel)
        {
            if (tblPostModel == null)
            {
                return new ExecutionResult<TblPostModel>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            if (tblPostModel.PostId == 0)
            {
                TblPost tblPost = new TblPost();
                tblPost.EventId = tblPostModel.EventId;
                tblPost.Caption = tblPostModel.Caption;
                tblPost.CreatedDate = DateTime.UtcNow;
                tblPost.Id = tblPostModel.Id;
                await _dbContext.TblPosts.AddAsync(tblPost);
                await _dbContext.SaveChangesAsync();
                tblPostModel.PostId = tblPost.PostId;
                tblPostModel.EventId = tblPost.EventId == null ? 0 : tblPost.EventId;
            }
            else
            {
                var getPost = _dbContext.TblPosts
                .FirstOrDefault(x => x.PostId == tblPostModel.PostId);

                if (getPost != null && getPost.PostId > 0)
                {
                    getPost.Caption = tblPostModel.Caption;
                    getPost.EventId = tblPostModel.EventId;
                    getPost.ModifiedDate = DateTime.UtcNow;
                    _dbContext.TblPosts.Update(getPost);
                    await _dbContext.SaveChangesAsync();
                    tblPostModel.EventId = getPost.EventId == null ? 0 : getPost.EventId;
                }
            }

            var gettblTagPost = _dbContext.TblTagPosts
                .Where(x => x.PostId == tblPostModel.PostId).ToList();
            foreach (var item in gettblTagPost)
            {
                _dbContext.TblTagPosts.Remove(item);
                _dbContext.SaveChanges();
            }
            SendNotification sendNotification = new SendNotification();
            foreach (var item in tblPostModel.TagUserID)
            {
                if (item != 0)
                {
                    TblTagPost tblTagPost = new TblTagPost();
                    tblTagPost.PostId = tblPostModel.PostId;
                    tblTagPost.TagUserId = item;
                    tblTagPost.CreatedDate = DateTime.UtcNow;
                    tblTagPost.ModifiedDate = DateTime.UtcNow;
                    await _dbContext.TblTagPosts.AddAsync(tblTagPost);
                    await _dbContext.SaveChangesAsync();
                }

            }
            int Prop = await Task.Run(() => sendPostNotification(tblPostModel, tblPostModel.TagUserID));




            return tblPostModel.PostId > 0 ? new ExecutionResult<TblPostModel>(_mapper.Map<TblPostModel>(tblPostModel)) :
               new ExecutionResult<TblPostModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post")));
        }

        public async Task<ExecutionResult<TblMemorieModel>> CreateMemorie(TblMemorieModel tblMemorieModel)
        {
            if (tblMemorieModel == null)
            {
                return new ExecutionResult<TblMemorieModel>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            if (tblMemorieModel.MemorieId == 0)
            {
                TblMemorie tblMemorie = new TblMemorie();
                tblMemorie.EventId = tblMemorieModel.EventId;
                tblMemorie.Caption = tblMemorieModel.Caption;
                tblMemorie.CreatedDate = DateTime.UtcNow;
                tblMemorie.Id = tblMemorieModel.Id;
                await _dbContext.TblMemories.AddAsync(tblMemorie);
                await _dbContext.SaveChangesAsync();
                tblMemorieModel.MemorieId = tblMemorie.MemorieId;
                tblMemorieModel.EventId = tblMemorie.EventId == null ? 0 : tblMemorie.EventId;
            }
            else
            {
                var getMemorie = _dbContext.TblMemories
                .FirstOrDefault(x => x.MemorieId == tblMemorieModel.MemorieId);

                if (getMemorie != null && getMemorie.MemorieId > 0)
                {
                    getMemorie.Caption = "";
                    getMemorie.EventId = tblMemorieModel.EventId;
                    getMemorie.ModifiedDate = DateTime.UtcNow;
                    getMemorie.Caption = tblMemorieModel.Caption;
                    _dbContext.TblMemories.Update(getMemorie);
                    await _dbContext.SaveChangesAsync();
                    tblMemorieModel.EventId = getMemorie.EventId == null ? 0 : getMemorie.EventId;
                }
            }


            return tblMemorieModel.MemorieId > 0 ? new ExecutionResult<TblMemorieModel>(_mapper.Map<TblMemorieModel>(tblMemorieModel)) :
               new ExecutionResult<TblMemorieModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Memorie")));
        }


        public async Task<ExecutionResult<TblMemorieModel>> CreateMemorieWithFiles(TblMemorieModel tblMemorieModel)
        {
            if (tblMemorieModel == null)
            {
                return new ExecutionResult<TblMemorieModel>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            if (tblMemorieModel.MemorieId == 0)
            {
                TblMemorie tblMemorie = new TblMemorie();
                tblMemorie.EventId = tblMemorieModel.EventId;
                tblMemorie.Caption = tblMemorieModel.Caption;
                tblMemorie.CreatedDate = DateTime.UtcNow;
                tblMemorie.Id = tblMemorieModel.Id;
                await _dbContext.TblMemories.AddAsync(tblMemorie);
                await _dbContext.SaveChangesAsync();
                tblMemorieModel.MemorieId = tblMemorie.MemorieId;
                tblMemorieModel.EventId = tblMemorie.EventId == null ? 0 : tblMemorie.EventId;

                foreach (var item in tblMemorieModel.MemorieFiles)
                {
                    TblMemorieFile file = new TblMemorieFile();
                    file.Videolink = item.Video;
                    file.MemorieId = tblMemorieModel.MemorieId;
                    file.Downloadlink = item.FileName;
                    if (item.MediaType != null)
                    {
                        file.MediaType = item.MediaType.ToLower();
                    }
                    else
                    {
                        file.MediaType = "";
                    }
                    await _dbContext.TblMemorieFiles.AddAsync(file);
                    await _dbContext.SaveChangesAsync();
                }

            }
            else
            {
                var getMemorie = _dbContext.TblMemories
                .FirstOrDefault(x => x.MemorieId == tblMemorieModel.MemorieId);

                if (getMemorie != null && getMemorie.MemorieId > 0)
                {
                    getMemorie.Caption = "";
                    getMemorie.EventId = tblMemorieModel.EventId;
                    getMemorie.ModifiedDate = DateTime.UtcNow;
                    getMemorie.Caption = tblMemorieModel.Caption;
                    _dbContext.TblMemories.Update(getMemorie);
                    await _dbContext.SaveChangesAsync();
                    tblMemorieModel.EventId = getMemorie.EventId == null ? 0 : getMemorie.EventId;
                }

                foreach (var item in tblMemorieModel.MemorieFiles)
                {
                    TblMemorieFile file = new TblMemorieFile();
                    file.Videolink = item.Video;
                    file.MemorieId = tblMemorieModel.MemorieId;
                    file.Downloadlink = item.FileName;
                    if (item.MediaType != null)
                    {
                        file.MediaType = item.MediaType.ToLower();
                    }
                    else
                    {
                        file.MediaType = "";
                    }
                    await _dbContext.TblMemorieFiles.AddAsync(file);
                    await _dbContext.SaveChangesAsync();
                }
            }
            var memoryfiles = _dbContext.TblMemorieFiles.Where(x => x.MemorieId == tblMemorieModel.MemorieId).ToList();
            tblMemorieModel.MemorieFiles =
                (from xx in memoryfiles
                 select new CreateMemorieFiles
                 {
                     MemorieFileId = xx.MemorieFileId,
                     MemorieId = xx.MemorieId,
                     FileName = xx.Downloadlink,
                     MediaType = xx.MediaType,
                     Video = xx.Videolink
                 }).ToList();

                                   
               

            return tblMemorieModel.MemorieId > 0 ? new ExecutionResult<TblMemorieModel>(_mapper.Map<TblMemorieModel>(tblMemorieModel)) :
               new ExecutionResult<TblMemorieModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Memorie")));
        }


        async Task<int> sendPostNotification(TblPostModel TblPost, List<long> listusers)
        {
            var appuser = _dbContext.Usermanagements.Where(x => x.Id == TblPost.Id).FirstOrDefault();
            var fcmdetails = _dbContext.TblUserFcms.Where(x => listusers.Contains(x.Id) && x.Login == true).ToList();
            SendNotification sendNotification = new SendNotification();
            sendNotification.data = new data();
            sendNotification.notification = new notification();
            sendNotification.registration_ids = new List<string>();
            sendNotification.notification.title = "Tagged in post";
            sendNotification.notification.body = appuser.Firstname + " " + appuser.Lastname + " has tagged you in new post.";
            sendNotification.notification.image = appuser.Image;
            sendNotification.data.conversationId = TblPost.Id;
            sendNotification.data.conversationInfo = "";
            sendNotification.data.type = 2;
            foreach (var item1 in fcmdetails)
            {

                sendNotification.registration_ids.Add(item1.Fcm);

            }
            await _NotificationRepo.NotifyAsync(sendNotification);

            foreach (var item in listusers)
            {

                TblNotification tblNotification = new TblNotification();
                tblNotification.Ssrno = 0;
                tblNotification.Date = DateTime.UtcNow;
                tblNotification.Id = item;
                tblNotification.Image = appuser.Image;
                tblNotification.Readflag = "0";
                tblNotification.Title = "Tagged in post";
                tblNotification.NotificationType = "POST";
                tblNotification.NotificationTypeId = TblPost.PostId;
                tblNotification.NuserName = appuser.Firstname + " " + appuser.Lastname;
                tblNotification.Descp = " has tagged you in new post.";
                await _dbContext.TblNotifications.AddAsync(tblNotification);
                await _dbContext.SaveChangesAsync();
            }

            return 1;
            //var  Prop = await Task.Run(() => GetSomething());
        }
        public async Task<ExecutionResult> PostTagRequestAccept(AcceptTagPostModel AcceptTagPostModel)
        {
            var gettblTagPost = _dbContext.TblTagPosts
                .Where(x => x.PostId == AcceptTagPostModel.PostId && x.TagUserId == AcceptTagPostModel.Id).ToList();
            if (gettblTagPost != null)
            {
                foreach (var item in gettblTagPost)
                {
                    if (AcceptTagPostModel.Status == true)
                        item.RequestAccepted = 1;
                    else
                        item.RequestAccepted = 2;
                }
                await _dbContext.SaveChangesAsync();
                var updateNotification = _dbContext.TblNotifications
                 .Where(x => x.NotificationTypeId == AcceptTagPostModel.PostId && x.NotificationType == "POST" && x.Id == AcceptTagPostModel.Id).ToList();

                foreach (var item in updateNotification)
                {
                    item.Readflag = "3";
                }
                await _dbContext.SaveChangesAsync();
            }

            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Request", "Accepted", "")));

        }



        public async Task<ExecutionResult<List<FollowUsers>>> GetTagUserofPost(GetByPostID GetByPostID)
        {
            List<FollowUsers> response = new List<FollowUsers>();
            List<long> usersId = _dbContext.TblTagPosts.Where(x => x.PostId == GetByPostID.PostId).Select(x => x.TagUserId).ToList();
            var AllUsers = _dbContext.Usermanagements.Where(x => usersId.Contains(x.Id) && x.IsActive == true && x.IsDeleted == false)
             .OrderBy(x => x.Username).ToList();
            response = _mapper.Map<List<FollowUsers>>(AllUsers);


            return response.Count > 0 ? new ExecutionResult<List<FollowUsers>>(response) :
               new ExecutionResult<List<FollowUsers>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }


        public async Task<ExecutionResult> RemoveTagofPost(RemovePostModel RemovePostModel)
        {
            List<FollowUsers> response = new List<FollowUsers>();
            var usersId = _dbContext.TblTagPosts.Where(x => x.PostId == RemovePostModel.PostId && x.TagUserId == RemovePostModel.Id).ToList();
            foreach (var item in usersId)
            {
                _dbContext.TblTagPosts.Remove(item);
                await _dbContext.SaveChangesAsync();
            }
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Tag", "Removed", "")));
            //return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Removed Tag")));
        }

        /// <summary>
        /// Report / Block post 
        /// </summary>
        /// <param name="BlockPostModel">Post Block/Report</param>
        /// <returns>BlockPostModel</returns>
        public async Task<ExecutionResult<BlockPostResponce>> BlockPost(BlockPostModel BlockPostModel)
        {
            if (BlockPostModel == null)
            {
                return new ExecutionResult<BlockPostResponce>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            bool isblockedPost = _dbContext.TblBlockPosts.Where(x => x.PostId == BlockPostModel.PostId && x.Id == BlockPostModel.Id).Any();

            TblBlockPost TblBlockPost = new TblBlockPost();
            TblBlockPost.Id = BlockPostModel.Id;
            TblBlockPost.PostId = BlockPostModel.PostId;
            TblBlockPost.PostuserId = BlockPostModel.PostuserId;
            TblBlockPost.Reason = BlockPostModel.Reason;
            TblBlockPost.CreatedDate = DateTime.UtcNow;

            await _dbContext.TblBlockPosts.AddAsync(TblBlockPost);
            await _dbContext.SaveChangesAsync();
            var PostResponce = _mapper.Map<BlockPostResponce>(TblBlockPost);
            PostResponce.PostBlockCount = _dbContext.TblBlockPosts.Where(x => x.PostId == BlockPostModel.PostId).Count();
            if (PostResponce.PostBlockCount > 2)
            {
                var ByPostBlocked = _dbContext.Usermanagements.Where(x => x.Id == BlockPostModel.PostuserId).FirstOrDefault();
                if (ByPostBlocked != null)
                {
                    ByPostBlocked.ByPostBlocked = true;
                }
            }
            return TblBlockPost.BlockId > 0 ? new ExecutionResult<BlockPostResponce>(PostResponce) :
           new ExecutionResult<BlockPostResponce>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post")));



        }


        /// <summary>
        /// Used to get all Users of post liked 
        /// </summary>
        /// <param name="GetPostLikeUsers">unique indentification number</param>
        /// <returns>Users</returns>
        public async Task<ExecutionResult<PaginatedResponse<PostLikedUsers>>> GetpostLikesUsers(GetPostLikeUsers GetPostLikeUsers)
        {
            PaginatedResponse<PostLikedUsers> response = new PaginatedResponse<PostLikedUsers>();

            var postLikesuser = _dbContext.TblPostLikes.Where(x => x.PostId == GetPostLikeUsers.PostId);

            var postLikesuserId = postLikesuser.Select(o => o.Id);

            var AllUsers = _dbContext.Usermanagements.Where(x => postLikesuserId.Contains(x.Id) && x.Firstname.StartsWith(GetPostLikeUsers.Search))
                .Skip((GetPostLikeUsers.PageNumber - 1) * GetPostLikeUsers.PageSize)
                                                 .Take(GetPostLikeUsers.PageSize)
                                                     .OrderBy(x => x.Username).ToList();
            response.Data = _mapper.Map<List<PostLikedUsers>>(AllUsers);



            response.PageNumber = GetPostLikeUsers.PageNumber;
            response.PageSize = GetPostLikeUsers.PageSize;
            response.TotalRecords = GetPostLikeUsers.TotalRecords > 0 ? GetPostLikeUsers.TotalRecords :
                _dbContext.Usermanagements.Where(x => postLikesuserId.Contains(x.Id) && x.Firstname.StartsWith(GetPostLikeUsers.Search)).Count();

            foreach (var item in response.Data)
            {
                item.mutualCount = _dbContext.TblFollowers.Where(x => x.UserId == item.Id && x.FollowerId == GetPostLikeUsers.Id && x.RequestAccepted == true && x.IsDeleted == false).Count();
                item.SelfLiked = postLikesuser
                    .Where(o => o.Id == item.Id)
                    .Select(o => o.LikeType).FirstOrDefault();
            }


            return response.Data.Count > 0 ? new ExecutionResult<PaginatedResponse<PostLikedUsers>>(response) :
               new ExecutionResult<PaginatedResponse<PostLikedUsers>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }
        /// <summary>
        /// Used to get all the posts added by to wom your are following 
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User posts</returns>
        public async Task<ExecutionResult<PaginatedResponse<PostDetailsModel>>> GetUserPostFeeds(GetPostmodel GetPostmodel)
        {


            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "sp_GetPostDetails";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", GetPostmodel.Id);
                    command.Parameters.AddWithValue("@EventId", GetPostmodel.EventId);
                    command.Parameters.AddWithValue("@Search", GetPostmodel.Search);
                    command.Parameters.AddWithValue("@GetSelf", GetPostmodel.GetSelf);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable by filter only the post of which you are following
                    DataSet ds = new DataSet();
                    da.Fill(table);

                    PaginatedResponse<PostDetailsModel> result = new PaginatedResponse<PostDetailsModel>();
                    var responce = Constants.ConvertDataTable<PostDetailsModel>(table).ToList();
                    result.Data = responce.Skip((GetPostmodel.PageNumber - 1) * GetPostmodel.PageSize)
                                                .Take(GetPostmodel.PageSize).ToList();
                    if (result.Data != null)
                    {
                        foreach (var item in result.Data)
                        {
                            var getPostsMedia = _dbContext.TblPostFiles.Where(x => x.PostId == item.PostId).ToList();
                            var postLikes = _dbContext.TblPostLikes.Where(x => x.PostId == item.PostId).ToList();
                            var postThumbs = _dbContext.TblPostThumbs.Where(x => x.PostId == item.PostId).ToList();
                            var postComments = _dbContext.TblPostComments.Where(x => x.PostId == item.PostId).ToList();
                            if (item.EventId > 0)
                            {
                                var getEventMedia = _dbContext.TblEventUserFiles.Where(x => x.EventId == item.EventId).ToList();
                                if (getEventMedia.Any())
                                {
                                    item.EventImages = _mapper.Map<List<TblEventFile>>(getEventMedia);
                                }
                            }


                            item.NoOfLikes = postLikes.Count;
                            item.NoOfThumbs = postThumbs.Count;
                            item.NoOfComments = postComments.Count;
                            if (postLikes != null)
                            {
                                var selfl = postLikes.Where(a => a.Id == GetPostmodel.Id).FirstOrDefault();
                                if (selfl != null)
                                    item.SelfLiked = selfl.LikeType;
                            }
                            item.SelfThumbed = postThumbs.Any(a => a.Id == GetPostmodel.Id);
                            item.SelfCommented = postComments.Any(a => a.Id == GetPostmodel.Id);

                            if (getPostsMedia.Any())
                            {

                                item.PostMediaLinks = _mapper.Map<List<PostMediaModel>>(getPostsMedia);


                            }


                            List<PostCommentModel> lstPostComments = new List<PostCommentModel>();

                            var allPostComments = _dbContext.TblPostComments
                                .Where(x => x.PostId == item.PostId)
                                .Skip((1 - 1) * 5).Take(5)
                                .OrderByDescending(x => x.CreatedDate).ToList();

                            if (allPostComments.Any())
                            {
                                foreach (var item1 in allPostComments)
                                {
                                    var alluser = _dbContext.Usermanagements.Where(x => x.Id == item1.Id).FirstOrDefault();
                                    lstPostComments.Add(new PostCommentModel
                                    {
                                        PostCommentId = item1.PostCommentId,
                                        PostId = item1.PostId,
                                        Id = item1.Id,
                                        Comment = item1.Comment,
                                        Firstname = alluser.Firstname,
                                        Lastname = alluser.Lastname,
                                        Image = alluser.Image,
                                        Username = alluser.Username,
                                        ProfileVerified = alluser.ProfileVerified,
                                        likeStatus = (from xx in _dbContext.TblPostCommentfeeds where xx.Id == item.Id && xx.PostCommentId == item1.PostCommentId select xx.Id).Count() > 0 ? true : false,
                                        TotalLike = (from efcl in _dbContext.TblPostCommentfeeds where efcl.PostCommentId == item1.PostCommentId select efcl.PostCommentId).Count(),
                                        ReplyBody = (from ec in _dbContext.TblPostCommentReplies
                                                     join um in _dbContext.Usermanagements on ec.Id equals um.Id
                                                     where ec.PostCommentId == item1.PostCommentId
                                                     select new UserDataforReply
                                                     {
                                                         ReplyID = ec.ReplyId,
                                                         ReplyBody = ec.ReplyBody,
                                                         UserId = um.Id,
                                                         Firstname = um.Firstname,
                                                         Lastname = um.Lastname,
                                                         Username = um.Username,
                                                         ProfileVerified = um.ProfileVerified,
                                                         Email = um.Email,
                                                         Phone = um.Phone,
                                                         Image = um.Image
                                                     }).ToList()

                                    });
                                }
                            }
                            item.PostComments = lstPostComments;


                            List<long> usersId = _dbContext.TblTagPosts.Where(x => x.PostId == item.PostId).Select(x => x.TagUserId).ToList();
                            var AllUsers = _dbContext.Usermanagements.Where(x => usersId.Contains(x.Id) && x.IsActive == true && x.IsDeleted == false)
                             .OrderBy(x => x.Username).ToList();
                            item.TagUsers = _mapper.Map<List<FollowUsers>>(AllUsers);

                        }

                    }

                    result.PageNumber = GetPostmodel.PageNumber;
                    result.PageSize = GetPostmodel.PageSize;
                    result.TotalRecords = GetPostmodel.TotalRecords > 0 ? GetPostmodel.TotalRecords :
                        responce.Count();

                    return result.Data != null ? new ExecutionResult<PaginatedResponse<PostDetailsModel>>(result) :
                new ExecutionResult<PaginatedResponse<PostDetailsModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post is not Live or ")));
                }
            }


        }

        public async Task<ExecutionResult<PaginatedResponse<PostDetailsModel>>> GetPostTopCountFeeds(GetPostmodel GetPostmodel)
        {


            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "sp_PostTopCountFeeds";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", GetPostmodel.Id);
                    command.Parameters.AddWithValue("@EventId", GetPostmodel.EventId);
                    command.Parameters.AddWithValue("@Search", GetPostmodel.Search);
                    command.Parameters.AddWithValue("@GetSelf", GetPostmodel.GetSelf);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable by filter only the post of which you are following
                    DataSet ds = new DataSet();
                    da.Fill(table);

                    PaginatedResponse<PostDetailsModel> result = new PaginatedResponse<PostDetailsModel>();
                    var responce = Constants.ConvertDataTable<PostDetailsModel>(table).ToList();
                    result.Data = responce.Skip((GetPostmodel.PageNumber - 1) * GetPostmodel.PageSize)
                                                .Take(GetPostmodel.PageSize).ToList();
                    if (result.Data != null)
                    {
                        foreach (var item in result.Data)
                        {
                            var getPostsMedia = _dbContext.TblPostFiles.Where(x => x.PostId == item.PostId).ToList();
                            var postLikes = _dbContext.TblPostLikes.Where(x => x.PostId == item.PostId).ToList();
                            var postThumbs = _dbContext.TblPostThumbs.Where(x => x.PostId == item.PostId).ToList();
                            var postComments = _dbContext.TblPostComments.Where(x => x.PostId == item.PostId).ToList();
                            if (item.EventId > 0)
                            {
                                var getEventMedia = _dbContext.TblEventUserFiles.Where(x => x.EventId == item.EventId).ToList();
                                if (getEventMedia.Any())
                                {
                                    item.EventImages = _mapper.Map<List<TblEventFile>>(getEventMedia);
                                }
                            }


                            item.NoOfLikes = postLikes.Count;
                            item.NoOfThumbs = postThumbs.Count;
                            item.NoOfComments = postComments.Count;
                            if (postLikes != null)
                            {
                                var selfl = postLikes.Where(a => a.Id == GetPostmodel.Id).FirstOrDefault();
                                if (selfl != null)
                                    item.SelfLiked = selfl.LikeType;
                            }
                            item.SelfThumbed = postThumbs.Any(a => a.Id == GetPostmodel.Id);
                            item.SelfCommented = postComments.Any(a => a.Id == GetPostmodel.Id);

                            if (getPostsMedia.Any())
                            {

                                item.PostMediaLinks = _mapper.Map<List<PostMediaModel>>(getPostsMedia);


                            }


                        }

                    }

                    result.PageNumber = GetPostmodel.PageNumber;
                    result.PageSize = GetPostmodel.PageSize;
                    result.TotalRecords = GetPostmodel.TotalRecords > 0 ? GetPostmodel.TotalRecords :
                        responce.Count();

                    return result.Data != null ? new ExecutionResult<PaginatedResponse<PostDetailsModel>>(result) :
                new ExecutionResult<PaginatedResponse<PostDetailsModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post is not Live or ")));
                }
            }


        }


        public async Task<ExecutionResult<PaginatedResponse<PostDetailsModel>>> GetALLPostTopCount(GetPostmodel GetPostmodel)
        {


            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "sp_All_PostTopCountFeeds";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", GetPostmodel.Id);
                    command.Parameters.AddWithValue("@EventId", GetPostmodel.EventId);
                    command.Parameters.AddWithValue("@Search", GetPostmodel.Search);
                    command.Parameters.AddWithValue("@GetSelf", GetPostmodel.GetSelf);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable by filter only the post of which you are following
                    DataSet ds = new DataSet();
                    da.Fill(table);

                    PaginatedResponse<PostDetailsModel> result = new PaginatedResponse<PostDetailsModel>();
                    var responce = Constants.ConvertDataTable<PostDetailsModel>(table).ToList();
                    result.Data = responce.Skip((GetPostmodel.PageNumber - 1) * GetPostmodel.PageSize)
                                                .Take(GetPostmodel.PageSize).ToList();
                    if (result.Data != null)
                    {
                        foreach (var item in result.Data)
                        {
                            var getPostsMedia = _dbContext.TblPostFiles.Where(x => x.PostId == item.PostId).ToList();
                            var postLikes = _dbContext.TblPostLikes.Where(x => x.PostId == item.PostId).ToList();
                            var postThumbs = _dbContext.TblPostThumbs.Where(x => x.PostId == item.PostId).ToList();
                            var postComments = _dbContext.TblPostComments.Where(x => x.PostId == item.PostId).ToList();
                            if (item.EventId > 0)
                            {
                                var getEventMedia = _dbContext.TblEventUserFiles.Where(x => x.EventId == item.EventId).ToList();
                                if (getEventMedia.Any())
                                {
                                    item.EventImages = _mapper.Map<List<TblEventFile>>(getEventMedia);
                                }
                            }


                            item.NoOfLikes = postLikes.Count;
                            item.NoOfThumbs = postThumbs.Count;
                            item.NoOfComments = postComments.Count;
                            if (postLikes != null)
                            {
                                var selfl = postLikes.Where(a => a.Id == GetPostmodel.Id).FirstOrDefault();
                                if (selfl != null)
                                    item.SelfLiked = selfl.LikeType;
                            }
                            item.SelfThumbed = postThumbs.Any(a => a.Id == GetPostmodel.Id);
                            item.SelfCommented = postComments.Any(a => a.Id == GetPostmodel.Id);

                            if (getPostsMedia.Any())
                            {

                                item.PostMediaLinks = _mapper.Map<List<PostMediaModel>>(getPostsMedia);


                            }


                        }

                    }

                    result.PageNumber = GetPostmodel.PageNumber;
                    result.PageSize = GetPostmodel.PageSize;
                    result.TotalRecords = GetPostmodel.TotalRecords > 0 ? GetPostmodel.TotalRecords :
                        responce.Count();

                    return result.Data != null ? new ExecutionResult<PaginatedResponse<PostDetailsModel>>(result) :
                new ExecutionResult<PaginatedResponse<PostDetailsModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post is not Live or ")));
                }
            }


        }


        /// <summary>
        /// Used to get all the posts added by to wom your are following 
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User posts</returns>
        public async Task<ExecutionResult<PostDetailsModel>> GetPostbyPostId(GetByPostID GetByPostID)
        {


            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "sp_GetPostDetailsByPostId";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PostId", GetByPostID.PostId);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable by filter only the post of which you are following
                    DataSet ds = new DataSet();

                    da.Fill(table);

                    PostDetailsModel responce = new PostDetailsModel();
                    responce = Constants.ConvertDataTable<PostDetailsModel>(table).FirstOrDefault();

                    if (responce != null)
                    {
                        var getPostsMedia = _dbContext.TblPostFiles.Where(x => x.PostId == responce.PostId).ToList();
                        var postLikes = _dbContext.TblPostLikes.Where(x => x.PostId == responce.PostId).ToList();
                        var postThumbs = _dbContext.TblPostThumbs.Where(x => x.PostId == responce.PostId).ToList();
                        var postComments = _dbContext.TblPostComments.Where(x => x.PostId == responce.PostId).ToList();
                        if (responce.EventId > 0)
                        {
                            var getEventMedia = _dbContext.TblEventUserFiles.Where(x => x.EventId == responce.EventId).ToList();
                            if (getEventMedia.Any())
                            {
                                responce.EventImages = _mapper.Map<List<TblEventFile>>(getEventMedia);
                            }
                        }


                        responce.NoOfLikes = postLikes.Count;
                        responce.NoOfThumbs = postThumbs.Count;
                        responce.NoOfComments = postComments.Count;
                        if (postLikes != null)
                        {
                            var selfl = postLikes.Where(a => a.Id == responce.Id).FirstOrDefault();
                            if (selfl != null)
                                responce.SelfLiked = selfl.LikeType;
                        }
                        responce.SelfThumbed = postThumbs.Any(a => a.Id == responce.Id);
                        responce.SelfCommented = postComments.Any(a => a.Id == responce.Id);

                        if (getPostsMedia.Any())
                        {

                            responce.PostMediaLinks = _mapper.Map<List<PostMediaModel>>(getPostsMedia);


                        }

                        List<PostCommentModel> lstPostComments = new List<PostCommentModel>();

                        var allPostComments = _dbContext.TblPostComments
                            .Where(x => x.PostId == responce.PostId)
                            .Skip((1 - 1) * 5).Take(5)
                            .OrderByDescending(x => x.CreatedDate).ToList();

                        if (allPostComments.Any())
                        {
                            foreach (var item1 in allPostComments)
                            {
                                var alluser = _dbContext.Usermanagements.Where(x => x.Id == item1.Id).FirstOrDefault();
                                lstPostComments.Add(new PostCommentModel
                                {
                                    PostCommentId = item1.PostCommentId,
                                    PostId = item1.PostId,
                                    Id = item1.Id,
                                    Comment = item1.Comment,
                                    Firstname = alluser.Firstname,
                                    Lastname = alluser.Lastname,
                                    Image = alluser.Image,
                                    Username = alluser.Username,
                                    ProfileVerified = alluser.ProfileVerified,
                                    likeStatus = (from xx in _dbContext.TblPostCommentfeeds where xx.Id == responce.Id && xx.PostCommentId == item1.PostCommentId select xx.Id).Count() > 0 ? true : false,
                                    TotalLike = (from efcl in _dbContext.TblPostCommentfeeds where efcl.PostCommentId == item1.PostCommentId select efcl.PostCommentId).Count(),
                                    ReplyBody = (from ec in _dbContext.TblPostCommentReplies
                                                 join um in _dbContext.Usermanagements on ec.Id equals um.Id
                                                 where ec.PostCommentId == item1.PostCommentId
                                                 select new UserDataforReply
                                                 {
                                                     ReplyID = ec.ReplyId,
                                                     ReplyBody = ec.ReplyBody,
                                                     UserId = um.Id,
                                                     Firstname = um.Firstname,
                                                     Lastname = um.Lastname,
                                                     Username = um.Username,
                                                     ProfileVerified = um.ProfileVerified,
                                                     Email = um.Email,
                                                     Phone = um.Phone,
                                                     Image = um.Image
                                                 }).ToList()

                                });
                            }
                        }
                        responce.PostComments = lstPostComments;


                        List<long> usersId = _dbContext.TblTagPosts.Where(x => x.PostId == responce.PostId).Select(x => x.TagUserId).ToList();
                        var AllUsers = _dbContext.Usermanagements.Where(x => usersId.Contains(x.Id) && x.IsActive == true && x.IsDeleted == false)
                         .OrderBy(x => x.Username).ToList();
                        responce.TagUsers = _mapper.Map<List<FollowUsers>>(AllUsers);
                    }


                    return responce != null ? new ExecutionResult<PostDetailsModel>(responce) :
                new ExecutionResult<PostDetailsModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post is not Live or ")));
                }
            }


        }


        /// <summary>
        /// Used to get all the posts added by user 
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User posts</returns>
        public async Task<ExecutionResult<List<UserPostsModel>>> GetUserPosts(GetPostID GetPostID)
        {
            List<UserPostsModel> lstUserPosts = new List<UserPostsModel>();

            var gettblTagPost = _dbContext.TblTagPosts
                .Where(x => x.TagUserId == GetPostID.Id && x.RequestAccepted == 1).Select(x => x.PostId).ToList();

            var getPostsByUsers = _dbContext.TblPosts
                .Where(x => (x.Id == GetPostID.Id || gettblTagPost.Contains(x.PostId)) && !x.IsDeleted && x.IsActive != 0).OrderByDescending(x => x.PostId).ToList();

            if (getPostsByUsers.Any())
            {
                foreach (var item in getPostsByUsers)
                {
                    UserPostsModel userPost = new UserPostsModel();
                    userPost.PostMediaLinks = new List<PostMediaModel>();

                    var getPostsMedia = _dbContext.TblPostFiles.Where(x => x.PostId == item.PostId).ToList();

                    userPost.PostId = item.PostId;
                    userPost.Caption = item.Caption;
                    userPost.EventId = item.EventId ?? 0;
                    userPost.Id = item.Id;
                    var userdetails = _dbContext.Usermanagements.Where(x => x.Id == item.Id).FirstOrDefault();
                    userPost.Firstname = userdetails.Firstname;
                    userPost.Lastname = userdetails.Lastname;
                    userPost.Username = userdetails.Username;
                    userPost.Image = userdetails.Image;

                    if (getPostsMedia.Any())
                    {

                        foreach (var media in getPostsMedia)
                        {
                            userPost.PostMediaLinks.Add(new PostMediaModel
                            {
                                PostFileId = media.PostFileId,
                                Downloadlink = media.Downloadlink,
                                MediaType = media.MediaType
                            });
                        }
                    }

                    lstUserPosts.Add(userPost);
                }

            }

            return lstUserPosts.Any() ? new ExecutionResult<List<UserPostsModel>>(_mapper.Map<List<UserPostsModel>>(lstUserPosts)) :
               new ExecutionResult<List<UserPostsModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "User Post")));
        }



        /// <summary>
        /// Used to get all user memories
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User memories</returns>
        public async Task<ExecutionResult<PaginatedResponse<UserMemorysDetailsModel>>> GetUserMemories(GetMemorieID GetMemorieID)
        {
            PaginatedResponse<UserMemorysDetailsModel> ResultUserMemories = new PaginatedResponse<UserMemorysDetailsModel>();

            var memoryresult = (from ec in _dbContext.TblMemories
                                join um in _dbContext.TblEvents on ec.EventId equals um.EventId
                                where ec.Id == GetMemorieID.Id && ec.IsDeleted == false && ec.IsActive != 0
                                select new UserMemorysDetailsModel
                                {
                                    MemorieId = ec.MemorieId,
                                    Caption = ec.Caption,
                                    EventId = um.EventId,
                                    EventName = um.EventName,
                                    StartDate = um.StartDate,
                                    Id = ec.Id                                   

                                }).ToList();

            var getMemoriesByUsers = memoryresult                
                .Skip((GetMemorieID.PageNumber - 1) * GetMemorieID.PageSize)
                                                 .Take(GetMemorieID.PageSize).OrderByDescending(x=>x.StartDate).ToList();

            var lstUserMemories = _mapper.Map<List<UserMemorysDetailsModel>>(getMemoriesByUsers);
            if (lstUserMemories != null)
            {
                foreach (var item in lstUserMemories)
                {                  

                    
                    var userdetails = _dbContext.Usermanagements.Where(x => x.Id == item.Id).FirstOrDefault();
                    item.Firstname = userdetails.Firstname;
                    item.Lastname = userdetails.Lastname;
                    item.Username = userdetails.Username;
                    item.Image = userdetails.Image;

                    var MemorieMediafil = _dbContext.TblMemorieFiles.Where(x => x.MemorieId == item.MemorieId).ToList();
                    item.MemorieMediaLinks = _mapper.Map<List<MemorieMediaModel>>(MemorieMediafil);
                }

            }
            ResultUserMemories.Data = lstUserMemories;
            ResultUserMemories.PageNumber = GetMemorieID.PageNumber;
            ResultUserMemories.PageSize = GetMemorieID.PageSize;
            ResultUserMemories.TotalRecords = GetMemorieID.TotalRecords > 0 ? GetMemorieID.TotalRecords :
                                   memoryresult.Count();
            return memoryresult != null ? new ExecutionResult<PaginatedResponse<UserMemorysDetailsModel>>(ResultUserMemories) :
        new ExecutionResult<PaginatedResponse<UserMemorysDetailsModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Memories")));
        }


        public async Task<ExecutionResult> DeleteMemoriesFiles(GetPostID GetMemorieID)
        {
            var MemorieMediafil = _dbContext.TblMemorieFiles.Where(x => x.MemorieFileId == GetMemorieID.Id).ToList();
            foreach (var item in MemorieMediafil)
            {
                _dbContext.TblMemorieFiles.Remove(item);
            }
            await _dbContext.SaveChangesAsync();
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "File", "deleted", "")));

        }


        /// <summary>
        /// Used to get all the details related with Post
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User Post Details</returns>
        public async Task<ExecutionResult<List<UserPostDetailsModel>>> GetUserPostDetails(GetPostID GetPostID)
        {
            List<UserPostDetailsModel> lstUserPosts = new List<UserPostDetailsModel>();

            var getPostsByUsers = _dbContext.TblPosts
                .Where(x => x.Id == GetPostID.Id && !x.IsDeleted && x.IsActive != 0).ToList();

            if (getPostsByUsers.Any())
            {
                foreach (var item in getPostsByUsers)
                {
                    UserPostDetailsModel userPost = new UserPostDetailsModel();
                    userPost.PostMediaLinks = new List<PostMediaModel>();

                    var getPostsMedia = _dbContext.TblPostFiles.Where(x => x.PostId == item.PostId).ToList();
                    var postLikes = _dbContext.TblPostLikes.Where(x => x.PostId == item.PostId).ToList();
                    var postThumbs = _dbContext.TblPostThumbs.Where(x => x.PostId == item.PostId).ToList();
                    var postComments = _dbContext.TblPostComments.Where(x => x.PostId == item.PostId).ToList();

                    userPost.PostId = item.PostId;
                    userPost.Caption = item.Caption;
                    userPost.EventId = item.EventId ?? 0;
                    userPost.NoOfLikes = postLikes.Count;
                    userPost.NoOfThumbs = postThumbs.Count;
                    userPost.NoOfComments = postComments.Count;
                    userPost.SelfLiked = postLikes.Any(a => a.Id == GetPostID.Id);
                    userPost.SelfThumbed = postThumbs.Any(a => a.Id == GetPostID.Id);
                    userPost.SelfCommented = postComments.Any(a => a.Id == GetPostID.Id);

                    if (getPostsMedia.Any())
                    {
                        foreach (var media in getPostsMedia)
                        {
                            userPost.PostMediaLinks.Add(new PostMediaModel
                            {
                                PostFileId = media.PostFileId,
                                Downloadlink = media.Downloadlink,
                                MediaType = media.MediaType
                            });
                        }
                    }

                    lstUserPosts.Add(userPost);
                }

            }

            return lstUserPosts.Any() ? new ExecutionResult<List<UserPostDetailsModel>>(_mapper.Map<List<UserPostDetailsModel>>(lstUserPosts)) :
               new ExecutionResult<List<UserPostDetailsModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event")));
        }

        /// <summary>
        /// Used to delete media file from Post
        /// </summary>
        /// <param name="removePostMedia">Details to remove post media</param>
        /// <returns>Successfully delete or not</returns>
        public async Task<ExecutionResult> RemovePost(RemovePostModel removePostModel)
        {
            if (removePostModel == null || removePostModel.PostId == 0)
            {
                return new ExecutionResult();
            }

            var postData = _dbContext.TblPosts.FirstOrDefault(x => x.PostId == removePostModel.PostId && x.Id == removePostModel.Id);
            if (postData == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "post")));
            }

            #region Remove related media

            var getAllPostMedia = _dbContext.TblPostFiles.Where(x => x.PostId == postData.PostId).ToList();

            foreach (var media in getAllPostMedia)
            {
                string fileName = "Post/" + Path.GetFileName(media.Downloadlink);

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.PassParaMessage, "filename")));
                }

                var delete = await _amazonRepo.DeleteFile(fileName);
                _dbContext.TblPostFiles.Remove(media);
                await _dbContext.SaveChangesAsync();
            }

            #endregion

            #region remove likes

            var getLikes = _dbContext.TblPostLikes.Where(x => x.PostId == postData.PostId).ToList();

            foreach (var likes in getLikes)
            {
                _dbContext.TblPostLikes.Remove(likes);
                await _dbContext.SaveChangesAsync();
            }

            #endregion

            #region remove comments

            var getComments = _dbContext.TblPostComments.Where(x => x.PostId == postData.PostId).ToList();

            foreach (var comments in getComments)
            {
                _dbContext.TblPostComments.Remove(comments);
                await _dbContext.SaveChangesAsync();
            }

            #endregion

            #region remove thumbs

            var getThumbs = _dbContext.TblPostThumbs.Where(x => x.PostId == postData.PostId).ToList();

            foreach (var thumbs in getThumbs)
            {
                _dbContext.TblPostThumbs.Remove(thumbs);
                await _dbContext.SaveChangesAsync();
            }

            #endregion

            #region remove shared

            var getShareds = _dbContext.TblPostShareds.Where(x => x.PostId == postData.PostId).ToList();

            foreach (var share in getShareds)
            {
                _dbContext.TblPostShareds.Remove(share);
                await _dbContext.SaveChangesAsync();
            }

            #endregion

            #region remove Taged User

            var getTaged = _dbContext.TblTagPosts.Where(x => x.PostId == postData.PostId).ToList();

            foreach (var Tageed in getTaged)
            {
                _dbContext.TblTagPosts.Remove(Tageed);
                await _dbContext.SaveChangesAsync();
            }

            #endregion

            _dbContext.TblPosts.Remove(postData);
            await _dbContext.SaveChangesAsync();

            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Post", "deleted", "")));
        }

        /// <summary>
        /// Used to add likes and thumbs on the post
        /// </summary>
        /// <param name="likePostStatus">Create post with given model having caption, event</param>
        /// <returns>Like/Thumb added successfully or not</returns>
        public async Task<ExecutionResult<LikeStatusModel>> LikePost(LikeThumbPostModel likePostStatus)
        {
            LikeStatusModel likeStatusModel = new LikeStatusModel();
            var getPostDetails = _dbContext.TblPosts.FirstOrDefault(x => x.PostId == likePostStatus.PostId);

            if (getPostDetails != null)
            {
                var postlike = _dbContext.TblPostLikes.Where(x => x.PostId == likePostStatus.PostId && x.Id == likePostStatus.Id).FirstOrDefault();
                if (likePostStatus.LikeStatus == true)
                {

                    if (postlike != null)
                    {
                        postlike.LikeType = likePostStatus.LikeType;
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        TblPostLike tblPostLikes = new TblPostLike();
                        tblPostLikes.PostId = likePostStatus.PostId;
                        tblPostLikes.Id = likePostStatus.Id;
                        tblPostLikes.LikeType = likePostStatus.LikeType;
                        await _dbContext.TblPostLikes.AddAsync(tblPostLikes);
                        await _dbContext.SaveChangesAsync();

                        int Prop = await Task.Run(() => LikePostNotification(tblPostLikes));
                    }

                }
                else
                {
                    if (postlike != null)
                    {
                        _dbContext.Remove(postlike);
                        await _dbContext.SaveChangesAsync();
                    }
                    //TblPostThumb tblPostThum = new TblPostThumb();
                    //tblPostThum.PostId = likePostStatus.PostId;
                    //tblPostThum.Id = likePostStatus.Id;                    
                    //await _dbContext.TblPostThumbs.AddAsync(tblPostThum);
                    //await _dbContext.SaveChangesAsync();
                }



                var postLikes = _dbContext.TblPostLikes.Where(x => x.PostId == getPostDetails.PostId).ToList();
                likeStatusModel.PostId = getPostDetails.PostId;
                likeStatusModel.TotalLikes = postLikes.Count;
                var SelfLik = postLikes.Where(a => a.Id == likePostStatus.Id).FirstOrDefault();
                if (SelfLik != null)
                    likeStatusModel.SelfLiked = SelfLik.LikeType;


                return getPostDetails.PostId > 0 ? new ExecutionResult<LikeStatusModel>(_mapper.Map<LikeStatusModel>(likeStatusModel)) :
              new ExecutionResult<LikeStatusModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post")));
            }

            return new ExecutionResult<LikeStatusModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post")));
        }

        public async Task<ExecutionResult<FileLikeStatusModel>> LikePostFiles(LikeThumbPostFilesModel likePostStatus)
        {
            FileLikeStatusModel likeStatusModel = new FileLikeStatusModel();
            var getPostDetails = _dbContext.TblPosts.FirstOrDefault(x => x.PostId == likePostStatus.PostId);

            if (getPostDetails != null)
            {
                var postfilelike = _dbContext.TblPostFileLikes.Where(x => x.PostFileId == likePostStatus.PostFileId && x.Id == likePostStatus.Id).FirstOrDefault();
                if (likePostStatus.LikeStatus == true)
                {

                    if (postfilelike != null)
                    {
                        postfilelike.LikeType = likePostStatus.LikeType;
                        await _dbContext.SaveChangesAsync();   

                    }
                    else
                    {
                        TblPostFileLike TblPostFileLikes = new TblPostFileLike();
                        TblPostFileLikes.PostId = likePostStatus.PostId;
                        TblPostFileLikes.PostFileId = likePostStatus.PostFileId;
                        TblPostFileLikes.Id = likePostStatus.Id;
                        TblPostFileLikes.LikeType = likePostStatus.LikeType;
                        await _dbContext.TblPostFileLikes.AddAsync(TblPostFileLikes);
                        await _dbContext.SaveChangesAsync();

                        //int Prop = await Task.Run(() => LikePostNotification(TblPostFileLikes));
                    }

                }
                else
                {
                    if (postfilelike != null)
                    {
                        _dbContext.Remove(postfilelike);
                        await _dbContext.SaveChangesAsync();
                    }
                    //TblPostThumb tblPostThum = new TblPostThumb();
                    //tblPostThum.PostId = likePostStatus.PostId;
                    //tblPostThum.Id = likePostStatus.Id;                    
                    //await _dbContext.TblPostThumbs.AddAsync(tblPostThum);
                    //await _dbContext.SaveChangesAsync();
                }



                var postLikes = _dbContext.TblPostFileLikes.Where(x => x.PostFileId == likePostStatus.PostFileId).ToList();
                likeStatusModel.PostId = getPostDetails.PostId;
                likeStatusModel.PostFileId = likePostStatus.PostFileId;
                likeStatusModel.TotalLikes = postLikes.Count;
                var SelfLik = postLikes.Where(a => a.PostFileId == likePostStatus.PostFileId && a.Id == likePostStatus.Id).FirstOrDefault();
                if (SelfLik != null)
                    likeStatusModel.SelfLiked = SelfLik.LikeType;

         
                return getPostDetails.PostId > 0 ? new ExecutionResult<FileLikeStatusModel>(_mapper.Map<FileLikeStatusModel>(likeStatusModel)) :
              new ExecutionResult<FileLikeStatusModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post File")));
            }

            return new ExecutionResult<FileLikeStatusModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post File")));
        }

        //async Task<string> Liketype(int status)
        //{
        //    if (status == 1 )
        //    {
        //        return "Like";
        //    }
        //    if (status == 2)
        //    {
        //        return "Fire";
        //    }
        //    if (status == 3)
        //    {
        //        return "Mind Blown";
        //    }
        //    if (status == 4)
        //    {
        //        return "Love";
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        async Task<int> LikePostNotification(TblPostLike TblPostLike)
        {


            var postdetails = _dbContext.TblPosts.Where(x => x.PostId == TblPostLike.PostId).FirstOrDefault();
            var userdetails = _dbContext.Usermanagements.Where(x => x.Id == TblPostLike.Id).FirstOrDefault();
            var fcmdetails = _dbContext.TblUserFcms.Where(x => x.Id == postdetails.Id && x.Login == true).ToList();
            SendNotification sendNotification = new SendNotification();
            sendNotification.data = new data();
            sendNotification.notification = new notification();
            sendNotification.registration_ids = new List<string>();
            sendNotification.notification.title = "Reacted on post";
            if (TblPostLike.LikeType == 1)
            {
                sendNotification.notification.body = userdetails.Firstname + " " + userdetails.Lastname + " has Reacted Like on your post.";
            }
            else if (TblPostLike.LikeType == 2)
            {
                sendNotification.notification.body = userdetails.Firstname + " " + userdetails.Lastname + " has Reacted Fire on your post.";

            }
            else if (TblPostLike.LikeType == 3)
            {
                sendNotification.notification.body = userdetails.Firstname + " " + userdetails.Lastname + " has Reacted Mind Blown on your post.";

            }
            else if (TblPostLike.LikeType == 4)
            {
                sendNotification.notification.body = userdetails.Firstname + " " + userdetails.Lastname + " has Reacted Love on your post.";

            }
            else
            {
                sendNotification.notification.body = userdetails.Firstname + " " + userdetails.Lastname + " has Reacted on your post.";
            }

            sendNotification.notification.image = userdetails.Image;
            sendNotification.data.conversationId = postdetails.Id;
            sendNotification.data.conversationInfo = "";
            sendNotification.data.type = 2;
            foreach (var item1 in fcmdetails)
            {

                sendNotification.registration_ids.Add(item1.Fcm);

            }
            await _NotificationRepo.NotifyAsync(sendNotification);

            TblNotification tblNotification = new TblNotification();
            tblNotification.Ssrno = 0;
            tblNotification.Date = DateTime.UtcNow;
            tblNotification.Id = postdetails.Id;
            tblNotification.Image = userdetails.Image;
            tblNotification.Readflag = "0";
            tblNotification.Title = "Reacted on post";
            tblNotification.NotificationType = "LIKEPOST";
            tblNotification.NotificationTypeId = postdetails.PostId;
            tblNotification.NuserName = userdetails.Firstname + " " + userdetails.Lastname;

            if (TblPostLike.LikeType == 1)
            {
                tblNotification.Descp = " has Reacted Like on your post.";
            }
            else if (TblPostLike.LikeType == 2)
            {
                tblNotification.Descp = " has Reacted Fire on your post.";

            }
            else if (TblPostLike.LikeType == 3)
            {
                tblNotification.Descp = " has Reacted Mind Blown on your post.";

            }
            else if (TblPostLike.LikeType == 4)
            {
                tblNotification.Descp = " has Reacted Love on your post.";

            }
            else
            {
                tblNotification.Descp = " has Reacted on your post.";
            }

            await _dbContext.TblNotifications.AddAsync(tblNotification);
            await _dbContext.SaveChangesAsync();


            return 1;
            //var  Prop = await Task.Run(() => GetSomething());
        }

        /// <summary>
        /// Used to Remove comments of post
        /// </summary>
        /// <param name="RemovePostCommentModel">Comment details</param>
        /// <returns>Remove comment details</returns>
        public async Task<ExecutionResult> RemoveComments(RemovePostCommentModel postComment)
        {
            #region remove comments

            var getComments = _dbContext.TblPostComments.Where(x => x.PostCommentId == postComment.PostCommentId).FirstOrDefault();

            if (getComments != null)
            {
                var Postdetails = _dbContext.TblPosts.Where(x => x.PostId == getComments.PostId).FirstOrDefault();
                if (getComments.Id == postComment.Id || Postdetails.Id == postComment.Id)
                {
                    _dbContext.TblPostComments.Remove(getComments);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.AccessRestricted, "Access denied !")));
                }
            }

            #endregion
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Comment", "Removed", "")));
        }

        /// <summary>
        /// Used to Remove comments of post
        /// </summary>
        /// <param name="RemovePostCommentModel">Comment details</param>
        /// <returns>Remove comment details</returns>
        public async Task<ExecutionResult> RemoveCommentsReply(RemovePostCommentReplyModel CommentsReply)
        {
            #region remove comments

            var getCommentsReply = _dbContext.TblPostCommentReplies.Where(x => x.ReplyId == CommentsReply.ReplyId).FirstOrDefault();

            if (getCommentsReply != null)
            {
                var getComments = _dbContext.TblPostComments.Where(x => x.PostCommentId == getCommentsReply.PostCommentId).FirstOrDefault();
                if (getComments != null)
                {
                    var Postdetails = _dbContext.TblPosts.Where(x => x.PostId == getComments.PostId).FirstOrDefault();
                    if (getCommentsReply.Id == CommentsReply.Id || Postdetails.Id == CommentsReply.Id)
                    {
                        _dbContext.TblPostCommentReplies.Remove(getCommentsReply);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.AccessRestricted, "Access denied !")));
                    }
                }

            }

            #endregion
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Reply", "Removed", "")));
        }

        /// <summary>
        /// Used to add comments to post
        /// </summary>
        /// <param name="postComment">Comment details</param>
        /// <returns>Newly added comment details</returns>
        public async Task<ExecutionResult<PostCommentModel>> AddComments(AddPostCommentModel postComment)
        {
            if (postComment == null)
            {
                return new ExecutionResult<PostCommentModel>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            //var getPost = _dbContext.TblPosts.FirstOrDefault(x => x.PostId == postComment.PostId &&
            //x.Id == postComment.Id);
            var alluser = _dbContext.Usermanagements.Where(x => x.Id == postComment.Id).FirstOrDefault();
            PostCommentModel PostCommentModelt = new PostCommentModel();
            //if (getPost != null)
            //{
            TblPostComment tblPostComments = new TblPostComment();
            tblPostComments.PostId = postComment.PostId;
            tblPostComments.Comment = postComment.Comment;
            tblPostComments.Id = postComment.Id;
            await _dbContext.TblPostComments.AddAsync(tblPostComments);
            await _dbContext.SaveChangesAsync();
            postComment.PostCommentId = tblPostComments.PostCommentId;

            int Prop = await Task.Run(() => PostCommentNotification(tblPostComments));

            PostCommentModelt = _mapper.Map<PostCommentModel>(tblPostComments);
            PostCommentModelt.Firstname = alluser.Firstname;
            PostCommentModelt.Lastname = alluser.Lastname;
            PostCommentModelt.ProfileVerified = alluser.ProfileVerified;
            PostCommentModelt.Username = alluser.Username;
            PostCommentModelt.Image = alluser.Image;
            PostCommentModelt.likeStatus = (from xx in _dbContext.TblPostCommentfeeds where xx.Id == postComment.Id && xx.PostCommentId == PostCommentModelt.PostCommentId select xx.Id).Count() > 0 ? true : false;
            PostCommentModelt.TotalLike = (from efcl in _dbContext.TblPostCommentfeeds where efcl.PostCommentId == PostCommentModelt.PostCommentId select efcl.PostCommentId).Count();
            PostCommentModelt.ReplyBody = (from ec in _dbContext.TblPostCommentReplies
                                           join um in _dbContext.Usermanagements on ec.Id equals um.Id
                                           where ec.PostCommentId == PostCommentModelt.PostCommentId
                                           select new UserDataforReply
                                           {
                                               ReplyID = ec.ReplyId,
                                               ReplyBody = ec.ReplyBody,
                                               UserId = um.Id,
                                               Firstname = um.Firstname,
                                               Lastname = um.Lastname,
                                               ProfileVerified = um.ProfileVerified,
                                               Username = um.Username,
                                               Email = um.Email,
                                               Phone = um.Phone,
                                               Image = um.Image
                                           }).ToList();
            //}

            return PostCommentModelt != null ? new ExecutionResult<PostCommentModel>(PostCommentModelt) :
               new ExecutionResult<PostCommentModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post")));
        }


        async Task<int> PostCommentNotification(TblPostComment TblPostComment)
        {
            var postdetails = _dbContext.TblPosts.Where(x => x.PostId == TblPostComment.PostId).FirstOrDefault();
            var userdetails = _dbContext.Usermanagements.Where(x => x.Id == TblPostComment.Id).FirstOrDefault();
            var fcmdetails = _dbContext.TblUserFcms.Where(x => x.Id == postdetails.Id && x.Login == true).ToList();
            SendNotification sendNotification = new SendNotification();
            sendNotification.data = new data();
            sendNotification.notification = new notification();
            sendNotification.registration_ids = new List<string>();
            sendNotification.notification.title = "Commented on post";
            sendNotification.notification.body = userdetails.Firstname + " " + userdetails.Lastname + " has Commented on your post.";
            sendNotification.notification.image = userdetails.Image;
            sendNotification.data.conversationId = postdetails.Id;
            sendNotification.data.conversationInfo = "";
            sendNotification.data.type = 2;
            foreach (var item1 in fcmdetails)
            {

                sendNotification.registration_ids.Add(item1.Fcm);

            }
            await _NotificationRepo.NotifyAsync(sendNotification);

            TblNotification tblNotification = new TblNotification();
            tblNotification.Ssrno = 0;
            tblNotification.Date = DateTime.UtcNow;
            tblNotification.Id = postdetails.Id;
            tblNotification.Image = userdetails.Image;
            tblNotification.Readflag = "0";
            tblNotification.Title = "Commented on post";
            tblNotification.NotificationType = "COMMENT";
            tblNotification.NotificationTypeId = postdetails.PostId;
            tblNotification.NuserName = userdetails.Firstname + " " + userdetails.Lastname;
            tblNotification.Descp = " has Commented on your post.";
            await _dbContext.TblNotifications.AddAsync(tblNotification);
            await _dbContext.SaveChangesAsync();


            return 1;
            //var  Prop = await Task.Run(() => GetSomething());
        }

        /// <summary>
        /// Used to Insert Post Comments Reply for given post
        /// </summary>
        /// <param name="InsertPostCommentsReply">Get all the comments added on post</param>
        /// <returns>Insert Post Comments Reply added on post</returns>
        public async Task<ExecutionResult<PostCommentReply>> InsertPostCommentsReply(PostCommentReply obj)
        {
            if (obj == null)
            {
                return new ExecutionResult<PostCommentReply>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            TblPostCommentReply xx = new TblPostCommentReply();
            xx.PostCommentId = obj.PostCommentId;
            xx.Id = obj.Id;
            xx.ReplyBody = obj.ReplyBody;
            await _dbContext.TblPostCommentReplies.AddAsync(xx);
            await _dbContext.SaveChangesAsync();

            //return new ExecutionResult<UpdateCommentReply>(new InfoMessage(string.Format(MessageHelper.Message, "Event", "Reply on comments successfully", "")));

            return xx.ReplyId > 0 ? new ExecutionResult<PostCommentReply>(_mapper.Map<PostCommentReply>(xx)) :
              new ExecutionResult<PostCommentReply>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Comment Reply")));
        }

        /// <summary>
        /// Used to get comments for given post
        /// </summary>
        /// <param name="getPostComments">Get all the comments added on post</param>
        /// <returns>Comments added on post</returns>
        public async Task<ExecutionResult<List<PostCommentModel>>> GetPostComments(GetPostCommentModel getPostComments)
        {
            List<PostCommentModel> lstPostComments = new List<PostCommentModel>();

            var allPostComments = _dbContext.TblPostComments
                .Where(x => x.PostId == getPostComments.PostId && x.Comment.Contains(getPostComments.Search))
                .Skip((getPostComments.PageNumber - 1) * getPostComments.PageSize)
                                                .Take(getPostComments.PageSize)
                .OrderByDescending(x => x.CreatedDate).ToList();

            if (allPostComments.Any())
            {
                foreach (var item in allPostComments)
                {
                    var alluser = _dbContext.Usermanagements.Where(x => x.Id == item.Id).FirstOrDefault();
                    lstPostComments.Add(new PostCommentModel
                    {
                        PostCommentId = item.PostCommentId,
                        PostId = item.PostId,
                        Id = item.Id,
                        Comment = item.Comment,
                        Firstname = alluser.Firstname,
                        Lastname = alluser.Lastname,
                        Image = alluser.Image,
                        Username = alluser.Username,
                        ProfileVerified = alluser.ProfileVerified,
                        likeStatus = (from xx in _dbContext.TblPostCommentfeeds where xx.Id == getPostComments.Id && xx.PostCommentId == item.PostCommentId select xx.Id).Count() > 0 ? true : false,
                        TotalLike = (from efcl in _dbContext.TblPostCommentfeeds where efcl.PostCommentId == item.PostCommentId select efcl.PostCommentId).Count(),
                        ReplyBody = (from ec in _dbContext.TblPostCommentReplies
                                     join um in _dbContext.Usermanagements on ec.Id equals um.Id
                                     where ec.PostCommentId == item.PostCommentId
                                     select new UserDataforReply
                                     {
                                         ReplyID = ec.ReplyId,
                                         ReplyBody = ec.ReplyBody,
                                         UserId = um.Id,
                                         Firstname = um.Firstname,
                                         Lastname = um.Lastname,
                                         Username = um.Username,
                                         ProfileVerified = um.ProfileVerified,
                                         Email = um.Email,
                                         Phone = um.Phone,
                                         Image = um.Image
                                     }).ToList()

                    });
                }
            }

            return allPostComments != null ? new ExecutionResult<List<PostCommentModel>>(lstPostComments) :
               new ExecutionResult<List<PostCommentModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Post Comments")));
        }
    }
}

