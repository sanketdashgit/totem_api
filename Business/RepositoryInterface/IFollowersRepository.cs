using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels;
using Totem.Business.Core.DataTransferModels.Account;
using Totem.Database.Models;

namespace Totem.Business.RepositoryInterface
{
    public interface IFollowersRepository
    {
        ExecutionResult<PaginatedResponse<FollowUsers>> GetAllUsers(FollowModel listModel);

        ExecutionResult<PaginatedResponse<FollowUsers>> GetAllExploreUsers(FollowModel listModel);
        ExecutionResult<PaginatedResponse<FollowUsers>> UpdatedGetAllUsers(GetFollowModel listModel);
        Task<ExecutionResult<FollowersCountModel>> GetfollowerCount(FollowersModel FollowersModel);
        Task<ExecutionResult<PaginatedResponse<FollowUsers>>> GetAllfollower(FollowModel FollowModel);
        Task<ExecutionResult<PaginatedResponse<FollowUsers>>> GetAllfollow(FollowModel FollowModel);
        Task<ExecutionResult<PaginatedResponse<FollowUsers>>> TagAllfollow(FollowModel FollowModel);
        Task<ExecutionResult<PaginatedResponse<FollowUsers>>> GetfollowerRequest(FollowModel FollowModel);
        Task<ExecutionResult> ApproveFollow(Follow Follow);
        Task<ExecutionResult> Follow(Follow Follow, SendNotification sendNotification);

        Task<ExecutionResult> Deletesuggested(DeleteSuggested DeleteSuggested);
    }
}
