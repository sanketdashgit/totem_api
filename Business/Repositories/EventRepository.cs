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
using Totem.Business.Core.DataTransferModels.WebsiteDesign;
using Totem.Business.Core.Consts;
using Totem.Business.Core.DataTransferModels.Event;
using Totem.Business.Core.DataTransferModels.Documents;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Totem.Database.StoreProcedure;
using System.IO;

namespace Totem.Business.Repositories
{
    public class EventRepository : IEventRepository
    {
        #region Constructor

        private readonly TotemDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly string _connectionString;
        private readonly IAmazonS3Repository _amazonRepo;
        private readonly INotificationRepository _NotificationRepo;
        private readonly TokenManager _tokenManger;
        private readonly CommonMethods _commonMethods;
        private readonly IOptions<AppSettings> _appSettings;

        public EventRepository(
            TotemDBContext dbContext,
            IMapper mapper,
            TokenManager tokenManger,
            IOptions<AppSettings> appSettings,
            CommonMethods commonMethods, IAmazonS3Repository amazonRepo, INotificationRepository NotificationRepo)
        {
            _commonMethods = commonMethods;
            _appSettings = appSettings;
            _tokenManger = tokenManger;
            _amazonRepo = amazonRepo;
            _mapper = mapper;
            _dbContext = dbContext;
            _connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            _NotificationRepo = NotificationRepo;
        }

        #endregion


        #region Create Event

        public async Task<ExecutionResult<TblEventModel>> CreateEvent(TblEventModel TblEventModel)
        {
            if (TblEventModel == null)
            {
                return new ExecutionResult<TblEventModel>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var createAcc = _mapper.Map<TblEvent>(TblEventModel);

            TblEvent AllEvents = _dbContext.TblEvents
               .Where(x => x.EventId == createAcc.EventId).FirstOrDefault();

            if (AllEvents != null)
            {
                AllEvents.Id = createAcc.Id;
                AllEvents.EventName = TblEventModel.EventName;
                AllEvents.StartDate = TblEventModel.StartDate;
                AllEvents.EndDate = TblEventModel.EndDate;
                AllEvents.Address = TblEventModel.Address;
                AllEvents.Details = TblEventModel.Details;
                AllEvents.Latitude = TblEventModel.Latitude;
                AllEvents.Longitude = TblEventModel.Longitude;
                _dbContext.SaveChanges();
            }
            else
            {
                createAcc.IsActive = _dbContext.Usermanagements
            .Where(o => o.Id == TblEventModel.Id)
            .Select(o => o.Role).FirstOrDefault();
                await _dbContext.TblEvents.AddAsync(createAcc);
                await _dbContext.SaveChangesAsync();
                if (createAcc.IsActive != 0 && createAcc.IsActive != null)
                    Senteventnotification(createAcc);
            }


            return createAcc.EventId > 0 ? new ExecutionResult<TblEventModel>(_mapper.Map<TblEventModel>(createAcc)) :
               new ExecutionResult<TblEventModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event")));

        }


        public async Task<ExecutionResult<TblEventModel>> CreateEventwithvanueId(TblEventwithvanueIdModel TblEventModel)
        {
            if (TblEventModel == null)
            {
                return new ExecutionResult<TblEventModel>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var createAcc = _mapper.Map<TblEvent>(TblEventModel);
            createAcc.Id = 2;// this id fixed to save event from Different Api
            createAcc.Details = "";
            TblEvent AllEvents = _dbContext.TblEvents
               .Where(x => x.VanueId == createAcc.VanueId && x.StartDate == createAcc.StartDate && x.EventName == createAcc.EventName).FirstOrDefault();

            if (AllEvents != null)
            {
                AllEvents.Id = createAcc.Id; 
                AllEvents.EventName = TblEventModel.EventName;
                AllEvents.StartDate = TblEventModel.StartDate;
                AllEvents.Details = "";
                AllEvents.Address = TblEventModel.Address;               
                AllEvents.Latitude = TblEventModel.Latitude;
                AllEvents.Longitude = TblEventModel.Longitude;
                await _dbContext.SaveChangesAsync();
                createAcc = AllEvents;

            }
            else
            {
                createAcc.IsActive = 1;
                createAcc.EndDate = createAcc.StartDate;
                await _dbContext.TblEvents.AddAsync(createAcc);
                await _dbContext.SaveChangesAsync();

                TblEventUserFile file = new TblEventUserFile();
                file.FileName = TblEventModel.EventName;
                file.FileType = "Cover";
                file.Title = TblEventModel.EventName;
                file.EventId = createAcc.EventId;
                file.Downloadlink = TblEventModel.Image;
                file.CreatedDate = DateTime.UtcNow;
                await _dbContext.TblEventUserFiles.AddAsync(file);
                await _dbContext.SaveChangesAsync();
            }
           

            return createAcc.EventId > 0 ? new ExecutionResult<TblEventModel>(_mapper.Map<TblEventModel>(createAcc)) :
               new ExecutionResult<TblEventModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event")));

        }


        public async Task<ExecutionResult<TblEventByPostModel>> CreateEventByPost(TblEventByPostModel TblEventModel)
        {
            if (TblEventModel == null)
            {
                return new ExecutionResult<TblEventByPostModel>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var createAcc = _mapper.Map<TblEvent>(TblEventModel);
            createAcc.IsActive = 3;
            await _dbContext.TblEvents.AddAsync(createAcc);
            await _dbContext.SaveChangesAsync();




            return createAcc.EventId > 0 ? new ExecutionResult<TblEventByPostModel>(_mapper.Map<TblEventByPostModel>(createAcc)) :
               new ExecutionResult<TblEventByPostModel>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event")));

        }



        #endregion

        #region Senteventnotification

        async Task<int> sendNotification(TblEvent TblEvent, List<GetAllUserStatuswithNotification> listusers)
        {

            var sentnoty = listusers.Where(x => x.EventNotification == true).Select(x => x.Id).ToList();
            var fcmdetails = _dbContext.TblUserFcms.Where(x => sentnoty.Contains(x.Id) && x.Login == true).ToList();
            SendNotification sendNotification = new SendNotification();
            sendNotification.data = new data();
            sendNotification.notification = new notification();
            sendNotification.registration_ids = new List<string>();
            sendNotification.notification.title = TblEvent.EventName;
            sendNotification.notification.body = "New Event Created" + " in " + TblEvent.Address;
            sendNotification.notification.image = "";
            sendNotification.data.conversationId = TblEvent.Id;
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
                tblNotification.Id = TblEvent.Id;
                tblNotification.Image = "";
                tblNotification.Readflag = "0";
                tblNotification.Title = TblEvent.EventName;
                tblNotification.NotificationType = "EVENT";
                tblNotification.NotificationTypeId = TblEvent.EventId;
                tblNotification.Descp = "New Event Created" + " in " + TblEvent.Address;
                await _dbContext.TblNotifications.AddAsync(tblNotification);
                await _dbContext.SaveChangesAsync();
            }

            return 1;
            //var  Prop = await Task.Run(() => GetSomething());
        }
        public async void Senteventnotification(TblEvent TblEvent)
        {

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "sp_EventNotification";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@orig_lat", Convert.ToDecimal(TblEvent.Latitude));
                    command.Parameters.AddWithValue("@orig_lon", Convert.ToDecimal(TblEvent.Longitude));

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable
                    DataSet ds = new DataSet();

                    da.Fill(table);

                    List<GetAllUserStatuswithNotification> TableVEventDetai = Constants.ConvertDataTable<GetAllUserStatuswithNotification>(table).ToList();

                    int Prop = await Task.Run(() => sendNotification(TblEvent, TableVEventDetai));


                }
            }

        }
        #endregion


