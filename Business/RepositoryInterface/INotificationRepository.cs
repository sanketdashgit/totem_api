using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totem.Business.Core.DataTransferModels;

namespace Totem.Business.RepositoryInterface
{
    public interface INotificationRepository
    {
        Task<bool> NotifyAsync(SendNotification SendNotification);
    }
}
