using System.Collections.Generic;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Account;
using Totem.Business.Core.DataTransferModels.Event;
using Totem.Business.Core.DataTransferModels.Post;

namespace Totem.Business.RepositoryInterface
{
    public interface IPostRepository
    {
        /// <summary>
        /// Used to create post 
        /// </summary>
        /// <param name="tblPostModel">Post Add/Edit details</param>
        /// <returns>New/Existing post Id</returns>
        /// 
        Task<ExecutionResult<TblPostModel>> UpdatedCreatePost(TblPostModel tblPostModel);

        Task<ExecutionResult<PaginatedResponse<spExplorepostfilesModel>>> GetExplorePost(PaginationFileIdModel PaginationFileIdModel);
        Task<ExecutionResult<TblPostModel>> CreatePost(TblPostModel tblPostModel);
        Task<ExecutionResult<TblMemorieModel>> CreateMemorie(TblMemorieModel tblMemorieModel);
        Task<ExecutionResult<TblMemorieModel>> CreateMemorieWithFiles(TblMemorieModel tblMemorieModel);

        Task<ExecutionResult> PostTagRequestAccept(AcceptTagPostModel AcceptTagPostModel);

        Task<ExecutionResult<List<FollowUsers>>> GetTagUserofPost(GetByPostID GetByPostID);

        Task<ExecutionResult> RemoveTagofPost(RemovePostModel RemovePostModel);

        /// <summary>
        /// Used to Post privacy Edit 
        /// </summary>
        /// <param name="editPostprivacy">Edit Post privacy details</param>
        /// <returns></returns>
        Task<ExecutionResult> editPostprivacy(EditPostprivacyModel EditPostprivacyModel);

        /// <summary>
        /// Used to get all the posts added by user 
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User posts</returns>
        Task<ExecutionResult<List<UserPostsModel>>> GetUserPosts(GetPostID GetPostID);



        /// <summary>
        /// Used to get all the posts added by to wom your are following pagination
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User posts</returns>
        Task<ExecutionResult<PaginatedResponse<PostDetailsModel>>> GetUserPostFeeds(GetPostmodel GetPostmodel);

        Task<ExecutionResult<PaginatedResponse<PostDetailsModel>>> GetALLPostTopCount(GetPostmodel GetPostmodel);
        Task<ExecutionResult<PaginatedResponse<PostDetailsModel>>> GetPostTopCountFeeds(GetPostmodel GetPostmodel);

        /// <summary>
        /// Used to get all user memories
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User memories</returns>
        Task<ExecutionResult<PaginatedResponse<UserMemorysDetailsModel>>> GetUserMemories(GetMemorieID GetMemorieID);
        Task<ExecutionResult> DeleteMemoriesFiles(GetPostID GetMemorieID);
        Task<ExecutionResult> editMemorieprivacy(EditMemorieprivacyModel EditMemorieprivacyModel);
        Task<ExecutionResult> editMemorieFileprivacy(EditMemorieFileprivacyModel EditMemorieFileprivacyModel);
        /// <summary>
        /// Used to get all the details related with Post
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User Post Details</returns>
        Task<ExecutionResult<List<UserPostDetailsModel>>> GetUserPostDetails(GetPostID GetPostID);

        /// <summary>
        /// Used to delete Post
        /// </summary>
        /// <param name="removePostModel">Details to remove post</param>
        /// <returns>Successfully delete or not</returns>
        Task<ExecutionResult> RemovePost(RemovePostModel removePostModel);

        /// <summary>
        /// Used to add likes and thumbs on the post
        /// </summary>
        /// <param name="likePostStatus">Create post with given model having caption, event</param>
        /// <returns>Like/Thumb added successfully or not</returns>
        Task<ExecutionResult<LikeStatusModel>> LikePost(LikeThumbPostModel likePostStatus);
        Task<ExecutionResult<FileLikeStatusModel>> LikePostFiles(LikeThumbPostFilesModel likePostStatus);

        Task<ExecutionResult> RemoveComments(RemovePostCommentModel postComment);
        Task<ExecutionResult> RemoveCommentsReply(RemovePostCommentReplyModel CommentsReply);

        /// <summary>
        /// Used to add comments to post
        /// </summary>
        /// <param name="postComment">Comment details</param>
        /// <returns>Newly added comment details</returns>
        Task<ExecutionResult<PostCommentModel>> AddComments(AddPostCommentModel postComment);

        /// <summary>
        /// Used to get comments for given post
        /// </summary>
        /// <param name="getPostComments">Get all the comments added on post</param>
        /// <returns>Comments added on post</returns>
        Task<ExecutionResult<List<PostCommentModel>>> GetPostComments(GetPostCommentModel getPostComments);

        /// <summary>
        /// Used to Insert Post Comments Reply for given post
        /// </summary>
        /// <param name="InsertPostCommentsReply">Get all the comments added on post</param>
        /// <returns>Insert Post Comments Reply added on post</returns>
        Task<ExecutionResult<PostCommentReply>> InsertPostCommentsReply(PostCommentReply obj);

        /// <summary>
        /// Used to get all Users of post liked 
        /// </summary>
        /// <param name="GetPostLikeUsers">unique indentification number</param>
        /// <returns>Users</returns>
        Task<ExecutionResult<PaginatedResponse<PostLikedUsers>>> GetpostLikesUsers(GetPostLikeUsers GetPostLikeUsers);

        Task<ExecutionResult<PostDetailsModel>> GetPostbyPostId(GetByPostID GetByPostID);


        /// <summary>
        /// Report / Block post 
        /// </summary>
        /// <param name="BlockPostModel">Post Block/Report</param>
        /// <returns>BlockPostModel</returns>
        Task<ExecutionResult<BlockPostResponce>> BlockPost(BlockPostModel BlockPostModel);
    }
}
