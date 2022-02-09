using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Account;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Totem.Business.Core.DataTransferModels.WebsiteDesign;
using Totem.Business.Core.Consts;

namespace Totem.Business.RepositoryInterface
{
    public interface IWebsiteRepository
    {
        // Create Account
        Task<ExecutionResult> CreateWebContent(WebsiteConten websiteConten);

        Task<ExecutionResult> ReceiveEmailUpdatesSave(TblUpdatEmail Email);

        Task<ExecutionResult> UpdateWebContent(WebsiteContenID websiteConten);

        Task<ExecutionResult<List<WebsiteContenID>>> GetAll();

       Task<ExecutionResult<List<string>>> Getsection();
        Task<ExecutionResult<List<Section>>> GetSession1();

        Task<ExecutionResult> EarlyToParty(EarlyToPartyModel EarlyToPartyModel);

    }
}
