using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Account;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Totem.Business.Core.DataTransferModels.Spotify;
using Totem.Database.Models;
using Totem.Business.Core.DataTransferModels.Post;

namespace Totem.Business.RepositoryInterface
{
    public interface IAccountRepository
    {
        // Create Account
        Task<ExecutionResult> CreateAccount(CreateAccountRequestModel createAccountModel);

        Task<ExecutionResult<DashbordModel>> GetDastbord();
        Task<ExecutionResult> CheckMailExist(checkmail checkmail);

        Task<ExecutionResult<PaginatedResponse<NotificationModel>>> GetNotification(FollowModel FollowModel);
        Task<ExecutionResult> ClearNotification(UserIdModel UserIdModel);
        Task<ExecutionResult> Signup(InsertUsermanagement UsermanagementDetails);

        Task<ExecutionResult> editprivacy(EditprivacyModel EditprivacyModel);

        Task<ExecutionResult> Updateuser(UserProfile userProfile);
        Task<ExecutionResult> Deleteuser(Login Login);

        Task<ExecutionResult> ResendMailverify(GetSpotify GetSpotify);
        Task<ExecutionResult> AdminUserUpdate(AdminUserProfile userProfile);
        Task<ExecutionResult> UpdateBussnessUser(long Id, bool status);
        Task<ExecutionResult> UpdateProfileVerified(long Id, bool status);

        Task<ExecutionResult> EventActive(long EventId, bool status);

        Task<ExecutionResult<List<UsermanagementDetails>>> GetAllUsers();
        Task<ExecutionResult<List<GetAllUserReferedUser>>> GetAllReferedUsers();
        Task<ExecutionResult<List<GetAllUserReferedUser>>> GetAllReferedUsersByuserId(long Id);
        Task<ExecutionResult<List<VSupport>>> GetAllsupport();

        Task<ExecutionResult<List<GetAllUserStatusModel>>> GetAllUserStatus(int id);

        Task<ExecutionResult<GetAllUserStatuswithNotification>> GetAllUserStatus(long id);

        Task<ExecutionResult<List<UserFcmModel>>> GetFCMDetails(GetFcmModel GetFcmModel);

        //LogoutFCM
        Task<ExecutionResult> LogoutFCM(UserFcmModel UserFcmModel);

        Task<ExecutionResult<GetAllUserStatuswithNotification>> ConfigNotefication(AddUserNotification AddUserNotification);

        //Invalid Login Attempts Blocked
        Task<ExecutionResult> InvalidLoginAttempts(EmailModel EmailId);

        //Login
        ExecutionResult<UsermanagementDetailsID> Login(Login login);
        ExecutionResult<UsermanagementDetailsID> AdminLogin(Login login);
        //Forget Password
        Task<ExecutionResult> ForgetPassword(string EmailId);
        //Reset Password
        Task<ExecutionResult> ResetPassword(Guid EmailToken, string Password);

        Task<ExecutionResult> EmailConfirmation(string EmailToken);

        ExecutionResult ChangePassword(long accountId, string OldPassword, string Password);


        Task<ExecutionResult> InsertFAQ(FAQ FAQ);
        Task<ExecutionResult<List<FAQDetails>>> GetAllFAQ();

        Task<ExecutionResult> Contactus(ContactUs contactUs);

        Task<ExecutionResult> GetAllContactUs();
        //Task<ExecutionResult> FAQAnswer(FAQ_answer FAQ_answer);

        Task<ExecutionResult> FAQUpdate(FAQDetails FAQDetails);

        Task<ExecutionResult> FAQDelete(int Id);

        Task<ExecutionResult> Spotify(SpotifyModel SpotifyModel);

        Task<ExecutionResult<GetSpotifyModel>> GetSpotify(GetSpotify GetSpotify);
        Task<ExecutionResult<GetSpotifyModel>> GetTopSpotify();
        //Task<ExecutionResult<SpotifyModel>> GetSpotify(GetSpotify GetSpotify);

        Task<ExecutionResult> FavSongs(Spotifysongs Spotifysongs);
        Task<ExecutionResult<List<Songs>>> GetTopSongs();
        Task<ExecutionResult<List<Songs>>> GetFavSongs(GetSpotify GetSpotify);
        Task<ExecutionResult> BusinessUserReq(BusinessModel BusinessModel);

        Task<ExecutionResult<GetBusinessModel>> GetProfileVerifyReq(long Id);
       
        Task<ExecutionResult> ProfileVerifyReq(VerifyProfile VerifyProfile);

        Task<ExecutionResult<List<AdminUserPostsModel>>> AdminGetUserPosts();
        Task<ExecutionResult<List<AdminUserPostsModel>>> AdminGetPostByID(long PostId);
        ExecutionResult<Versionmodel> GetUpdatedVersion(GetVersionmodel GetVersionmodel, Versionmodel Versionmodel);
        #region Block user request by User add and remove
        Task<ExecutionResult> BlockUser(BlockUserModel BlockUserModel);
        Task<ExecutionResult> PresentLiveStatus(PresentLiveStatusModel PresentLiveStatusModel);
        Task<ExecutionResult<PaginatedResponse<FollowUsers>>> GetBlockuser(FollowModel FollowModel);
        #endregion
    }
}
