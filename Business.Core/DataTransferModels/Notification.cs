using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels
{
    public class notification
    {
        public string title { get; set; }
        public string body { get; set; }
        public string image { get; set; }


    }

    public class data
    {
        public long conversationId { get; set; }
        public int type { get; set; }
        public string conversationInfo { get; set; }

    }

    public class SendNotification
    {
        public notification notification { get; set; }
        public data data { get; set; }
        public List<string> registration_ids { get; set; }      

    }

    public class SendNotificationModel
    {
        public SendNotification SendNotification { get; set; }       
        public long Id { get; set; }

    }
}
