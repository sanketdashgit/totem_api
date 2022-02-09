using AutoMapper;
using Totem.Business.Core.AppSettings;
using Totem.Business.Core.Consts;
using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Account;
using Totem.Business.Helpers;
using Totem.Business.RepositoryInterface;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Totem.Database.Models;
using Totem.Business.Core.DataTransferModels.Spotify;


using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Totem.Business.Core.DataTransferModels.Event;
using System.Data;
using Totem.Database.StoreProcedure;
using Totem.Business.Core.DataTransferModels.Post;

namespace Totem.Business.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        #region Constructor

        private readonly TotemDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly TokenManager _tokenManger;
        private readonly CommonMethods _commonMethods;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly string _connectionString;

        public AccountRepository(
            TotemDBContext dbContext,
            IMapper mapper,
            TokenManager tokenManger,
            IOptions<AppSettings> appSettings,
            CommonMethods commonMethods)
        {
            _commonMethods = commonMethods;
            _appSettings = appSettings;
            _tokenManger = tokenManger;
            _mapper = mapper;
            _dbContext = dbContext;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
        }

        #endregion

        #region EmailConfirmation
        public async Task<ExecutionResult> EmailConfirmation(string EmailToken)
        {
            if (EmailToken != null)
            {
                var emailConfirmation = _dbContext.Usermanagements.FirstOrDefault(x => x.EmailToken.ToString().ToLower() == EmailToken.ToString().ToLower());
                if (emailConfirmation != null)
                {
                    emailConfirmation.IsActive = true;
                    emailConfirmation.InvalidLoginAttempts = false;
                    emailConfirmation.IsEmailVerified = true;
                    _dbContext.Usermanagements.Update(emailConfirmation);
                    await _dbContext.SaveChangesAsync();
                    return new ExecutionResult();
                }
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "account")));
            }
            return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Email token")));
        }
        #endregion


        #region CreateAccount

        public async Task<ExecutionResult> CreateAccount(CreateAccountRequestModel createAccountModel)
        {
            if (createAccountModel == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var account = _dbContext.Usermanagements.Where(x => x.Email.ToLower() == createAccountModel.Email.ToLower()).Any();
            if (account)
            {
                ErrorInfo errorInfo = new ErrorInfo(string.Format(MessageHelper.EmailExist));
                return new ExecutionResult(errorInfo);
                //return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.EmailExist)));
            }
            //account = _dbContext.Usermanagements.Where(x => x.Phone.ToLower() == createAccountModel.Phone.ToLower()).Any();
            //if (account)
            //{
            //    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.PhoneExist)));
            //}
            account = _dbContext.Usermanagements.Where(x => x.Username.ToLower() == createAccountModel.Username.ToLower()).Any();
            if (account)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.UserNameExist)));
            }

            var createAcc = _mapper.Map<Usermanagement>(createAccountModel);
            createAcc.EmailToken = Guid.NewGuid();
            await _dbContext.Usermanagements.AddAsync(createAcc);
            await _dbContext.SaveChangesAsync();

            string link = _appSettings.Value.Login + "/" + createAcc.EmailToken.ToString();

            string HTMLString = EmailHelper.AccountActivation(createAccountModel.FirstName, createAccountModel.LastName, link);

            var isMailSent = await _commonMethods.SendMail(createAccountModel.Email, HTMLString, MessageHelper.Account.SubjectAccountActivation);

            if (!isMailSent)
            {
                return new ExecutionResult(new ErrorInfo(MessageHelper.Account.EmailSentFailedWhileActivation));
            }
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Account", "added", "please check confirmation email")));
        }



        #endregion

        #region Check Existing Mail
        public async Task<ExecutionResult> CheckMailExist(checkmail checkmail)
        {
            if (checkmail == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var account = _dbContext.Usermanagements.Where(x => x.Email.ToLower() == checkmail.EmailId.ToLower()).Any();
            if (account)
            {
                ErrorInfo errorInfo = new ErrorInfo(string.Format(MessageHelper.EmailExist));
                return new ExecutionResult(errorInfo);
                //return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.EmailExist)));
            }
            account = _dbContext.Usermanagements.Where(x => x.Username.ToLower() == checkmail.Username.ToLower()).Any();
            if (account)
            {
                ErrorInfo errorInfo = new ErrorInfo(string.Format(MessageHelper.UserNameExist));
                return new ExecutionResult(errorInfo);
                //return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.EmailExist)));
            }
            //account = _dbContext.Usermanagements.Where(x => x.Phone.ToLower() == checkmail.Phone.ToLower()).Any();
            //if (account)
            //{
            //    ErrorInfo errorInfo = new ErrorInfo(string.Format(MessageHelper.PhoneExist));
            //    return new ExecutionResult(errorInfo);
            //    //return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.EmailExist)));
            //}
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Account", "Ckecked", "This Email Does't Exist")));
        }

        #endregion

        #region  notification
        public async Task<ExecutionResult<PaginatedResponse<NotificationModel>>> GetNotification(FollowModel FollowModel)
        {
            PaginatedResponse<NotificationModel> response = new PaginatedResponse<NotificationModel>();
            var GetNotification = _dbContext.TblNotifications.Where(x => x.Id == FollowModel.ID && x.Readflag != "3" && x.Title.Contains(FollowModel.Search)).OrderByDescending(x => x.Date).ToList();
            var PageNotefication = GetNotification.Skip((FollowModel.PageNumber - 1) * FollowModel.PageSize)
                                                .Take(FollowModel.PageSize).ToList();
            response.Data = _mapper.Map<List<NotificationModel>>(PageNotefication);
            response.PageNumber = FollowModel.PageNumber;
            response.PageSize = FollowModel.PageSize;
            response.TotalRecords = FollowModel.TotalRecords > 0 ? FollowModel.TotalRecords :
               GetNotification.Count();
            return PageNotefication != null ? new ExecutionResult<PaginatedResponse<NotificationModel>>(response) :
               new ExecutionResult<PaginatedResponse<NotificationModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Notification")));
        }

        public async Task<ExecutionResult> ClearNotification(UserIdModel UserIdModel)
        {
            var GetNotification = _dbContext.TblNotifications.Where(x => x.Id == UserIdModel.Id && x.NotificationType != "POST").ToList();
            GetNotification.ForEach(x => x.Readflag = "3");
            await _dbContext.SaveChangesAsync();
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Notification", "Cleared", "")));
        }

        #endregion

        #region signin from mobile
        public async Task<ExecutionResult> Signup(InsertUsermanagement createAccountModel)
        {
            if (createAccountModel == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var account = _dbContext.Usermanagements.Where(x => x.Email.ToLower() == createAccountModel.Email.ToLower()).Any();
            if (account)
            {
                ErrorInfo errorInfo = new ErrorInfo(string.Format(MessageHelper.EmailExist));
                return new ExecutionResult(errorInfo);
                //return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.EmailExist)));
            }
            //account = _dbContext.Usermanagements.Where(x => x.Phone.ToLower() == createAccountModel.Phone.ToLower()).Any();
            //if (account)
            //{
            //    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.PhoneExist)));
            //}
            account = _dbContext.Usermanagements.Where(x => x.Username.ToLower() == createAccountModel.Username.ToLower()).Any();
            if (account)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.UserNameExist)));
            }
            var createAcc = _mapper.Map<Usermanagement>(createAccountModel);
            createAcc.EmailToken = Guid.NewGuid();
            await _dbContext.Usermanagements.AddAsync(createAcc);
            await _dbContext.SaveChangesAsync();

            var ReferedaccountUpdate = _dbContext.Usermanagements.Where(x => x.Id == createAccountModel.ReferredBy).FirstOrDefault();
            if(ReferedaccountUpdate != null)
            {
                ReferedaccountUpdate.TotemPoints = ReferedaccountUpdate.TotemPoints + 1;
                await _dbContext.SaveChangesAsync();
            }

            string link = _appSettings.Value.Login + "/" + createAcc.EmailToken.ToString();

            string HTMLString = EmailHelper.AccountActivation(createAccountModel.Firstname, createAccountModel.Lastname, link);

            var isMailSent = await _commonMethods.SendMail(createAccountModel.Email, HTMLString, MessageHelper.Account.SubjectAccountActivation);

            if (!isMailSent)
            {
                return new ExecutionResult(new InfoMessage(MessageHelper.Account.EmailSentFailedWhileActivation));
            }
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Account", "added", "please check confirmation email")));
        }

        #endregion


        #region edit privacy from mobile
        public async Task<ExecutionResult> editprivacy(EditprivacyModel EditprivacyModel)
        {
            if (EditprivacyModel == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var account = _dbContext.Usermanagements.Where(x => x.Id == EditprivacyModel.Id).FirstOrDefault();
            if (account == null)
            {
                ErrorInfo errorInfo = new ErrorInfo(string.Format(MessageHelper.NoFound));
                return new ExecutionResult(errorInfo);

            }
            else
            {
                account.IsPrivate = EditprivacyModel.IsPrivate;
            }
            await _dbContext.SaveChangesAsync();

            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Account", "privacy", "Changed")));
        }

        #endregion


        public async Task<ExecutionResult> ResendMailverify(GetSpotify GetSpotify)
        {
            var createAcc = _dbContext.Usermanagements.Where(x => x.Id == GetSpotify.Id).FirstOrDefault();
            if (createAcc != null)
            {

                string link = _appSettings.Value.Login + "/" + createAcc.EmailToken.ToString();

                string HTMLString = EmailHelper.AccountActivation(createAcc.Firstname, createAcc.Lastname, link);

                var isMailSent = await _commonMethods.SendMail(createAcc.Email, HTMLString, MessageHelper.Account.SubjectAccountActivation);

                if (!isMailSent)
                {
                    return new ExecutionResult(new ErrorInfo(MessageHelper.Account.EmailSentFailedWhileActivation));
                }
                else
                {
                    return new ExecutionResult(new InfoMessage("Email Sent Sucessfully"));
                }
            }
            else
            {
                return new ExecutionResult(new ErrorInfo(MessageHelper.NoFound, "Account"));
            }

        }


        #region Update Bussness User
        public async Task<ExecutionResult> UpdateBussnessUser(long Id, bool status)
        {

            if (Id != null)
            {
                var account = _dbContext.Usermanagements.FirstOrDefault(x => x.Id == Id);
                if (account != null)
                {
                    if (account.Role != 1)
                    {
                        if (status)
                        {
                            account.Role = 2;
                        }
                        else
                        {
                            account.Role = 0;
                        }
                    }
                    //account.Image = userProfile.Image;
                    _dbContext.SaveChanges();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Account", "Updated", "")));

                }
                else
                {
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "Account")));
                }
            }
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "Account")));



        }
        #endregion

        #region Update Profile Verified
        public async Task<ExecutionResult> UpdateProfileVerified(long Id, bool status)
        {

            if (Id != null)
            {
                var account = _dbContext.Usermanagements.FirstOrDefault(x => x.Id == Id);
                if (account != null)
                {

                    account.ProfileVerified = status;

                    _dbContext.SaveChanges();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Account", "Updated", "")));

                }
                else
                {
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "Account")));
                }
            }
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "Account")));



        }
        #endregion


        #region update Admin Profile
        public async Task<ExecutionResult> AdminUserUpdate(AdminUserProfile userProfile)
        {

            if (userProfile != null)
            {

                var account = _dbContext.Usermanagements.FirstOrDefault(x => x.Id == 1);
                if (account != null)
                {

                    account.Firstname = userProfile.FirstName;
                    account.Lastname = userProfile.LastName;

                    _dbContext.SaveChanges();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Admin Account", "Updated", "")));

                }
                else
                {
                    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "Admin Account")));
                }
            }
            return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "Admin Account")));



        }
        #endregion


        #region update User Profile
        public async Task<ExecutionResult> Updateuser(UserProfile userProfile)
        {

            if (userProfile != null)
            {
                if (userProfile.Id == 1)
                {
                    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.WrongIdMessage, "User ID. This is an Admin", "Updated", "")));
                }
                var account = _dbContext.Usermanagements.FirstOrDefault(x => x.Id == userProfile.Id);
                if (account != null)
                {
                    account.Id = userProfile.Id;
                    account.Firstname = userProfile.FirstName;
                    account.Lastname = userProfile.LastName;
                    account.Username = userProfile.Username;
                    account.Email = userProfile.Email;
                    account.Phone = userProfile.Phone;
                    account.BirthDate = userProfile.BirthDate;
                    account.ModifiedDate = DateTime.UtcNow;
                    account.Address = userProfile.Address;
                    account.Latitude = userProfile.Latitude;
                    account.Longitude = userProfile.Longitude;
                    account.Bio = userProfile.Bio;
                    _dbContext.SaveChanges();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Account", "Updated", "")));

                }
                else
                {
                    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "Account")));
                }
            }
            return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "Account")));



        }
        #endregion

        #region Delete User Profile
        public async Task<ExecutionResult> Deleteuser(Login login)
        {
            if (!string.IsNullOrWhiteSpace(login.EmailId) && !string.IsNullOrWhiteSpace(login.Password) && login.SignInType == 0)
            {
                //UsermanagementDetailsID model = new UsermanagementDetailsID();
                var account = _dbContext.Usermanagements.Where(x => x.Email == login.EmailId && x.Password == login.Password).FirstOrDefault();

                if (account != null)
                {
                    if (account.Id == 1)
                    {
                        return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.WrongIdMessage, "User ID. This is an Admin", "Deleted", "")));
                    }
                    account.IsDeleted = true;
                    account.IsActive = false;
                    account.ModifiedDate = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();

                    var fallow = _dbContext.TblFollowers.Where(x => x.UserId == account.Id || x.FollowerId == account.Id).ToList();
                    foreach (var item in fallow)
                    {
                        item.IsDeleted = true;
                    }
                    await _dbContext.SaveChangesAsync();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Account", "Deleted", "")));
                }
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "Account")));
            }
            if (!string.IsNullOrWhiteSpace(login.EmailId) && login.SignInType != 0)
            {
                var account = _dbContext.Usermanagements.Where(x => x.Email == login.EmailId && x.SignInType == login.SignInType).FirstOrDefault();
                if (account != null)
                {
                    if (account.Id == 1)
                    {
                        return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.WrongIdMessage, "User ID. This is an Admin", "Deleted", "")));
                    }
                    account.IsDeleted = true;
                    account.ModifiedDate = DateTime.UtcNow;
                    _dbContext.SaveChanges();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Account", "Deleted", "")));

                }
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "Account")));
            }
            return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "Account")));



        }
        #endregion

        #region Get All Users
        public async Task<ExecutionResult<List<VSupport>>> GetAllsupport()
        {
            var AllUsers = _dbContext.VSupports.ToList();

            return AllUsers.Count > 0 ? new ExecutionResult<List<VSupport>>(_mapper.Map<List<VSupport>>(AllUsers)) :
               new ExecutionResult<List<VSupport>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }


        #endregion


        #region Get All Users
        public async Task<ExecutionResult<List<UsermanagementDetails>>> GetAllUsers()
        {
            var AllUsers = _dbContext.Usermanagements.Where(x => x.IsActive == true).OrderByDescending(x => x.Id).ToList();

            return AllUsers.Count > 0 ? new ExecutionResult<List<UsermanagementDetails>>(_mapper.Map<List<UsermanagementDetails>>(AllUsers)) :
               new ExecutionResult<List<UsermanagementDetails>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }
        #endregion
        #region Get All Refered Users
        public async Task<ExecutionResult<List<GetAllUserReferedUser>>> GetAllReferedUsers()
        {
            List<GetAllUserReferedUser> result = new List<GetAllUserReferedUser>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "GetRefferedCount";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@Id", id);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable
                    DataSet ds = new DataSet();

                    da.Fill(table);

                    try
                    {

                        result = (from DataRow dr in table.Rows
                                  select new GetAllUserReferedUser()
                                  {
                                      Id = Convert.ToInt64(dr["Id"]),
                                      Firstname = dr["Firstname"].ToString(),
                                      Lastname = dr["Lastname"].ToString(),
                                      Username = dr["Username"].ToString(),
                                      Email = dr["Email"].ToString(),
                                      Phone = dr["Phone"].ToString(),
                                      RefferedCount = dr["RefferedCount"].ToString(),


                                  }).ToList();


                    }
                    catch (Exception ex)
                    {

                        throw;
                    }


                }
            }

            return result != null ? new ExecutionResult<List<GetAllUserReferedUser>>(_mapper.Map<List<GetAllUserReferedUser>>(result)) :
               new ExecutionResult<List<GetAllUserReferedUser>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }

        public async Task<ExecutionResult<List<GetAllUserReferedUser>>> GetAllReferedUsersByuserId(long Id)
        {
            var createAcc = _dbContext.Usermanagements.Where(x => x.ReferredBy == Id).ToList();
            List<GetAllUserReferedUser> result = new List<GetAllUserReferedUser>();
            result = (from dr in createAcc
                      select new GetAllUserReferedUser()
                      {
                          Id = dr.Id,
                          Firstname = dr.Firstname,
                          Lastname = dr.Lastname,
                          Username = dr.Username,
                          Email = dr.Email,
                          Phone = dr.Phone,
                          CreatedDate = dr.CreatedDate,
                          RefferedCount = 0.ToString()
                      }).ToList();

            return result != null ? new ExecutionResult<List<GetAllUserReferedUser>>(_mapper.Map<List<GetAllUserReferedUser>>(result)) :
               new ExecutionResult<List<GetAllUserReferedUser>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        }


        #endregion

        #region Admin Dashboard
        public async Task<ExecutionResult<DashbordModel>> GetDastbord()
        {
            var account = _dbContext.VDashbords.FirstOrDefault();
            return new ExecutionResult<DashbordModel>(_mapper.Map<DashbordModel>(account));


        }

        #endregion


        #region Get All User Status
        //public async Task<ExecutionResult<List<V_UserdetailModel>>> GetAllUserStatus()
        //{
        //    var AllUsers = _dbContext.VGetUserdetails.Where(x => x.Id == x.Id).OrderByDescending(x => x.Id).ToList();

        //    return AllUsers.Count > 0 ? new ExecutionResult<List<V_UserdetailModel>>(_mapper.Map<List<V_UserdetailModel>>(AllUsers)) :
        //       new ExecutionResult<List<V_UserdetailModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Users")));
        //}


        public async Task<ExecutionResult<List<GetAllUserStatusModel>>> GetAllUserStatus(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "Sp_GetUserDetails";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable
                    DataSet ds = new DataSet();

                    da.Fill(table);
                    List<GetAllUserStatusModel> result = new List<GetAllUserStatusModel>();
                    try
                    {

                        result = (from DataRow dr in table.Rows
                                  select new GetAllUserStatusModel()
                                  {
                                      Id = Convert.ToInt64(dr["Id"]),
                                      Firstname = dr["Firstname"].ToString(),
                                      Lastname = dr["Lastname"].ToString(),
                                      Username = dr["Username"].ToString(),
                                      Email = dr["Email"].ToString(),
                                      Phone = dr["Phone"].ToString(),
                                      BirthDate = dr["BirthDate"].ToString(),
                                      Image = dr["Image"].ToString(),
                                      BussinessUser = Convert.ToBoolean(dr["BussinessUser"]),
                                      ProfileVerified = Convert.ToBoolean(dr["ProfileVerified"]),
                                      IsBusinessRequestSend = Convert.ToBoolean(dr["IsBusinessRequestSend"]),
                                      IsProfileVarificationRequestSend = Convert.ToBoolean(dr["IsProfileVarificationRequestSend"]),
                                      IsPrivate = Convert.ToBoolean(dr["IsPrivate"])

                                  }).ToList();


                    }
                    catch (Exception ex)
                    {

                        throw;
                    }


                    return result != null ? new ExecutionResult<List<GetAllUserStatusModel>>(result) :
                new ExecutionResult<List<GetAllUserStatusModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "User")));
                }
            }



        }


        public async Task<ExecutionResult<GetAllUserStatuswithNotification>> GetAllUserStatus(long id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "Sp_GetUserDetails";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable
                    DataSet ds = new DataSet();

                    da.Fill(table);
                    GetAllUserStatuswithNotification result = new GetAllUserStatuswithNotification();
                    if (table.Rows.Count > 0)
                    {
                        var account = _dbContext.Usermanagements.Where(x => x.Id == id).FirstOrDefault();
                        result = _mapper.Map<GetAllUserStatuswithNotification>(account);
                        result.BussinessUser = Convert.ToBoolean(table.Rows[0]["BussinessUser"]);
                        result.ProfileVerified = Convert.ToBoolean(table.Rows[0]["ProfileVerified"]);
                        result.IsBusinessRequestSend = Convert.ToBoolean(table.Rows[0]["IsBusinessRequestSend"]);
                        result.IsProfileVarificationRequestSend = Convert.ToBoolean(table.Rows[0]["IsProfileVarificationRequestSend"]);
                        result.IsPrivate = Convert.ToBoolean(table.Rows[0]["IsPrivate"]);
                        result.Token = "Bearer" + " " + (_tokenManger.BuildToken(account.Id)).ToString().Trim();

                    }

                    return result.Id != 0 ? new ExecutionResult<GetAllUserStatuswithNotification>(result) :
                new ExecutionResult<GetAllUserStatuswithNotification>(new ErrorInfo(string.Format(MessageHelper.NoFound, "User")));
                }
            }



        }


        #endregion

        #region Get FCM Details
        //
        public async Task<ExecutionResult<List<UserFcmModel>>> GetFCMDetails(GetFcmModel GetFcmModel)
        {
            GetFcmModel.Id = _dbContext.Usermanagements.Where(x => x.MessageNotification == true && GetFcmModel.Id.Contains(x.Id)).Select(x => x.Id).ToList();
            var Fcmdetails = _dbContext.TblUserFcms.Where(x => GetFcmModel.Id.Contains(x.Id) && x.Login == true).ToList();
            return Fcmdetails != null ? new ExecutionResult<List<UserFcmModel>>(_mapper.Map<List<UserFcmModel>>(Fcmdetails)) :
             new ExecutionResult<List<UserFcmModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "FCM")));

        }
        #endregion


        #region save FCM Details
        public async Task<ExecutionResult> LogoutFCM(UserFcmModel UserFcmModel)
        {
            var Fcmdetails = _dbContext.TblUserFcms.Where(x => x.Fcm == UserFcmModel.Fcm).FirstOrDefault();
            if (UserFcmModel.Login != true)
            {
                var userdetails = _dbContext.Usermanagements.Where(x => x.Id == UserFcmModel.Id).FirstOrDefault();
                userdetails.PresentLiveStatus = 0;
                _dbContext.Usermanagements.Update(userdetails);
                await _dbContext.SaveChangesAsync();
            }
            if (Fcmdetails == null)
            {
                var createfaq = _mapper.Map<TblUserFcm>(UserFcmModel);
                await _dbContext.TblUserFcms.AddAsync(createfaq);
                await _dbContext.SaveChangesAsync();
                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "FCM", "added", "")));
            }
            else
            {
                Fcmdetails.Id = UserFcmModel.Id;
                Fcmdetails.Login = UserFcmModel.Login;
                _dbContext.TblUserFcms.Update(Fcmdetails);
                await _dbContext.SaveChangesAsync();
                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "FCM", "Updated", "")));
            }


        }
        #endregion

        #region Status of Golive - Interest - Favorite

        public async Task<ExecutionResult<GetAllUserStatuswithNotification>> ConfigNotefication(AddUserNotification AddUserNotification)
        {
            if (AddUserNotification == null)
            {
                return new ExecutionResult<GetAllUserStatuswithNotification>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            var AllConfigNotefication = _dbContext.Usermanagements
                 .Where(x => x.Id == AddUserNotification.Id).FirstOrDefault();
            if (AllConfigNotefication != null)
            {
                AllConfigNotefication.MessageNotification = AddUserNotification.MessageNotification;
                AllConfigNotefication.EventNotification = AddUserNotification.EventNotification;
                AllConfigNotefication.FollowNotification = AddUserNotification.FollowNotification;
                AllConfigNotefication.ModifiedDate = DateTime.UtcNow;
                _dbContext.SaveChanges();
            }


            return AllConfigNotefication != null ? new ExecutionResult<GetAllUserStatuswithNotification>(_mapper.Map<GetAllUserStatuswithNotification>(AllConfigNotefication)) :
              new ExecutionResult<GetAllUserStatuswithNotification>(new ErrorInfo(string.Format(MessageHelper.NoFound, "User")));


        }



        #endregion

        #region Invalid Login Attempts Block

        public async Task<ExecutionResult> InvalidLoginAttempts(EmailModel EmailModel)
        {
            if (EmailModel == null)
            {
                return new ExecutionResult<GetAllUserStatuswithNotification>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            var AllConfigNotefication = _dbContext.Usermanagements
                 .Where(x => x.Email == EmailModel.EmailId).FirstOrDefault();
            if (AllConfigNotefication == null)
            {
                AllConfigNotefication = _dbContext.Usermanagements
                    .Where(x => x.Phone == EmailModel.EmailId).FirstOrDefault();
            }
            if (AllConfigNotefication == null)
            {
                AllConfigNotefication = _dbContext.Usermanagements
                    .Where(x => x.Username == EmailModel.EmailId).FirstOrDefault();
            }

            if (AllConfigNotefication != null)
            {
                if (AllConfigNotefication.Id != 1)
                {
                    AllConfigNotefication.InvalidLoginAttempts = true;
                    AllConfigNotefication.EmailToken = new Guid();
                    _dbContext.SaveChanges();

                    string link = _appSettings.Value.Login + "/" + AllConfigNotefication.EmailToken.ToString();

                    string HTMLString = EmailHelper.AccountActivation(AllConfigNotefication.Firstname, AllConfigNotefication.Lastname, link);

                    var isMailSent = await _commonMethods.SendMail(AllConfigNotefication.Email, HTMLString, MessageHelper.Account.SubjectAccountActivation);

                    if (!isMailSent)
                    {
                        return new ExecutionResult(new InfoMessage(MessageHelper.Account.EmailSentFailedWhileActivation));
                    }
                }

            }

            return new ExecutionResult<GetAllUserStatuswithNotification>(new ErrorInfo(string.Format(MessageHelper.LoginAttemptsFailed)));



        }



        #endregion

        #region Login
        public ExecutionResult<UsermanagementDetailsID> Login(Login login)
        {
            if (!string.IsNullOrWhiteSpace(login.EmailId) && !string.IsNullOrWhiteSpace(login.Password) && login.SignInType == 0)
            {
                //UsermanagementDetailsID model = new UsermanagementDetailsID();
                var account = _dbContext.Usermanagements.Where(x => x.Email == login.EmailId && EF.Functions.Collate(x.Password, "SQL_Latin1_General_CP1_CS_AS") == login.Password).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (account == null)
                {
                    account = _dbContext.Usermanagements.Where(x => x.Phone == login.EmailId && EF.Functions.Collate(x.Password, "SQL_Latin1_General_CP1_CS_AS") == login.Password).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                }
                if (account == null)
                {
                    account = _dbContext.Usermanagements.Where(x => x.Username == login.EmailId && EF.Functions.Collate(x.Password, "SQL_Latin1_General_CP1_CS_AS") == login.Password).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                }
                if (account != null)
                {
                    if (account.IsDeleted)
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.AccountDeleted)));
                    }
                    if (account.IsActive == false && !string.IsNullOrWhiteSpace(Convert.ToString(account.IsDeleted)))
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.Account.ActiveMessage)));
                    }
                    if (!account.IsActive)
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.Account.EmailConfirmFail)));
                    }
                    if (account.ByPostBlocked)
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.Account.ReportedBlocked)));
                    }


                    var accountData = _mapper.Map<UsermanagementDetailsID>(account);
                    accountData.Token = "Bearer" + " " + (_tokenManger.BuildToken(account.Id)).ToString().Trim();
                    if (account.InvalidLoginAttempts == true)
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.LoginAttemptsFailed)));
                    }

                    if (account.Id != 1)
                    {
                        UserFcmModel userFcmModel = new UserFcmModel();
                        userFcmModel.Id = account.Id;
                        userFcmModel.Fcm = login.Fcm;
                        userFcmModel.Login = true;
                        var Fcmdetails = _dbContext.TblUserFcms.Where(x => x.Fcm == userFcmModel.Fcm && x.Id == account.Id).FirstOrDefault();
                        if (Fcmdetails == null)
                        {
                            var createfaq = _mapper.Map<TblUserFcm>(userFcmModel);
                            _dbContext.TblUserFcms.AddAsync(createfaq);
                            _dbContext.SaveChanges();

                        }
                        else
                        {
                            Fcmdetails.Id = account.Id;
                            Fcmdetails.Login = true;
                            Fcmdetails.Fcm = login.Fcm;
                            _dbContext.TblUserFcms.Update(Fcmdetails);
                            _dbContext.SaveChanges();

                        }
                    }
                    return new ExecutionResult<UsermanagementDetailsID>(accountData);
                }


                return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.Account.LoginFail)));
            }
            if (!string.IsNullOrWhiteSpace(login.EmailId) && login.SignInType != 0)
            {
                //UsermanagementDetailsID model = new UsermanagementDetailsID();
                var account = _dbContext.Usermanagements.Where(x => x.Email == login.EmailId).FirstOrDefault();
                if (account != null)
                {
                    if (account.IsDeleted)
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.AccountDeleted)));
                    }
                    if (account.IsActive == false && !string.IsNullOrWhiteSpace(Convert.ToString(account.IsDeleted)))
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.Account.ActiveMessage)));
                    }
                    if (!account.IsActive)
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.Account.EmailConfirmFail)));
                    }
                    var accountData = _mapper.Map<UsermanagementDetailsID>(account);
                    accountData.Token = "Bearer" + " " + (_tokenManger.BuildToken(account.Id)).ToString().Trim();

                    UserFcmModel userFcmModel = new UserFcmModel();
                    userFcmModel.Id = account.Id;
                    userFcmModel.Fcm = login.Fcm;
                    userFcmModel.Login = true;
                    var Fcmdetails = _dbContext.TblUserFcms.Where(x => x.Fcm == userFcmModel.Fcm && x.Id == account.Id).FirstOrDefault();
                    if (Fcmdetails == null)
                    {
                        var createfaq = _mapper.Map<TblUserFcm>(userFcmModel);
                        _dbContext.TblUserFcms.AddAsync(createfaq);
                        _dbContext.SaveChanges();

                    }
                    else
                    {
                        Fcmdetails.Id = account.Id;
                        Fcmdetails.Login = true;
                        Fcmdetails.Fcm = login.Fcm;
                        _dbContext.TblUserFcms.Update(Fcmdetails);
                        _dbContext.SaveChanges();

                    }

                    return new ExecutionResult<UsermanagementDetailsID>(accountData);
                }
                return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.EmailNotRegistred)));
            }
            return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.PassParaMessage, "Email address or password")));
        }
        #endregion


        #region AdminLogin
        public ExecutionResult<UsermanagementDetailsID> AdminLogin(Login login)
        {
            if (!string.IsNullOrWhiteSpace(login.EmailId) && !string.IsNullOrWhiteSpace(login.Password))
            {
                //UsermanagementDetailsID model = new UsermanagementDetailsID();
                var account = _dbContext.Usermanagements.Where(x => x.Email == login.EmailId && EF.Functions.Collate(x.Password, "SQL_Latin1_General_CP1_CS_AS") == login.Password).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (account == null)
                {
                    account = _dbContext.Usermanagements.Where(x => x.Phone == login.EmailId && EF.Functions.Collate(x.Password, "SQL_Latin1_General_CP1_CS_AS") == login.Password).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                }
                if (account == null)
                {
                    account = _dbContext.Usermanagements.Where(x => x.Username == login.EmailId && EF.Functions.Collate(x.Password, "SQL_Latin1_General_CP1_CS_AS") == login.Password).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                }
                if (account != null)
                {
                    if (account.IsDeleted)
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.AccountDeleted)));
                    }
                    if (account.IsActive == false && !string.IsNullOrWhiteSpace(Convert.ToString(account.IsDeleted)))
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.Account.ActiveMessage)));
                    }
                    if (!account.IsActive)
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.Account.EmailConfirmFail)));
                    }
                    if (account.ByPostBlocked)
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.Account.ReportedBlocked)));
                    }
                    if (account.Role != 1)
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.Account.AdminLoginFail)));
                    }

                    var accountData = _mapper.Map<UsermanagementDetailsID>(account);
                    accountData.Token = "Bearer" + " " + (_tokenManger.BuildToken(account.Id)).ToString().Trim();
                    if (account.InvalidLoginAttempts == true)
                    {
                        return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.LoginAttemptsFailed)));
                    }
                    return new ExecutionResult<UsermanagementDetailsID>(accountData);
                }


                return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.Account.AdminLoginFail)));
            }

            return new ExecutionResult<UsermanagementDetailsID>(new ErrorInfo(string.Format(MessageHelper.PassParaMessage, "Email address or password")));
        }
        #endregion

        #region version check
        public ExecutionResult<Versionmodel> GetUpdatedVersion(GetVersionmodel GetVersionmodel, Versionmodel Versionmodel)
        {
            var account = _dbContext.TblVersions.Where(x => x.Type == GetVersionmodel.Type).FirstOrDefault();
            if (account != null)
            {
                Versionmodel.Ismandatory = account.Ismandatory;
                if (Convert.ToInt32(account.Version.Replace(".", "")) <= Convert.ToInt32(GetVersionmodel.Version.Replace(".", "")))
                {
                    Versionmodel.UpdateRequired = false;
                }
                else
                {
                    Versionmodel.UpdateRequired = true;
                }
                return new ExecutionResult<Versionmodel>(Versionmodel);
            }


            return new ExecutionResult<Versionmodel>(new ErrorInfo(string.Format("Please Update New Version")));
        }
        #endregion


        #region Block user request by User add and remove
        public async Task<ExecutionResult> BlockUser(BlockUserModel BlockUserModel)
        {
            if (BlockUserModel == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            var account = _dbContext.TblBlockUsers.Where(x => x.Id == BlockUserModel.Id && x.BlockId == BlockUserModel.BlockId).Any();
            if (account)
            {
                var followchange = _dbContext.TblBlockUsers.Where(x => x.Id == BlockUserModel.Id && x.BlockId == BlockUserModel.BlockId);
                foreach (var item in followchange)
                {
                    _dbContext.TblBlockUsers.Remove(item);

                }
                await _dbContext.SaveChangesAsync();
                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "UnBlock", "Requested", "")));
            }
            else
            {

                var createAcc = _mapper.Map<TblBlockUser>(BlockUserModel);
                createAcc.CreatedDate = DateTime.UtcNow;
                createAcc.ModifiedDate = DateTime.UtcNow;
                createAcc.RequestAccepted = true;
                await _dbContext.TblBlockUsers.AddAsync(createAcc);
                await _dbContext.SaveChangesAsync();
                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Block User", "Requested", "")));

            }
        }

        #endregion


        #region Change Present Live Status of Users
        public async Task<ExecutionResult> PresentLiveStatus(PresentLiveStatusModel PresentLiveStatusModel)
        {
            if (PresentLiveStatusModel == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            var Statuschange = _dbContext.Usermanagements.Where(x => x.Id == PresentLiveStatusModel.Id).FirstOrDefault();
            if (Statuschange != null)
            {
                Statuschange.PresentLiveStatus = PresentLiveStatusModel.PresentLiveStatus;
                await _dbContext.SaveChangesAsync();
            }

            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "User", "Status", "")));

        }

        #endregion


        #region Get Block user By User Id
        public async Task<ExecutionResult<PaginatedResponse<FollowUsers>>> GetBlockuser(FollowModel FollowModel)
        {
            PaginatedResponse<FollowUsers> response = new PaginatedResponse<FollowUsers>();

            var Deletesugg = _dbContext.TblBlockUsers
    .Where(o => o.Id == FollowModel.ID)
    .Select(o => o.BlockId);

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




        #region ForgetPassword
        public async Task<ExecutionResult> ForgetPassword(string EmailId)
        {
            if (!string.IsNullOrWhiteSpace(EmailId))
            {
                var account = _dbContext.Usermanagements.FirstOrDefault(x => x.Email == EmailId);
                if (account != null)
                {
                    if (account.IsActive)
                    {
                        account.EmailToken = Guid.NewGuid();
                        account.ModifiedDate = DateTime.UtcNow;

                        _dbContext.SaveChanges();

                        string link = _appSettings.Value.ForgetPassword + "/" + account.EmailToken.ToString();

                        string HTMLString = EmailHelper.ForgotPassword(account.Firstname, account.Lastname, link);
                        var isMailSent = await _commonMethods.SendMail(EmailId, HTMLString, MessageHelper.SubjectResetPassword);

                        if (!isMailSent)
                        {
                            return new ExecutionResult(new ErrorInfo(MessageHelper.EmailSentFailedWhileResetPassword, "Opps! Please Try Again"));
                        }
                        return new ExecutionResult(new InfoMessage(MessageHelper.ForgotPassword.ForgotPasswordSuccess));
                    }
                    else
                    {
                        return new ExecutionResult<LoginResponseModel>(new ErrorInfo(MessageHelper.Account.AccountNotActiveMessage));
                    }

                }
                else
                {
                    return new ExecutionResult(new ErrorInfo(MessageHelper.EmailNotRegistred));
                }
            }
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NullMessage, "Email")));
        }

        #endregion

        #region ResetPassword
        public async Task<ExecutionResult> ResetPassword(Guid EmailToken, string Password)
        {
            if (EmailToken != null)
            {
                var account = _dbContext.Usermanagements.FirstOrDefault(x => x.EmailToken.ToString().ToLower().Trim() == EmailToken.ToString().ToLower().Trim());
                if (account != null)
                {
                    if (account.IsActive)
                    {
                        if (account.Password.ToLower() == Password.ToLower())
                        {
                            return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.ResetPassword.SamePasswordError)));
                        }
                        account.Password = Password;
                        account.ModifiedDate = DateTime.UtcNow;
                        _dbContext.SaveChanges();

                        string HTMLString = EmailHelper.ResetPassword(account.Firstname, account.Lastname);
                        var isMailSent = await _commonMethods.SendMail(account.Email, HTMLString, MessageHelper.SubjectResetPassword);

                        if (!isMailSent)
                        {
                            return new ExecutionResult(new ErrorInfo(MessageHelper.EmailSentFailedWhileResetPassword, "reset"));
                        }
                        return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Password", "reset", "")));
                    }
                    else
                    {
                        return new ExecutionResult(new ErrorInfo(MessageHelper.Account.AccountNotActiveMessage));
                    }

                }
            }
            return new ExecutionResult(new InfoMessage(MessageHelper.ResetPassword.ResetPasswordFail));
        }
        #endregion

        #region Change Password
        public ExecutionResult ChangePassword(long accountId, string oldPassword, string Password)
        {
            if (accountId != 0)
            {
                var account = _dbContext.Usermanagements.FirstOrDefault(x => x.Id == accountId);
                if (account != null)
                {
                    if (account.Password == oldPassword)
                    {
                        if (account.Password != Password)
                        {
                            account.Password = Password;
                            account.ModifiedDate = DateTime.UtcNow;
                            _dbContext.SaveChanges();
                            return new ExecutionResult(new InfoMessage(MessageHelper.ChangePassword.ChangePasswordSuccess));
                        }
                        else
                        {
                            return new ExecutionResult<LoginResponseModel>(new ErrorInfo(MessageHelper.ChangePassword.NewOldPasswordSameMatch));
                        }

                    }
                    else
                    {
                        return new ExecutionResult<LoginResponseModel>(new ErrorInfo(MessageHelper.ChangePassword.PasswordNotMatch));
                    }
                }
                else
                {
                    return new ExecutionResult<LoginResponseModel>(new ErrorInfo(MessageHelper.Account.AccountNotActiveMessage));
                }
            }
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NullMessage, "Email")));
        }

        #endregion



        #region FAQ
        public async Task<ExecutionResult> InsertFAQ(FAQ FAQ)
        {
            var createfaq = _mapper.Map<Faq>(FAQ);
            await _dbContext.Faqs.AddAsync(createfaq);
            await _dbContext.SaveChangesAsync();

            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "FAQ", "added", "You will get Reply Soon")));

        }



        #region update FAQ Update
        public async Task<ExecutionResult> FAQUpdate(FAQDetails FAQDetails)
        {

            if (FAQDetails != null)
            {
                var faq = _dbContext.Faqs.FirstOrDefault(x => x.Id == FAQDetails.Id);
                if (faq != null)
                {
                    faq.Question = FAQDetails.Question;
                    faq.Answer = FAQDetails.Answer;
                    _dbContext.SaveChanges();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "FAQ", "Updated", "")));

                }
                else
                {
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "FAQ")));
                }
            }
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "FAQ")));



        }
        #endregion

        #region  FAQ Delete
        public async Task<ExecutionResult> FAQDelete(int Id)
        {

            var faq = _dbContext.Faqs.FirstOrDefault(x => x.Id == Id);
            if (faq != null)
            {
                _dbContext.Faqs.Remove(faq);
                _dbContext.SaveChanges();
                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "FAQ", "Deleted", "")));

            }
            else
            {
                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "FAQ")));
            }

            // return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "FAQ")));



        }
        #endregion
        public async Task<ExecutionResult> GetAllContactUs()
        {
            List<ContactU> AllContacts = _dbContext.ContactUs.Where(x => x.Id >= 0).OrderByDescending(x => x.Id).ToList();
            return AllContacts.Count > 0 ? new ExecutionResult<List<ContactUs>>(_mapper.Map<List<ContactUs>>(AllContacts)) :
               new ExecutionResult<List<ContactUs>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "ContactUs")));
        }
        public async Task<ExecutionResult> Contactus(ContactUs contactUs)
        {
            if (contactUs != null)
            {
                var contact = _mapper.Map<ContactU>(contactUs);
                await _dbContext.ContactUs.AddAsync(contact);
                await _dbContext.SaveChangesAsync();
                var isMailSent = await _commonMethods.SendMail(contactUs.Email, "Enquery Submitted successfully You will get Reply Soon", MessageHelper.Account.Enquery);
                if (!isMailSent)
                {
                    return new ExecutionResult(new ErrorInfo(MessageHelper.Account.EmailSentFailedWhileEnquery));
                }
                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Enquery", "Submitted", "You will get Reply Soon")));
            }
            else
            {
                return new ExecutionResult(new InfoMessage(MessageHelper.Account.EnquerySubmitionFail));
            }
        }

        public async Task<ExecutionResult<List<FAQDetails>>> GetAllFAQ()
        {
            List<Faq> AllFAQ = _dbContext.Faqs.Where(x => x.Id >= 0).OrderBy(x => x.Id).ToList();
            //List<FaqAnswer> FAQanswer = _dbContext.FaqAnswers.Where(x => x.Id >= 0).OrderBy(x => x.Id).ToList();


            return AllFAQ.Count > 0 ? new ExecutionResult<List<FAQDetails>>(_mapper.Map<List<FAQDetails>>(AllFAQ)) :
               new ExecutionResult<List<FAQDetails>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "FAQ")));
        }


        #endregion



        #region Spotify


        public async Task<ExecutionResult> Spotify(SpotifyModel SpotifyModel)
        {
            try
            {
                if (SpotifyModel == null)
                {
                    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
                }
                var Artists = _dbContext.TblArtists.Where(x => x.Id == SpotifyModel.Id).ToList();
                if (Artists != null)
                {
                    foreach (var item in Artists)
                    {
                        _dbContext.TblArtists.Remove(item);
                        //_dbContext.SaveChanges();
                    }
                }
                if (SpotifyModel.Artists != null)
                {
                    foreach (var item in SpotifyModel.Artists)
                    {
                        var createAcc = _mapper.Map<TblArtist>(item);
                        await _dbContext.TblArtists.AddAsync(createAcc);
                        // await _dbContext.SaveChangesAsync();

                    }
                }



                var Genres = _dbContext.TblGenres.Where(x => x.Id == SpotifyModel.Id).ToList();
                if (Genres != null)
                {
                    foreach (var item in Genres)
                    {
                        _dbContext.TblGenres.Remove(item);
                        //_dbContext.SaveChanges();
                    }
                }

                if (SpotifyModel.Genres != null)
                {
                    foreach (var item in SpotifyModel.Genres)
                    {
                        var createAcc = _mapper.Map<TblGenre>(item);
                        await _dbContext.TblGenres.AddAsync(createAcc);
                        //await _dbContext.SaveChangesAsync();

                    }
                }
                #region Save Fav Events
                var Events = _dbContext.TblEventfeeds.Where(x => x.Id == SpotifyModel.Id).ToList();
                var NewFavourite = SpotifyModel.FavouriteEvents.Select(x => x.EventId);
                var PrevFavourite = Events.Select(o => o.EventId);
                Events.ForEach(x => x.Favorite = false);
                await _dbContext.SaveChangesAsync();
                if (Events != null)
                {

                    var avlableFeeds = _dbContext.TblEventfeeds.Where(x => x.Id == SpotifyModel.Id && NewFavourite.Contains(x.EventId)).ToList();
                    avlableFeeds.ForEach(x => x.Favorite = true);
                    await _dbContext.SaveChangesAsync();


                }
                if (NewFavourite != null)
                {
                    foreach (var item in SpotifyModel.FavouriteEvents)
                    {

                        if (!PrevFavourite.Contains(item.EventId))
                        {
                            TblEventfeed Eventfeed = new TblEventfeed();
                            Eventfeed.Id = SpotifyModel.Id;
                            Eventfeed.EventId = item.EventId;
                            Eventfeed.Golive = false;
                            Eventfeed.Interest = false;
                            Eventfeed.Favorite = true;
                            await _dbContext.TblEventfeeds.AddAsync(Eventfeed);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }
                #endregion


                var nextevent = _dbContext.TblEventfeeds.Where(x => x.Id == SpotifyModel.Id).ToList();
                var NewEventId = SpotifyModel.NextEvents.Select(o => o.EventId);
                var PrevEventId = nextevent.Select(o => o.EventId);
                if (nextevent != null)
                {
                    nextevent.ForEach(a => a.Golive = false);
                    await _dbContext.SaveChangesAsync();
                    var eventfeedupdateTrue = _dbContext.TblEventfeeds.Where(x => x.Id == SpotifyModel.Id && NewEventId.Contains(x.EventId)).ToList();
                    eventfeedupdateTrue.ForEach(a => a.Golive = true);
                    await _dbContext.SaveChangesAsync();
                }

                if (NewEventId != null)
                {
                    foreach (var item in SpotifyModel.NextEvents)
                    {

                        if (!PrevEventId.Contains(item.EventId))
                        {
                            if (SpotifyModel.Id > 0)
                            {
                                TblEventfeed Eventfeed = new TblEventfeed();
                                Eventfeed.Id = SpotifyModel.Id;
                                Eventfeed.EventId = item.EventId;
                                Eventfeed.Golive = true;
                                Eventfeed.Interest = false;
                                Eventfeed.Favorite = false;
                                await _dbContext.TblEventfeeds.AddAsync(Eventfeed);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }





                //if (SpotifyModel.FavouriteEvents != null)
                //{
                //    foreach (var item in SpotifyModel.FavouriteEvents)
                //    {
                //        var createAcc = _mapper.Map<TblFavouriteEvent>(item);
                //        await _dbContext.TblFavouriteEvents.AddAsync(createAcc);
                //        // await _dbContext.SaveChangesAsync();

                //    }
                //}


                await _dbContext.SaveChangesAsync();

                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Spotify", "Saved", "please check")));

            }
            catch (Exception ex)
            {

                return new ExecutionResult(new ErrorInfo(string.Format(ex.ToString())));
            }
        }


        public async Task<ExecutionResult<GetSpotifyModel>> GetSpotify(GetSpotify GetSpotify)
        {
            try
            {
                GetSpotifyModel SpotifyModel = new GetSpotifyModel();
                SpotifyModel.Id = GetSpotify.Id;

                var Artists = _dbContext.TblArtists.Where(x => x.Id == GetSpotify.Id).ToList();
                SpotifyModel.Artists = _mapper.Map<List<Artists>>(Artists);


                var EventsIds = _dbContext.TblEventfeeds
            .Where(x => x.Id == GetSpotify.Id && x.Favorite == true)
            .Select(o => o.EventId);

                List<EventModel> AllEvent = new List<EventModel>();
                var AllEvents = _dbContext.TblEvents
               .Where(x => EventsIds.Contains(x.EventId))
               .OrderByDescending(x => x.CreatedDate).ToList();

                AllEvent = _mapper.Map<List<EventModel>>(AllEvents);
                foreach (var item in AllEvent)
                {
                    var Images = _dbContext.TblEventUserFiles
                    .Where(x => x.EventId == item.EventId).ToList();
                    item.EventImages = _mapper.Map<List<TblEventFile>>(Images);
                }
                SpotifyModel.FavouriteEvents = AllEvent;



                var Genres = _dbContext.TblGenres.Where(x => x.Id == GetSpotify.Id).ToList();
                SpotifyModel.Genres = _mapper.Map<List<Genres>>(Genres);

                return GetSpotify.Id > 0 ? new ExecutionResult<GetSpotifyModel>(SpotifyModel) :
               new ExecutionResult<GetSpotifyModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Spotify")));

                // return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Spotify", "Saved", "please check")));

            }
            catch (Exception ex)
            {

                return new ExecutionResult<GetSpotifyModel>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong, "Spotify", "Not Found", "please check")));
            }
        }


        public async Task<ExecutionResult<GetSpotifyModel>> GetTopSpotify()
        {
            try
            {
                GetSpotifyModel SpotifyModel = new GetSpotifyModel();
                //SpotifyModel.Id = GetSpotify.Id;

                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "sp_top5_Artists";


                    var table = new DataTable();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        // this will query your database and return the result to your datatable
                        DataSet ds = new DataSet();

                        da.Fill(table);
                        
                        var Artists = Constants.ConvertDataTable<Artists>(table).ToList();
                        SpotifyModel.Artists = Artists;
                    }
                }

                //var Artists = _dbContext.TblArtists.Skip((0) * 5)
                //                                .Take(5).ToList();




                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "sp_top5_Genres";


                    var table = new DataTable();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        // this will query your database and return the result to your datatable
                        DataSet ds = new DataSet();

                        da.Fill(table);

                        var Genres = Constants.ConvertDataTable<Genres>(table).ToList();
                        SpotifyModel.Genres = Genres;
                    }
                }


                //var Genres = _dbContext.TblGenres.Skip((0) * 5)
                //                                .Take(5).ToList();
                //SpotifyModel.Genres = _mapper.Map<List<Genres>>(Genres);

                return SpotifyModel != null ? new ExecutionResult<GetSpotifyModel>(SpotifyModel) :
               new ExecutionResult<GetSpotifyModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Spotify")));

                // return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Spotify", "Saved", "please check")));

            }
            catch (Exception ex)
            {

                return new ExecutionResult<GetSpotifyModel>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong, "Spotify", "Not Found", "please check")));
            }
        }



        #endregion


        public async Task<ExecutionResult> FavSongs(Spotifysongs spotifysongs)
        {
            try
            {
                if (spotifysongs == null)
                {
                    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
                }

                var Songdelete = _dbContext.TblSongs.Where(x => x.Id == spotifysongs.Id).ToList();
                if (Songdelete != null)
                {
                    foreach (var item in Songdelete)
                    {
                        _dbContext.TblSongs.Remove(item);
                        //_dbContext.SaveChanges();
                    }
                }

                foreach (var item in spotifysongs.Songs)
                {

                    var createAcc = _mapper.Map<TblSong>(item);
                    await _dbContext.TblSongs.AddAsync(createAcc);
                    await _dbContext.SaveChangesAsync();



                }
                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Favourite Songs", "Saved", "please check")));


            }
            catch (Exception ex)
            {

                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SomethingWrong, "Favourite Songs", "Not Saved", "please check")));
            }
        }


        public async Task<ExecutionResult<List<Songs>>> GetFavSongs(GetSpotify GetSpotify)
        {
            try
            {
                SpotifyModel SpotifyModel = new SpotifyModel();
                SpotifyModel.Id = GetSpotify.Id;
                var songsDetails = _dbContext.TblSongs.Where(x => x.Id == GetSpotify.Id).ToList();

                return GetSpotify.Id > 0 ? new ExecutionResult<List<Songs>>(_mapper.Map<List<Songs>>(songsDetails)) :
               new ExecutionResult<List<Songs>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Spotify Songs")));

                // return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Spotify", "Saved", "please check")));

            }
            catch (Exception ex)
            {

                return new ExecutionResult<List<Songs>>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong, "Spotify Songs", "Not Found", "please check")));
            }
        }

        public async Task<ExecutionResult<List<Songs>>> GetTopSongs()
        {
            try
            {
                SpotifyModel SpotifyModel = new SpotifyModel();
                //SpotifyModel.Id = GetSpotify.Id;

                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "sp_top5_songs";


                    var table = new DataTable();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        // this will query your database and return the result to your datatable
                        DataSet ds = new DataSet();

                        da.Fill(table);
                        List<Songs> result = new List<Songs>();
                        var songsDetails = Constants.ConvertDataTable<Songs>(table).ToList();
                        return songsDetails != null ? new ExecutionResult<List<Songs>>(_mapper.Map<List<Songs>>(songsDetails)) :
              new ExecutionResult<List<Songs>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Spotify Songs")));

                    }
                }

                //var songsDetails = _dbContext.TblSongs.Skip((0) * 5)
                //                        .Take(5).ToList();


                // return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Spotify", "Saved", "please check")));

            }
            catch (Exception ex)
            {

                return new ExecutionResult<List<Songs>>(new ErrorInfo(string.Format(MessageHelper.SomethingWrong, "Spotify Songs", "Not Found", "please check")));
            }
        }



        public async Task<ExecutionResult> BusinessUserReq(BusinessModel BusinessModel)
        {
            try
            {
                if (BusinessModel == null)
                {
                    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
                }
                var businessdetails = _dbContext.TblBusinesses.Where(x => x.BusinessId == BusinessModel.BusinessId).FirstOrDefault();
                if (businessdetails != null)
                {
                    businessdetails.Id = BusinessModel.Id;
                    businessdetails.ComumuunicationEmailId = BusinessModel.ComumuunicationEmailId;
                    businessdetails.ComumuunicationPhone = BusinessModel.ComumuunicationEmailId;
                    businessdetails.Designation = BusinessModel.ComumuunicationEmailId;
                    businessdetails.OrganizationName = BusinessModel.ComumuunicationEmailId;
                    businessdetails.OrganizationAddress = BusinessModel.ComumuunicationEmailId;
                    businessdetails.LegalName = BusinessModel.LegalName;
                    //account.Image = userProfile.Image;
                    _dbContext.SaveChanges();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Business User", "Updated", "")));

                }
                else
                {
                    var createAcc = _mapper.Map<TblBusiness>(BusinessModel);
                    await _dbContext.TblBusinesses.AddAsync(createAcc);
                    await _dbContext.SaveChangesAsync();

                }

                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Business User", "Saved", "please check")));

            }
            catch (Exception ex)
            {

                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SomethingWrong, "Business User", "Not Saved", "please check")));
            }
        }




        public async Task<ExecutionResult<GetBusinessModel>> GetProfileVerifyReq(long Id)
        {
            try
            {

                GetBusinessModel getBusinessModel = new GetBusinessModel();
                getBusinessModel.BusinessModel = new BusinessModel();
                getBusinessModel.VerifyProfile = new VerifyProfile();

                var businessdetails = _dbContext.TblBusinesses.Where(x => x.Id == Id).FirstOrDefault();

                getBusinessModel.BusinessModel = _mapper.Map<BusinessModel>(businessdetails);

                var VerifyProfile = _dbContext.TblProfiles.Where(x => x.Id == Id).FirstOrDefault();

                getBusinessModel.VerifyProfile = _mapper.Map<VerifyProfile>(VerifyProfile);


                return getBusinessModel != null ? new ExecutionResult<GetBusinessModel>(getBusinessModel) :
               new ExecutionResult<GetBusinessModel>(new ErrorInfo(string.Format(MessageHelper.NoFound)));

            }
            catch (Exception ex)
            {

                return new ExecutionResult<GetBusinessModel>(new ErrorInfo(string.Format(ex.ToString())));
            }
        }


        public async Task<ExecutionResult> ProfileVerifyReq(VerifyProfile VerifyProfile)
        {
            try
            {
                if (VerifyProfile == null)
                {
                    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
                }
                var VerifyProfiledetails = _dbContext.TblProfiles.Where(x => x.ProfileId == VerifyProfile.ProfileId).FirstOrDefault();
                if (VerifyProfiledetails != null)
                {
                    VerifyProfiledetails.Id = VerifyProfile.Id;
                    //VerifyProfiledetails.ProfileId = VerifyProfile.Id;
                    VerifyProfiledetails.UserName = VerifyProfile.UserName;
                    VerifyProfiledetails.FullName = VerifyProfile.FullName;
                    VerifyProfiledetails.AdditionalInformation = VerifyProfile.AdditionalInformation;
                    //account.Image = userProfile.Image;
                    _dbContext.SaveChanges();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "User Verified", "Updated", "")));

                }
                else
                {
                    var createAcc = _mapper.Map<TblProfile>(VerifyProfile);
                    await _dbContext.TblProfiles.AddAsync(createAcc);
                    await _dbContext.SaveChangesAsync();

                }

                return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Profile", "Saved", "please check")));

            }
            catch (Exception ex)
            {

                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.SomethingWrong, "Profile", "Not Saved", "please check")));
            }
        }


        /// <summary>
        /// Used to get all the posts Admin 
        /// </summary>
        /// <param name="userId">User's unique indentification number</param>
        /// <returns>User posts</returns>
        public async Task<ExecutionResult<List<AdminUserPostsModel>>> AdminGetUserPosts()
        {
            List<AdminUserPostsModel> lstUserPosts = new List<AdminUserPostsModel>();


            lstUserPosts = _mapper.Map<List<AdminUserPostsModel>>(_dbContext.VAdminPostDetails
                .OrderByDescending(x => x.PostId).ToList());

            return lstUserPosts.Any() ? new ExecutionResult<List<AdminUserPostsModel>>(lstUserPosts) :
               new ExecutionResult<List<AdminUserPostsModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "User Post")));
        }


        public async Task<ExecutionResult<List<AdminUserPostsModel>>> AdminGetPostByID(long PostId)
        {
            List<AdminUserPostsModel> lstUserPosts = new List<AdminUserPostsModel>();


            lstUserPosts = _mapper.Map<List<AdminUserPostsModel>>(_dbContext.VAdminPostDetails.Where(x => x.PostId == PostId).ToList());

            foreach (var item in lstUserPosts)
            {
                var userids = _dbContext.TblBlockPosts.Where(x => x.PostId == item.PostId).Select(o => o.Id).ToList();
                var AllUsers = _dbContext.Usermanagements.Where(x => userids.Contains(x.Id))
              .OrderBy(x => x.Username).ToList();
                item.BlockedUsers = _mapper.Map<List<FollowUsers>>(AllUsers);

                item.PostMediaLinks = _mapper.Map<List<PostMediaModel>>(_dbContext.TblPostFiles.Where(x => x.PostId == item.PostId).ToList());

            }

            return lstUserPosts.Any() ? new ExecutionResult<List<AdminUserPostsModel>>(lstUserPosts) :
               new ExecutionResult<List<AdminUserPostsModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "User Post")));
        }


        #region Event Active Change by admin

        public async Task<ExecutionResult> EventActive(long EventId, bool status)
        {
            if (EventId != null)
            {
                var account = _dbContext.TblEvents.FirstOrDefault(x => x.EventId == EventId);
                if (account != null)
                {

                    account.IsActive = Convert.ToInt32(status);

                    _dbContext.SaveChanges();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Event", "Updated", "")));

                }
                else
                {
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "Event")));
                }
            }
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "Event")));

        }



        #endregion
    }
}