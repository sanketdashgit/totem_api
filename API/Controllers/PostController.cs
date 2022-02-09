using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Account;
using Totem.Business.Core.DataTransferModels.Documents;
using Totem.Business.Core.DataTransferModels.Event;
using Totem.Business.Core.DataTransferModels.Post;
using Totem.Business.RepositoryInterface;

namespace Totem.API.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : BaseController
    {
        #region Constructor

        private readonly IPostRepository _postRepo;
        private readonly IDocumentRepository _documentRepo;
        public PostController(IPostRepository postRepo, IDocumentRepository documentRepo)
        {
            _postRepo = postRepo;
            _documentRepo = documentRepo;
        }

        #endregion

        /// <summary>
        /// Used to get all the posts added by user 
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User posts</returns>
        [HttpPost("getuserposts")]
        public async Task<IActionResult> GetUserPosts(GetPostID GetPostID)
        {
            if (GetPostID.Id > 0)
            {
                ExecutionResult<List<UserPostsModel>> RequestModel = await _postRepo.GetUserPosts(GetPostID);

                string ResponceMessage = string.Empty;
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

            return null;
        }

       

        /// <summary>
        /// Used to get all user memories
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User memories</returns>
        [HttpPost("getusermemories")]
        public async Task<IActionResult> GetUserMemories(GetMemorieID GetMemorieID)
        {
            if (GetMemorieID.Id > 0)
            {
                ExecutionResult<PaginatedResponse<UserMemorysDetailsModel>> RequestModel = await _postRepo.GetUserMemories(GetMemorieID);

                string ResponceMessage = string.Empty;
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

            return null;
        }



        /// <summary>
        /// DeleteMemoriesFiles
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>DeleteMemoriesFiles</returns>
        [HttpPost("DeleteMemoriesFiles")]
        public async Task<IActionResult> DeleteMemoriesFiles(GetPostID GetMemorieID)
        {
            if (GetMemorieID.Id > 0)
            {
                ExecutionResult RequestModel = await _postRepo.DeleteMemoriesFiles(GetMemorieID);

                string ResponceMessage = string.Empty;
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

            return null;
        }
        /// <summary>
        /// Used to get all the details related with Post
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User Post Details</returns>
        [HttpPost("getuserpostdetails")]
        public async Task<IActionResult> GetUserPostDetails(GetPostID GetPostID)
        {
            if (GetPostID.Id > 0)
            {
                ExecutionResult<List<UserPostDetailsModel>> RequestModel = await _postRepo.GetUserPostDetails(GetPostID);

                string ResponceMessage = string.Empty;
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

            return null;
        }

        /// <summary>
        /// Used to get all the posts added by user 
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User posts</returns>
        [HttpPost("GetUserPostFeeds")]
        public async Task<IActionResult> GetUserPostFeeds(GetPostmodel GetPostmodel)
        {
            if (GetPostmodel != null)
            {
                ExecutionResult<PaginatedResponse<PostDetailsModel>> RequestModel = await _postRepo.GetUserPostFeeds(GetPostmodel);

                string ResponceMessage = string.Empty;
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

            return null;
        }
        //GetALLPostTopCount

        [HttpPost("GetALLPostTopCount")]
        public async Task<IActionResult> GetALLPostTopCount(GetPostmodel GetPostmodel)
        {
            if (GetPostmodel != null)
            {
                ExecutionResult<PaginatedResponse<PostDetailsModel>> RequestModel = await _postRepo.GetALLPostTopCount(GetPostmodel);

                string ResponceMessage = string.Empty;
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

            return null;
        }
        //GetExplorePost
        [HttpPost("GetExplorePost")]
        public async Task<IActionResult> GetExplorePost(PaginationFileIdModel GetPostmodel)
        {
            if (GetPostmodel != null)
            {
                ExecutionResult<PaginatedResponse<spExplorepostfilesModel>> RequestModel = await _postRepo.GetExplorePost(GetPostmodel);

                string ResponceMessage = string.Empty;
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

            return null;
        }

        [HttpPost("GetPostTopCountFeeds")]
        public async Task<IActionResult> GetPostTopCountFeeds(GetPostmodel GetPostmodel)
        {
            if (GetPostmodel != null)
            {
                ExecutionResult<PaginatedResponse<PostDetailsModel>> RequestModel = await _postRepo.GetPostTopCountFeeds(GetPostmodel);

                string ResponceMessage = string.Empty;
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

            return null;
        }

        [HttpPost("GetTagUserofPost")]
        public async Task<IActionResult> GetTagUserofPost(GetByPostID GetByPostID)
        {
            //return FromExecutionResult(await _accountRepo.GetAllfollow(ID));
            ExecutionResult<List<FollowUsers>> RequestModel = await _postRepo.GetTagUserofPost(GetByPostID);
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


        [HttpPost("PostTagRequestAccept")]
        public async Task<IActionResult> PostTagRequestAccept(AcceptTagPostModel AcceptTagPostModel)
        {

            ExecutionResult RequestModel = await _postRepo.PostTagRequestAccept(AcceptTagPostModel);
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
            return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });
        }

        //RemoveComments
        [HttpPost("RemoveComments")]
        public async Task<IActionResult> RemoveComments(RemovePostCommentModel RemovePostModel)
        {

            ExecutionResult RequestModel = await _postRepo.RemoveComments(RemovePostModel);
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
            return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });
        }


        //RemoveComments
        [HttpPost("RemoveCommentsReply")]
        public async Task<IActionResult> RemoveCommentsReply(RemovePostCommentReplyModel RemovePostModel)
        {

            ExecutionResult RequestModel = await _postRepo.RemoveCommentsReply(RemovePostModel);
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
            return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });
        }

        [HttpPost("RemoveTagofPost")]
        public async Task<IActionResult> RemoveTagofPost(RemovePostModel RemovePostModel)
        {
            
            ExecutionResult RequestModel = await _postRepo.RemoveTagofPost(RemovePostModel);
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
            return new JsonResult(new { Result = "", meta = new { status = RequestModel.Success, message = ResponceMessage } });
        }

        /// <summary>
        /// Used to create post with caption and attached events
        /// </summary>
        /// <param name="createPost">Create post with given model having caption, event</param>
        /// <returns>New/Existing PostId When success or Error mesage when something went wrong</returns>
        [HttpPost("addeditpost")]
        public async Task<IActionResult> AddEditPost(CreatePostModel createPost)
        {
            if (createPost != null && !string.IsNullOrEmpty(createPost.Caption))
            {
                TblPostModel tblPostModel = new TblPostModel();
                tblPostModel.PostId = createPost.PostId;
                tblPostModel.Caption = createPost.Caption;
                tblPostModel.EventId = createPost.EventId == 0 ? null : createPost.EventId;
                tblPostModel.Id = createPost.Id;
                if(createPost.TagUserID != null)
                tblPostModel.TagUserID = createPost.TagUserID;
                else
                {
                    tblPostModel.TagUserID = new List<long>();
                }
                ExecutionResult<TblPostModel> RequestModel = await _postRepo.CreatePost(tblPostModel);

                string ResponceMessage = string.Empty;
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

            return null;
        }


        [HttpPost("UpdatedAddEditpost")]
        public async Task<IActionResult> UpdatedAddEditpost(CreatePostModel createPost)
        {
            if (createPost != null && !string.IsNullOrEmpty(createPost.Caption))
            {
                TblPostModel tblPostModel = new TblPostModel();
                tblPostModel.PostId = createPost.PostId;
                tblPostModel.Caption = createPost.Caption;
                tblPostModel.EventId = createPost.EventId == 0 ? null : createPost.EventId;
                tblPostModel.Id = createPost.Id;
                if (createPost.TagUserID != null)
                    tblPostModel.TagUserID = createPost.TagUserID;
                else
                {
                    tblPostModel.TagUserID = new List<long>();
                }
                tblPostModel.postFiles = createPost.postFiles;
                ExecutionResult<TblPostModel> RequestModel = await _postRepo.UpdatedCreatePost(tblPostModel);

                string ResponceMessage = string.Empty;
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

            return null;
        }


        /// <summary>
        /// Used to create Memorie with caption and attached events
        /// </summary>
        /// <param name="createMemorie">Create Memorie with given model having caption, event</param>
        /// <returns>New/Existing MemorieId When success or Error mesage when something went wrong</returns>
        [HttpPost("addeditMemorie")]
        public async Task<IActionResult> AddEditMemorie(CreateMemorieModel createMemorie)
        {
            if (createMemorie != null)
            {
                TblMemorieModel tblMemorieModel = new TblMemorieModel();
                tblMemorieModel.MemorieId = createMemorie.MemorieId;
                tblMemorieModel.Caption = createMemorie.Caption;
                tblMemorieModel.EventId = createMemorie.EventId == 0 ? null : createMemorie.EventId;
                tblMemorieModel.Id = createMemorie.Id;
                
                ExecutionResult<TblMemorieModel> RequestModel = await _postRepo.CreateMemorie(tblMemorieModel);

                string ResponceMessage = string.Empty;
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

            return null;
        }


        [HttpPost("CreateMemorieWithFiles")]
        public async Task<IActionResult> CreateMemorieWithFiles(CreateMemorieModel createMemorie)
        {
            if (createMemorie != null)
            {
                TblMemorieModel tblMemorieModel = new TblMemorieModel();
                tblMemorieModel.MemorieId = createMemorie.MemorieId;
                tblMemorieModel.Caption = createMemorie.Caption;
                tblMemorieModel.EventId = createMemorie.EventId == 0 ? null : createMemorie.EventId;
                tblMemorieModel.Id = createMemorie.Id;
                tblMemorieModel.MemorieFiles = createMemorie.MemorieFiles;
                ExecutionResult<TblMemorieModel> RequestModel = await _postRepo.CreateMemorieWithFiles(tblMemorieModel);

                string ResponceMessage = string.Empty;
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

            return null;
        }



        /// <summary>
        /// EditPrivacy
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="IsPrivate"></param>
        /// <returns></returns>
        [HttpPost("EditPostPrivacy")]
        public async Task<IActionResult> EditPostPrivacy(EditPostprivacyModel EditprivacyModel)
        {

            ExecutionResult RequestModel = await _postRepo.editPostprivacy(EditprivacyModel);



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
        /// EditPrivacy
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="IsPrivate"></param>
        /// <returns></returns>
        [HttpPost("editMemorieprivacy")]
        public async Task<IActionResult> editMemorieprivacy(EditMemorieprivacyModel EditprivacyModel)
        {

            ExecutionResult RequestModel = await _postRepo.editMemorieprivacy(EditprivacyModel);



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
        /// EditPrivacy
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="IsPrivate"></param>
        /// <returns></returns>
        [HttpPost("editMemorieFileprivacy")]
        public async Task<IActionResult> editMemorieFileprivacy(EditMemorieFileprivacyModel EditMemorieFileprivacyModel)
        {

            ExecutionResult RequestModel = await _postRepo.editMemorieFileprivacy(EditMemorieFileprivacyModel);



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
        /// Used to add images/videos to Post
        /// </summary>
        /// <param name="createPost"></param>
        /// <returns></returns>
        [HttpPost("addpostfiles")]       // Set max limit to 60MB
        public async Task<IActionResult> AddPostFiles([FromForm] CreatePostMultiFileUploadRequestModel createPostFiles)
        {
            //return FromExecutionResult(await _documentRepo.MultiFileUploadForCreatePost(createPostFiles));

            ExecutionResult<PostMediaModel> RequestModel = await _documentRepo.MultiFileUploadForCreatePost(createPostFiles);
            string ResponceMessage = string.Empty;
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


        /// <summary>
        /// Used to add images/videos to Memorie
        /// </summary>
        /// <param name="createMemorie"></param>
        /// <returns></returns>
        [HttpPost("addMemoriefiles")]       // Set max limit to 60MB
        public async Task<IActionResult> AddMemorieFiles([FromForm] CreateMemorieMultiFileUploadRequestModel createMemorieFiles)
        {
            //return FromExecutionResult(await _documentRepo.MultiFileUploadForCreateMemorie(createMemorieFiles));

            ExecutionResult<MemorieMediaModel> RequestModel = await _documentRepo.MultiFileUploadForCreateMemorie(createMemorieFiles);
            string ResponceMessage = string.Empty;
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

        /// <summary>
        /// Used to add likes and thumbs on the post
        /// </summary>
        /// <param name="likePostStatus">Create post with given model having caption, event</param>
        /// <returns>Like/Thumb added successfully or not</returns>
        [HttpPost("likepost")]
        public async Task<IActionResult> LikePost(LikeThumbPostModel likePostStatus)
        {
            //return FromExecutionResult(await _postRepo.LikePost(likePostStatus));

            ExecutionResult<LikeStatusModel> RequestModel = await _postRepo.LikePost(likePostStatus);

            string ResponceMessage = string.Empty;
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

        [HttpPost("LikePostFiles")]
        public async Task<IActionResult> LikePostFiles(LikeThumbPostFilesModel likePostStatus)
        {
            //return FromExecutionResult(await _postRepo.LikePost(likePostStatus));
            LikeThumbPostModel likeThumbPostModel = new LikeThumbPostModel();
            likeThumbPostModel.Id = likePostStatus.Id;
            likeThumbPostModel.LikeStatus = likePostStatus.LikeStatus;
            likeThumbPostModel.LikeType = likePostStatus.LikeType;
            likeThumbPostModel.PostId = likePostStatus.PostId;
            likeThumbPostModel.LikeStatus = likePostStatus.LikeStatus;

            ExecutionResult<FileLikeStatusModel> RequestModel = await _postRepo.LikePostFiles(likePostStatus);
            if (likePostStatus.LikeStatus != false)
            {
                ExecutionResult<LikeStatusModel> RequestModel1 = await _postRepo.LikePost(likeThumbPostModel);
            }
            string ResponceMessage = string.Empty;
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

        /// <summary>
        /// Used to get comments for given post
        /// </summary>
        /// <param name="getPostComments">Get all the comments added on post</param>
        /// <returns>Comments added on post</returns>
        [HttpPost("GetComments")]
        public async Task<IActionResult> GetPostComments(GetPostCommentModel getPostComments)
        {
            ExecutionResult<List<PostCommentModel>> RequestModel = await _postRepo.GetPostComments(getPostComments);
            string ResponceMessage = string.Empty;
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

        /// <summary>
        /// Used to add comments to post
        /// </summary>
        /// <param name="postComment">Comment details</param>
        /// <returns>Newly added comment details</returns>
        [HttpPost("addcomment")]
        public async Task<IActionResult> AddComments(AddPostCommentModel postComment)
        {
            if (postComment != null && !string.IsNullOrEmpty(postComment.Comment))
            {
                ExecutionResult<PostCommentModel> RequestModel = await _postRepo.AddComments(postComment);

                string ResponceMessage = string.Empty;
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

            return null;
        }

        /// <summary>
        /// Report / Block post 
        /// </summary>
        /// <param name="BlockPostModel">Post Block/Report</param>
        /// <returns>BlockPostModel</returns>
        [HttpPost("BlockPost")]
        public async Task<IActionResult> BlockPost(BlockPostModel BlockPostModel)
        {
         
                ExecutionResult<BlockPostResponce> RequestModel = await _postRepo.BlockPost(BlockPostModel);

                string ResponceMessage = string.Empty;
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

        //Add Reply on Event Comments Krupa
        [HttpPost("InsertPostCommentsReply")]
        public async Task<IActionResult> InsertPostCommentsReply(PostCommentReply PostCommentReply)
        {
            ExecutionResult<PostCommentReply> RequestModel = await _postRepo.InsertPostCommentsReply(PostCommentReply);
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
        /// Used to delete media file from Post
        /// </summary>
        /// <param name="removePostMedia">Details to remove post media</param>
        /// <returns>Successfully delete or not</returns>
        [HttpPost("removepostmedia")]
       
        public async Task<IActionResult> RemovePostMedia(RemovePostMediaModel removePostMedia)
        {
            ExecutionResult RequestModel = await _documentRepo.RemovePostMediaFile(removePostMedia);
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
        /// Used to delete Post
        /// </summary>
        /// <param name="removePostModel">Details to remove post</param>
        /// <returns>Successfully delete or not</returns>
        [HttpPost("deletepost")]
        public async Task<IActionResult> RemovePost(RemovePostModel removePostModel)
        {
            ExecutionResult RequestModel = await _postRepo.RemovePost(removePostModel);
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
        /// Used to get all Users of post liked 
        /// </summary>
        /// <param name="GetPostLikeUsers">unique indentification number</param>
        /// <returns>Users</returns>
        [HttpPost("GetpostLikesUsers")]
        public async Task<IActionResult> GetpostLikesUsers(GetPostLikeUsers GetPostLikeUsers)
        {
            ExecutionResult<PaginatedResponse<PostLikedUsers>> RequestModel = await _postRepo.GetpostLikesUsers(GetPostLikeUsers);
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
        /// Used to get all Users of post liked 
        /// </summary>
        /// <param name="GetPostLikeUsers">unique indentification number</param>
        /// <returns>Users</returns>
        [HttpPost("GetPostbyPostId")]
        public async Task<IActionResult> GetPostbyPostId(GetByPostID GetByPostID)
        {
            ExecutionResult<PostDetailsModel> RequestModel = await _postRepo.GetPostbyPostId(GetByPostID);
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
    }
}