        #region EventFileDetailId
        public ExecutionResult<VEventDetai> GetByEventID(GetEvent GetEvent)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "Sp_EventByID";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EventId", GetEvent.EventId);
                    command.Parameters.AddWithValue("@Id", GetEvent.UserID);
                    command.Parameters.AddWithValue("@Search", "");
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable
                    DataSet ds = new DataSet();

                    da.Fill(table);

                    Sp_GetEvent result = new Sp_GetEvent();

                    VEventDetai EventDetai = Constants.ConvertDataTable<VEventDetai>(table).FirstOrDefault();
                    if (EventDetai != null)
                    {
                        var Images = _dbContext.TblEventUserFiles
                   .Where(x => x.EventId == EventDetai.EventId).ToList();
                        EventDetai.EventImages = _mapper.Map<List<TblEventFile>>(Images);

                        EventDetai.FavoriteCount = _dbContext.TblEventfeeds
                    .Where(x => x.EventId == GetEvent.EventId && x.Favorite == true).Count();

                        EventDetai.GoliveCount = _dbContext.TblEventfeeds
                           .Where(x => x.EventId == GetEvent.EventId && x.Golive == true).Count();

                        EventDetai.InterestCount = _dbContext.TblEventfeeds
                           .Where(x => x.EventId == GetEvent.EventId && x.Interest == true).Count();

                    }
                    return EventDetai != null ? new ExecutionResult<VEventDetai>(EventDetai) :
                new ExecutionResult<VEventDetai>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event is not Live or ")));
                }
            }



        }
        #endregion





        #region GetAllEvent
        public ExecutionResult<PaginatedResponse<VEventDetai>> GetAllEvent(GetAllEvent GetAllEvent)
        {

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "Sp_EventByID";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EventId", GetAllEvent.EventId);
                    command.Parameters.AddWithValue("@Id", GetAllEvent.UserID);
                    command.Parameters.AddWithValue("@Search", GetAllEvent.Search);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable
                    DataSet ds = new DataSet();

                    da.Fill(table);

                    //Sp_GetEvent result = new Sp_GetEvent();

                    PaginatedResponse<VEventDetai> EventDetai = new PaginatedResponse<VEventDetai>();
                    List<VEventDetai> TableVEventDetai = Constants.ConvertDataTable<VEventDetai>(table)
                        .Where(x => (x.IsActive == 2 || x.IsActive == 1 || x.Id == GetAllEvent.UserID)).ToList();
                    EventDetai.Data = TableVEventDetai
                        .Skip((GetAllEvent.PageNumber - 1) * GetAllEvent.PageSize)
                       .Take(GetAllEvent.PageSize)
                        .ToList();

                    //EventDetai.Data.Shuffle();
                    if (EventDetai != null)
                    {
                        foreach (var item in EventDetai.Data)
                        {
                            var Images = _dbContext.TblEventUserFiles
                   .Where(x => x.EventId == item.EventId).ToList();

                            item.Golive = _dbContext.TblEventfeeds
                        .Where(x => x.EventId == item.EventId && x.Id == GetAllEvent.UserID && x.Golive == true).Any();

                            item.Interest = _dbContext.TblEventfeeds
                       .Where(x => x.EventId == item.EventId && x.Id == GetAllEvent.UserID && x.Interest == true).Any();

                            item.Favorite = _dbContext.TblEventfeeds
                       .Where(x => x.EventId == item.EventId && x.Id == GetAllEvent.UserID && x.Favorite == true).Any();

                            item.EventImages = _mapper.Map<List<TblEventFile>>(Images);

                            item.FavoriteCount = _dbContext.TblEventfeeds
                        .Where(x => x.EventId == item.EventId && x.Favorite == true).Count();

                            item.GoliveCount = _dbContext.TblEventfeeds
                               .Where(x => x.EventId == item.EventId && x.Golive == true).Count();

                            item.InterestCount = _dbContext.TblEventfeeds
                               .Where(x => x.EventId == item.EventId && x.Interest == true).Count();
                        }


                    }

                    EventDetai.PageNumber = GetAllEvent.PageNumber;
                    EventDetai.PageSize = GetAllEvent.PageSize;
                    EventDetai.TotalRecords = GetAllEvent.TotalRecords > 0 ? GetAllEvent.TotalRecords :
                                           TableVEventDetai.Count;
                    return EventDetai != null ? new ExecutionResult<PaginatedResponse<VEventDetai>>(EventDetai) :
                new ExecutionResult<PaginatedResponse<VEventDetai>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event is not Live or ")));
                }
            }



            //PaginatedResponse<EventModel> AllEvent = new PaginatedResponse<EventModel>();
            //var AllEvents = _dbContext.TblEvents
            //    .Where(x => (x.IsActive == 2 || x.IsActive == 1) && x.EventName.Contains(GetAllEvent.Search))
            //    .Skip((GetAllEvent.PageNumber - 1) * GetAllEvent.PageSize)
            //                                    .Take(GetAllEvent.PageSize)
            //    .OrderByDescending(x => x.CreatedDate).ToList();

            //AllEvent.Data = _mapper.Map<List<EventModel>>(AllEvents);
            //foreach (var item in AllEvent.Data)
            //{
            //    var Images = _dbContext.TblEventUserFiles
            //    .Where(x => x.EventId == item.EventId).ToList();
            //    item.EventImages = _mapper.Map<List<TblEventFile>>(Images);
            //}
            //AllEvent.PageNumber = GetAllEvent.PageNumber;
            //AllEvent.PageSize = GetAllEvent.PageSize;
            //AllEvent.TotalRecords = GetAllEvent.TotalRecords > 0 ? GetAllEvent.TotalRecords :
            //    _dbContext.TblEvents
            //    .Where(x => (x.IsActive == 2 || x.IsActive == 1) && x.EventName.Contains(GetAllEvent.Search)).Count();
            //return AllEvent.Data.Count > 0 ? new ExecutionResult<PaginatedResponse<EventModel>>(AllEvent) :
            //   new ExecutionResult<PaginatedResponse<EventModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Events")));

        }


        public ExecutionResult<PaginatedResponse<VEventDetai>> GetAllUpcomingEvent(GetAllEvent GetAllEvent)
        {

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "Sp_EventUpcomingStartDate";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EventId", GetAllEvent.EventId);
                    command.Parameters.AddWithValue("@Id", GetAllEvent.UserID);
                    command.Parameters.AddWithValue("@Search", GetAllEvent.Search);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable
                    DataSet ds = new DataSet();

                    da.Fill(table);

                    //Sp_GetEvent result = new Sp_GetEvent();

                    PaginatedResponse<VEventDetai> EventDetai = new PaginatedResponse<VEventDetai>();
                    List<VEventDetai> TableVEventDetai = Constants.ConvertDataTable<VEventDetai>(table)
                        .Where(x => (x.IsActive == 2 || x.IsActive == 1 || x.Id == GetAllEvent.UserID)).ToList();
                    EventDetai.Data = TableVEventDetai
                        .Skip((GetAllEvent.PageNumber - 1) * GetAllEvent.PageSize)
                       .Take(GetAllEvent.PageSize)
                        .ToList();

                    //EventDetai.Data.Shuffle();
                    if (EventDetai != null)
                    {
                        foreach (var item in EventDetai.Data)
                        {
                            var Images = _dbContext.TblEventUserFiles
                   .Where(x => x.EventId == item.EventId).ToList();

                            item.Golive = _dbContext.TblEventfeeds
                        .Where(x => x.EventId == item.EventId && x.Id == GetAllEvent.UserID && x.Golive == true).Any();

                            item.Interest = _dbContext.TblEventfeeds
                       .Where(x => x.EventId == item.EventId && x.Id == GetAllEvent.UserID && x.Interest == true).Any();

                            item.Favorite = _dbContext.TblEventfeeds
                       .Where(x => x.EventId == item.EventId && x.Id == GetAllEvent.UserID && x.Favorite == true).Any();

                            item.EventImages = _mapper.Map<List<TblEventFile>>(Images);

                            item.FavoriteCount = _dbContext.TblEventfeeds
                        .Where(x => x.EventId == item.EventId && x.Favorite == true).Count();

                            item.GoliveCount = _dbContext.TblEventfeeds
                               .Where(x => x.EventId == item.EventId && x.Golive == true).Count();

                            item.InterestCount = _dbContext.TblEventfeeds
                               .Where(x => x.EventId == item.EventId && x.Interest == true).Count();
                        }


                    }

                    EventDetai.PageNumber = GetAllEvent.PageNumber;
                    EventDetai.PageSize = GetAllEvent.PageSize;
                    EventDetai.TotalRecords = GetAllEvent.TotalRecords > 0 ? GetAllEvent.TotalRecords :
                                           TableVEventDetai.Count;
                    return EventDetai != null ? new ExecutionResult<PaginatedResponse<VEventDetai>>(EventDetai) :
                new ExecutionResult<PaginatedResponse<VEventDetai>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event is not Live or ")));
                }
            }

        }

        public ExecutionResult<PaginatedResponse<VEventDetai>> GetAllByPostEvent(GetAllEvent GetAllEvent)
        {

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "Sp_EventByID";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EventId", GetAllEvent.EventId);
                    command.Parameters.AddWithValue("@Id", GetAllEvent.UserID);
                    command.Parameters.AddWithValue("@Search", GetAllEvent.Search);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable
                    DataSet ds = new DataSet();

                    da.Fill(table);

                    //Sp_GetEvent result = new Sp_GetEvent();

                    PaginatedResponse<VEventDetai> EventDetai = new PaginatedResponse<VEventDetai>();
                    List<VEventDetai> TableVEventDetai = Constants.ConvertDataTable<VEventDetai>(table)
                        .Where(x => (x.IsActive == 2 || x.IsActive == 1 || x.IsActive == 3 || x.Id == GetAllEvent.UserID)).ToList();
                    EventDetai.Data = TableVEventDetai
                        .Skip((GetAllEvent.PageNumber - 1) * GetAllEvent.PageSize)
                       .Take(GetAllEvent.PageSize)
                        .ToList();

                    //EventDetai.Data.Shuffle();
                    if (EventDetai != null)
                    {
                        foreach (var item in EventDetai.Data)
                        {
                            var Images = _dbContext.TblEventUserFiles
                   .Where(x => x.EventId == item.EventId).ToList();

                            item.Golive = _dbContext.TblEventfeeds
                        .Where(x => x.EventId == item.EventId && x.Id == GetAllEvent.UserID && x.Golive == true).Any();

                            item.Interest = _dbContext.TblEventfeeds
                       .Where(x => x.EventId == item.EventId && x.Id == GetAllEvent.UserID && x.Interest == true).Any();

                            item.Favorite = _dbContext.TblEventfeeds
                       .Where(x => x.EventId == item.EventId && x.Id == GetAllEvent.UserID && x.Favorite == true).Any();

                            item.EventImages = _mapper.Map<List<TblEventFile>>(Images);

                            item.FavoriteCount = _dbContext.TblEventfeeds
                        .Where(x => x.EventId == item.EventId && x.Favorite == true).Count();

                            item.GoliveCount = _dbContext.TblEventfeeds
                               .Where(x => x.EventId == item.EventId && x.Golive == true).Count();

                            item.InterestCount = _dbContext.TblEventfeeds
                               .Where(x => x.EventId == item.EventId && x.Interest == true).Count();
                        }


                    }

                    EventDetai.PageNumber = GetAllEvent.PageNumber;
                    EventDetai.PageSize = GetAllEvent.PageSize;
                    EventDetai.TotalRecords = GetAllEvent.TotalRecords > 0 ? GetAllEvent.TotalRecords :
                                           TableVEventDetai.Count;
                    return EventDetai != null ? new ExecutionResult<PaginatedResponse<VEventDetai>>(EventDetai) :
                new ExecutionResult<PaginatedResponse<VEventDetai>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event is not Live or ")));
                }
            }



         

        }



        #endregion



        #region GoNextEvent
        public ExecutionResult<PaginatedResponse<VEventDetai>> GoNextEvent(GetAllEvent GetAllEvent)
        {

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "Sp_GoinEventByuserID";


                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@EventId", GetAllEvent.EventId);
                    command.Parameters.AddWithValue("@Id", GetAllEvent.UserID);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable
                    DataSet ds = new DataSet();

                    da.Fill(table);

                    //Sp_GetEvent result = new Sp_GetEvent();

                    PaginatedResponse<VEventDetai> EventDetai = new PaginatedResponse<VEventDetai>();
                    List<VEventDetai> TableVEventDetai = Constants.ConvertDataTable<VEventDetai>(table)
                        .Where(x => (x.IsActive == 2 || x.IsActive == 1 || x.Id == GetAllEvent.UserID) && x.EventName.Contains(GetAllEvent.Search)).ToList();
                    EventDetai.Data = TableVEventDetai
                        .Skip((GetAllEvent.PageNumber - 1) * GetAllEvent.PageSize)
                       .Take(GetAllEvent.PageSize)
                        .ToList();

                    EventDetai.Data.Shuffle();
                    if (EventDetai != null)
                    {
                        foreach (var item in EventDetai.Data)
                        {
                            var Images = _dbContext.TblEventUserFiles
                  .Where(x => x.EventId == item.EventId).ToList();

                            item.Golive = _dbContext.TblEventfeeds
                        .Where(x => x.EventId == item.EventId && x.Id == GetAllEvent.UserID && x.Golive == true).Any();

                            item.Interest = _dbContext.TblEventfeeds
                       .Where(x => x.EventId == item.EventId && x.Id == GetAllEvent.UserID && x.Interest == true).Any();

                            item.Favorite = _dbContext.TblEventfeeds
                       .Where(x => x.EventId == item.EventId && x.Id == GetAllEvent.UserID && x.Favorite == true).Any();

                            item.EventImages = _mapper.Map<List<TblEventFile>>(Images);

                            item.FavoriteCount = _dbContext.TblEventfeeds
                        .Where(x => x.EventId == item.EventId && x.Favorite == true).Count();

                            item.GoliveCount = _dbContext.TblEventfeeds
                               .Where(x => x.EventId == item.EventId && x.Golive == true).Count();

                            item.InterestCount = _dbContext.TblEventfeeds
                               .Where(x => x.EventId == item.EventId && x.Interest == true).Count();
                        }


                    }

                    EventDetai.PageNumber = GetAllEvent.PageNumber;
                    EventDetai.PageSize = GetAllEvent.PageSize;
                    EventDetai.TotalRecords = GetAllEvent.TotalRecords > 0 ? GetAllEvent.TotalRecords :
                                           TableVEventDetai.Count;
                    return EventDetai != null ? new ExecutionResult<PaginatedResponse<VEventDetai>>(EventDetai) :
                new ExecutionResult<PaginatedResponse<VEventDetai>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event is not Live or ")));
                }
            }





        }
        #endregion


        #region AdminGetAllEvent
        public ExecutionResult<List<VEventDetai>> AdminGetAllEvent()
        {

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "Sp_AdminEventByID";

                var table = new DataTable();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EventId", 0);
                    command.Parameters.AddWithValue("@Id", 1);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    // this will query your database and return the result to your datatable
                    DataSet ds = new DataSet();

                    da.Fill(table);

                    //Sp_GetEvent result = new Sp_GetEvent();

                    List<VEventDetai> EventDetai = new List<VEventDetai>();
                    EventDetai = Constants.ConvertDataTable<VEventDetai>(table).ToList();

                    return EventDetai != null ? new ExecutionResult<List<VEventDetai>>(EventDetai) :
                new ExecutionResult<List<VEventDetai>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event is not Live or ")));
                }
            }



        }
        #endregion




        #region Status of Golive - Interest - Favorite

        public async Task<ExecutionResult<AddEventFeed>> AddEventFeed(AddEventFeed AddEventFeed)
        {
            if (AddEventFeed == null)
            {
                return new ExecutionResult<AddEventFeed>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var createAcc = _mapper.Map<TblEventfeed>(AddEventFeed);
            TblEventfeed AllEventsfeed = new TblEventfeed();
            AllEventsfeed = _dbContext.TblEventfeeds
               .Where(x => x.EventId == AddEventFeed.EventId && x.Id == AddEventFeed.Id).FirstOrDefault();
            if (AllEventsfeed != null)
            {
                AllEventsfeed.Favorite = AddEventFeed.Favorite;
                AllEventsfeed.Golive = AddEventFeed.Golive;
                AllEventsfeed.Interest = AddEventFeed.Interest;
                AllEventsfeed.ModifiedDate = DateTime.UtcNow;
                _dbContext.SaveChanges();
            }
            else
            {

                await _dbContext.TblEventfeeds.AddAsync(createAcc);
                await _dbContext.SaveChangesAsync();
            }

            AddEventFeed = _mapper.Map<AddEventFeed>(createAcc);


            return createAcc.EventId > 0 ? new ExecutionResult<AddEventFeed>(AddEventFeed) :
              new ExecutionResult<AddEventFeed>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event")));


        }



        #endregion


        /// <summary>
        /// Used to delete media file from Event
        /// </summary>
        /// <param name="remove Event">Details to remove Event media</param>
        /// <returns>Successfully delete or not</returns>
        public async Task<ExecutionResult> RemoveEvent(GetEvent GetEvent)
        {
            if (GetEvent == null || GetEvent.EventId == 0)
            {
                return new ExecutionResult();
            }

            var postData = _dbContext.TblEvents.FirstOrDefault(x => x.EventId == GetEvent.EventId && x.Id == GetEvent.UserID);
            if (postData == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event")));
            }

            #region Remove related media

            var getAllPostMedia = _dbContext.TblEventUserFiles.Where(x => x.EventId == postData.EventId).ToList();

            foreach (var media in getAllPostMedia)
            {
                string fileName = "Event/" + Path.GetFileName(media.Downloadlink);

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.PassParaMessage, "filename")));
                }

                var delete = await _amazonRepo.DeleteFile(fileName);
                _dbContext.TblEventUserFiles.Remove(media);
                await _dbContext.SaveChangesAsync();
            }

            #endregion

            #region remove likes

            var getLikes = _dbContext.TblEventfeeds.Where(x => x.EventId == postData.EventId).ToList();

            foreach (var likes in getLikes)
            {
                _dbContext.TblEventfeeds.Remove(likes);
                await _dbContext.SaveChangesAsync();
            }

            #endregion

            #region remove comments

            var getComments = _dbContext.TblEventComments.Where(x => x.EventId == postData.EventId).ToList();

            var commentIds = getComments.Select(o => o.CommentId);

            var getCommentsReplyes = _dbContext.TblEventCommentsReplies.Where(x => commentIds.Contains(x.CommentId)).ToList();
            foreach (var item in getCommentsReplyes)
            {
                _dbContext.TblEventCommentsReplies.Remove(item);
                await _dbContext.SaveChangesAsync();
            }

            

            foreach (var comments in getComments)
            {
                var EventAboutfeeds = _dbContext.TblEventAboutfeeds.Where(x => x.CommentId == comments.CommentId).ToList();
                foreach (var item in EventAboutfeeds)
                {
                    _dbContext.TblEventAboutfeeds.Remove(item);
                    await _dbContext.SaveChangesAsync();
                }
                _dbContext.TblEventComments.Remove(comments);
                await _dbContext.SaveChangesAsync();
            }

            #endregion

            #region all Event posts Update to Only Posts
            var Posts = _dbContext.TblPosts.Where(x => x.EventId == GetEvent.EventId).ToList();
            Posts.ForEach(x => x.EventId = null);
            await _dbContext.SaveChangesAsync();
            #endregion



            _dbContext.TblEvents.Remove(postData);
            await _dbContext.SaveChangesAsync();
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Event", "deleted", "")));
        }






        #region Mohsin &Krupa

        //Add Event comment Mohsin
        public async Task<ExecutionResult<EventAboutfeed>> CreateLikeoncomment(EventAboutfeed obj)
        {
            if (obj == null)
            {
                return new ExecutionResult<EventAboutfeed>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var createAcc = _mapper.Map<TblEventAboutfeed>(obj);
            await _dbContext.TblEventAboutfeeds.AddAsync(createAcc);
            await _dbContext.SaveChangesAsync();

            //return createAcc.FeedId > 0 ? new ExecutionResult<EventAboutfeed>(_mapper.Map<EventAboutfeed>(createAcc)) :
            //    new ExecutionResult<EventAboutfeed>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event Likes")));
            return new ExecutionResult<EventAboutfeed>(new InfoMessage(string.Format(MessageHelper.Message, "Event", "Like Successfully", "")));
        }

        //Get User list Mohsin
        public ExecutionResult<List<UserEventFeed>> GetUserListByEventType(GetUserListByEventType obj)
        {

            var Alleventfeeds = (from ef in _dbContext.TblEventfeeds
                                 join um1 in _dbContext.Usermanagements on ef.Id equals um1.Id
                                 join ev in _dbContext.TblEvents on ef.EventId equals ev.EventId
                                 where ef.EventId == obj.EventId
                                 select new UserEventFeed
                                 {
                                     EventId = ev.EventId,
                                     EventName = ev.EventName,
                                     Id = ef.Id,
                                     Firstname = um1.Firstname,
                                     Lastname = um1.Lastname,
                                     Username = um1.Username,
                                     ProfileVerified = um1.ProfileVerified,
                                     Email = um1.Email,
                                     Phone = um1.Phone,
                                     Golive = ef.Golive,
                                     Favorite = ef.Favorite,
                                     Interest = ef.Interest,
                                     Image = um1.Image
                                 }).ToList();


            if (!string.IsNullOrEmpty(obj.Type))
            {
                if (obj.Type == "Interest")
                {
                    Alleventfeeds = Alleventfeeds.Where(x => x.Interest == true).ToList();
                }
                else if (obj.Type == "Golive")
                {
                    Alleventfeeds = Alleventfeeds.Where(x => x.Golive == true).ToList();
                }
                else if (obj.Type == "Favorite")
                {
                    Alleventfeeds = Alleventfeeds.Where(x => x.Favorite == true).ToList();
                }
            }

            var list = _mapper.Map<List<UserEventFeed>>(Alleventfeeds);
            //return new ExecutionResult<UpdateCommentReply>(_mapper.Map<UpdateCommentReply>(fdata),new InfoMessage(string.Format(MessageHelper.Message, "Event", "Replybody Updated Successfully", "")));
            return list != null ? new ExecutionResult<List<UserEventFeed>>(list) : new ExecutionResult<List<UserEventFeed>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Events")));


        }


        //Add Event comment Krupa
        public async Task<ExecutionResult<AddCommentOnEvent>> AddCommentOnEvent(AddCommentOnEvent obj)
        {

            if (obj == null)
            {
                return new ExecutionResult<AddCommentOnEvent>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            TblEventComment AllCommentsOnEvents = new TblEventComment();
            AllCommentsOnEvents.Id = obj.Id;
            AllCommentsOnEvents.EventId = obj.EventId;
            AllCommentsOnEvents.CommentBody = obj.CommentBody;
            await _dbContext.TblEventComments.AddAsync(AllCommentsOnEvents);
            await _dbContext.SaveChangesAsync();

            //return new ExecutionResult<AddCommentOnEvent>(new InfoMessage(string.Format(MessageHelper.Message, "Event", "Comment added Successfully", "")));

            return AllCommentsOnEvents.CommentId > 0 ? new ExecutionResult<AddCommentOnEvent>(_mapper.Map<AddCommentOnEvent>(AllCommentsOnEvents)) :
              new ExecutionResult<AddCommentOnEvent>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event Comment")));

        }

        //Update Reply on comment Krupa
        public async Task<ExecutionResult<UpdateCommentReply>> UpdateCommentReply(UpdateCommentReply obj)
        {
            if (obj == null)
            {
                return new ExecutionResult<UpdateCommentReply>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            TblEventCommentsReply xx = new TblEventCommentsReply();
            xx.CommentId = obj.CommentID;
            xx.Id = obj.Id;
            xx.ReplyBody = obj.ReplyBody;
            await _dbContext.TblEventCommentsReplies.AddAsync(xx);
            await _dbContext.SaveChangesAsync();

            //return new ExecutionResult<UpdateCommentReply>(new InfoMessage(string.Format(MessageHelper.Message, "Event", "Reply on comments successfully", "")));

            return xx.ReplyId > 0 ? new ExecutionResult<UpdateCommentReply>(_mapper.Map<UpdateCommentReply>(xx)) :
              new ExecutionResult<UpdateCommentReply>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Comment Reply")));
        }

        public async Task<ExecutionResult> RemoveEventComments(RemoveEventCommentModel RemoveComment)
        {
            #region remove comments

            var getComments = _dbContext.TblEventComments.Where(x => x.CommentId == RemoveComment.CommentId).FirstOrDefault();

            if (getComments != null)
            {
                var Postdetails = _dbContext.TblEvents.Where(x => x.EventId == getComments.EventId).FirstOrDefault();
                if (getComments.Id == RemoveComment.Id || Postdetails.Id == RemoveComment.Id)
                {
                    _dbContext.TblEventComments.Remove(getComments);
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

        public async Task<ExecutionResult> RemoveEventCommentReply(RemoveEventCommentReplyModel CommentsReply)
        {
            #region remove comments

            var getCommentsReply = _dbContext.TblEventCommentsReplies.Where(x => x.ReplyId == CommentsReply.ReplyId).FirstOrDefault();

            if (getCommentsReply != null)
            {
                var getComments = _dbContext.TblEventComments.Where(x => x.CommentId == getCommentsReply.CommentId).FirstOrDefault();
                if (getComments != null)
                {
                    var Postdetails = _dbContext.TblEvents.Where(x => x.EventId == getComments.EventId).FirstOrDefault();
                    if (getCommentsReply.Id == CommentsReply.Id || Postdetails.Id == CommentsReply.Id)
                    {
                        _dbContext.TblEventCommentsReplies.Remove(getCommentsReply);
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

        //Get User Reply On EventComments Krupa
        public ExecutionResult<List<GetUserReplyonEventCommentsModel>> GetUserReplyOnEventComments(GetCommentsReply obj)
        {
            var AllEventComments = (from e in _dbContext.TblEventComments
                                    join ev in _dbContext.TblEvents on e.EventId equals ev.EventId
                                    where e.CommentId == obj.CommentID
                                    select new GetUserReplyonEventCommentsModel
                                    {
                                        EventId = e.EventId,
                                        EventName = ev.EventName,
                                        CommentID = e.CommentId,
                                        CommentBody = e.CommentBody,
                                        UserDataforReply = (from ec in _dbContext.TblEventCommentsReplies
                                                            join um in _dbContext.Usermanagements on ec.Id equals um.Id
                                                            where ec.CommentId == e.CommentId
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
                                                            }).ToList(),

                                    }).ToList();


            var data = _mapper.Map<List<GetUserReplyonEventCommentsModel>>(AllEventComments);

            return data != null ? new ExecutionResult<List<GetUserReplyonEventCommentsModel>>(AllEventComments) :
               new ExecutionResult<List<GetUserReplyonEventCommentsModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Event Comments Reply")));
        }

        //GetEventComments Krupa
        public ExecutionResult<PaginatedResponse<GetEventCommentsModel>> GetEventComments(GetEventCom obj)
        {
            PaginatedResponse<GetEventCommentsModel> response = new PaginatedResponse<GetEventCommentsModel>();
            if (obj.PageSize != 0 && obj.PageNumber != 0)
            {
                var AllEventComments = (from e in _dbContext.TblEvents
                                        where e.EventId == obj.EventId
                                        select new GetEventCommentsModel
                                        {
                                            EventId = e.EventId,
                                            EventName = e.EventName,
                                            UsersEventsComments = (from ec in _dbContext.TblEventComments
                                                                   join um in _dbContext.Usermanagements on ec.Id equals um.Id
                                                                   where ec.EventId == e.EventId
                                                                   orderby ec.CommentId descending
                                                                   select new UserGetComments
                                                                   {
                                                                       CommentID = ec.CommentId,
                                                                       CommentBody = ec.CommentBody,
                                                                       UserId = um.Id,
                                                                       Firstname = um.Firstname,
                                                                       Lastname = um.Lastname,
                                                                       Username = um.Username,
                                                                       ProfileVerified = um.ProfileVerified,
                                                                       Email = um.Email,
                                                                       Phone = um.Phone,
                                                                       Image = um.Image,
                                                                       likeStatus = (from xx in _dbContext.TblEventAboutfeeds where xx.Id == obj.Id && xx.CommentId == ec.CommentId select xx.Id).Count() > 0 ? true : false,
                                                                       TotalLike = (from efcl in _dbContext.TblEventAboutfeeds where efcl.CommentId == ec.CommentId select efcl.CommentId).Count(),
                                                                       ReplyBody = (from xec in _dbContext.TblEventCommentsReplies
                                                                                    join xum in _dbContext.Usermanagements on xec.Id equals xum.Id
                                                                                    where xec.CommentId == ec.CommentId
                                                                                    orderby xec.ReplyId descending
                                                                                    select new UserDataforReply
                                                                                    {
                                                                                        ReplyID = xec.ReplyId,
                                                                                        ReplyBody = xec.ReplyBody,
                                                                                        UserId = xum.Id,
                                                                                        Firstname = xum.Firstname,
                                                                                        Lastname = xum.Lastname,
                                                                                        Username = xum.Username,
                                                                                        ProfileVerified = xum.ProfileVerified,
                                                                                        Email = xum.Email,
                                                                                        Phone = xum.Phone,
                                                                                        Image = xum.Image,
                                                                                    }).ToList(),

                                                                   }).Skip((obj.PageNumber - 1) * obj.PageSize).Take(obj.PageSize).ToList(),

                                        }).ToList();

                response.Data = _mapper.Map<List<GetEventCommentsModel>>(AllEventComments);
            }
            else
            {
                var AllEventComments = (from e in _dbContext.TblEvents
                                        where e.EventId == obj.EventId
                                        select new GetEventCommentsModel
                                        {
                                            EventId = e.EventId,
                                            EventName = e.EventName,
                                            UsersEventsComments = (from ec in _dbContext.TblEventComments
                                                                   join um in _dbContext.Usermanagements on ec.Id equals um.Id
                                                                   where ec.EventId == e.EventId
                                                                   orderby ec.CommentId descending
                                                                   select new UserGetComments
                                                                   {
                                                                       CommentID = ec.CommentId,
                                                                       CommentBody = ec.CommentBody,
                                                                       UserId = um.Id,
                                                                       Firstname = um.Firstname,
                                                                       Lastname = um.Lastname,
                                                                       Username = um.Username,
                                                                       ProfileVerified = um.ProfileVerified,
                                                                       Email = um.Email,
                                                                       Phone = um.Phone,
                                                                       Image = um.Image,
                                                                       likeStatus = (from xx in _dbContext.TblEventAboutfeeds where xx.Id == obj.Id && xx.CommentId == ec.CommentId select xx.Id).Count() > 0 ? true : false,
                                                                       TotalLike = (from efcl in _dbContext.TblEventAboutfeeds where efcl.CommentId == ec.CommentId select efcl.CommentId).Count(),
                                                                       ReplyBody = (from xec in _dbContext.TblEventCommentsReplies
                                                                                    join xum in _dbContext.Usermanagements on xec.Id equals xum.Id
                                                                                    where xec.CommentId == ec.CommentId
                                                                                    orderby xec.ReplyId descending
                                                                                    select new UserDataforReply
                                                                                    {
                                                                                        ReplyID = xec.ReplyId,
                                                                                        ReplyBody = xec.ReplyBody,
                                                                                        UserId = xum.Id,
                                                                                        Firstname = xum.Firstname,
                                                                                        Lastname = xum.Lastname,
                                                                                        Username = xum.Username,
                                                                                        ProfileVerified = xum.ProfileVerified,
                                                                                        Email = xum.Email,
                                                                                        Phone = xum.Phone,
                                                                                        Image = xum.Image,
                                                                                    }).ToList(),

                                                                   }).ToList(),

                                        }).ToList();

                response.Data = _mapper.Map<List<GetEventCommentsModel>>(AllEventComments);
            }



            response.PageNumber = obj.PageNumber;
            response.PageSize = obj.PageSize;
            response.TotalRecords = obj.TotalRecords > 0 ? obj.TotalRecords :
                                   _dbContext.TblEventComments.Where(x => x.EventId == obj.EventId).Count();
            return response.Data.Count > 0 ? new ExecutionResult<PaginatedResponse<GetEventCommentsModel>>(_mapper.Map<PaginatedResponse<GetEventCommentsModel>>(response)) :
               new ExecutionResult<PaginatedResponse<GetEventCommentsModel>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Events Comments")));
        }


        //Add Support Krupa
        public async Task<ExecutionResult<AddSupport>> AddSupport(AddSupport obj)
        {

            if (obj == null)
            {
                return new ExecutionResult<AddSupport>(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }

            TblSupport AllSupports = new TblSupport();
            AllSupports.Id = obj.UserId;
            AllSupports.Email = obj.Email;
            AllSupports.Body = obj.Body;
            await _dbContext.TblSupports.AddAsync(AllSupports);
            await _dbContext.SaveChangesAsync();

            return new ExecutionResult<AddSupport>(new InfoMessage(string.Format(MessageHelper.Message, "Event", "Support added Successfully", "")));
        }
        //Get Supports by user id Krupa
        public ExecutionResult<List<GetAllSupportUser>> GetSupportByUserId(GetSupportByUserId obj)
        {

            var AllSupports = (from s in _dbContext.TblSupports
                               where s.Id == obj.UserId || obj.UserId == 0
                               select new GetAllSupportUser
                               {
                                   SupportID = s.SupportId,
                                   UserId = s.Id,
                                   Email = s.Email,
                                   Body = s.Body
                               }).ToList();


            var data = _mapper.Map<List<GetAllSupportUser>>(AllSupports);


            return data != null ? new ExecutionResult<List<GetAllSupportUser>>(AllSupports) :
               new ExecutionResult<List<GetAllSupportUser>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Supports")));

        }
        #endregion
    }
}
