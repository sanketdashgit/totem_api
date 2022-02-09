using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Account
{
    public class NotificationModel
    {
        public int Ssrno { get; set; }
        public string Id { get; set; }
        public DateTime? Date { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Descp { get; set; }
        public string NuserName { get; set; }
        public string Readflag { get; set; }
        public int RequestAccepted { get; set; }
        public string NotificationType { get; set; }      
        public long NotificationTypeId { get; set; }
    }
}
