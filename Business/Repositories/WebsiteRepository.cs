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


namespace Totem.Business.Repositories
{
    public class WebsiteRepository : IWebsiteRepository
    {
        #region Constructor

        private readonly TotemDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly TokenManager _tokenManger;
        private readonly CommonMethods _commonMethods;
        private readonly IOptions<AppSettings> _appSettings;

        public WebsiteRepository(
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
        }

        #endregion

        #region Create website Content

        public async Task<ExecutionResult> CreateWebContent(WebsiteConten websiteConten)
        {
            if (websiteConten == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var createAcc = _mapper.Map<WebsiteContent>(websiteConten);           
            await _dbContext.WebsiteContents.AddAsync(createAcc);
            await _dbContext.SaveChangesAsync();
            
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Website Design", "added", "please check")));
        }



        #endregion

        #region ReceiveUpdatesSave

        public async Task<ExecutionResult> ReceiveEmailUpdatesSave(TblUpdatEmail Email)
        {
            if (Email == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var account = _dbContext.TblUpdateEmails.Where(x => x.Email.ToLower() == Email.Email.ToLower()).Any();
            var createAcc = _mapper.Map<TblUpdateEmail>(Email);
            if (!account)
            {
               
                await _dbContext.TblUpdateEmails.AddAsync(createAcc);
                await _dbContext.SaveChangesAsync();
                // return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Website", "added", "Email to send updates")));
            }
           

            string HTMLString = EmailHelper.ReceiveEmailUpdatesSave("");

            var isMailSent = await _commonMethods.SendMail(createAcc.Email, HTMLString, MessageHelper.Account.ReceiveEmailUpdatesSave);

            if (!isMailSent)
            {
                return new ExecutionResult(new ErrorInfo(MessageHelper.Account.EmailSentReceiveEmailUpdatesSave));
            }

            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Email", "subscribed", "")));
        }



        #endregion


        #region Early To Party

        public async Task<ExecutionResult> EarlyToParty(EarlyToPartyModel EarlyToPartyModel)
        {
            if (EarlyToPartyModel == null)
            {
                return new ExecutionResult(new ErrorInfo(string.Format(MessageHelper.NullMessage, "Model")));
            }
            var account = _dbContext.TblEarlyToParties.Where(x => x.Email.ToLower() == EarlyToPartyModel.Email.ToLower()).FirstOrDefault();
            var createAcc = _mapper.Map<TblEarlyToParty>(EarlyToPartyModel);
            if (account == null)
            {

                await _dbContext.TblEarlyToParties.AddAsync(createAcc);
                await _dbContext.SaveChangesAsync();
                // return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Website", "added", "Email to send updates")));
            }
            else
            {
                account.Phone = EarlyToPartyModel.Phone;
                _dbContext.SaveChanges();
            }


            string HTMLString = EmailHelper.EarlyToParty("");
            var adminuser = _dbContext.Usermanagements.Where(x => x.Id == 1).FirstOrDefault();
            var isMailSent = await _commonMethods.SendMailAdminCC(createAcc.Email, HTMLString, MessageHelper.Account.EarlyToParty,adminuser.Email);           
            if (!isMailSent)
            {
                return new ExecutionResult(new ErrorInfo(MessageHelper.Account.EmailSentReceiveEmailUpdatesSave));
            }

            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Details", "Sent", "")));
        }



        #endregion


        public async Task<ExecutionResult<List<string>>> Getsection()
        {
            //EFContext.TestAddresses.Select(m => m.Name).Distinct();

            List<string> AllUsers = _dbContext.WebsiteContents.Select(m => m.Section).Distinct().ToList();
            return AllUsers.Count > 0 ? new ExecutionResult<List<string>>(_mapper.Map<List<string>>(AllUsers)) :
               new ExecutionResult<List<string>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Website content")));

        }

        public async Task<ExecutionResult<List<Section>>> GetSession1()
        {
            //EFContext.TestAddresses.Select(m => m.Name).Distinct();
            List<Section> days = Enum.GetNames(typeof(Section))
                            .Cast<Section>()
                            .ToList();
            //List<string> AllUsers = _dbContext.WebsiteContents.Select(m => m.Section).Distinct().ToList();
            return days.Count > 0 ? new ExecutionResult<List<Section>>(days) :
               new ExecutionResult<List<Section>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Website content")));

        }

        #region update Web Content
        public async Task<ExecutionResult> UpdateWebContent(WebsiteContenID websiteConten)
        {

            if (websiteConten != null)
            {
                var account = _dbContext.WebsiteContents.FirstOrDefault(x => x.Id == websiteConten.Id);
                if (account != null)
                {
                   
                    account.Title = websiteConten.Title;
                    account.Description = websiteConten.Description;
                    account.Page = websiteConten.Page;
                    _dbContext.SaveChanges();
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.SuccessMessage, "Website", "Updated", "")));

                }
                else
                {
                    return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "Website")));
                }
            }
            return new ExecutionResult(new InfoMessage(string.Format(MessageHelper.NoFound, "Website")));



        }
        #endregion
        #region Get All website Content
        public async Task<ExecutionResult<List<WebsiteContenID>>> GetAll()
        {
            var AllUsers = _dbContext.WebsiteContents.Where(x => x.Id > 0).OrderBy(x => x.Id).ToList();

            return AllUsers.Count > 0 ? new ExecutionResult<List<WebsiteContenID>>(_mapper.Map<List<WebsiteContenID>>(AllUsers)) :
               new ExecutionResult<List<WebsiteContenID>>(new ErrorInfo(string.Format(MessageHelper.NoFound, "Website content")));
        }


        #endregion
    }
}
