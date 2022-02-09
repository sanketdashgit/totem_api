using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblNotification
    {
        public int Ssrno { get; set; }
        public long Id { get; set; }
        public DateTime? Date { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Descp { get; set; }
        public string Readflag { get; set; }
        public string NotificationType { get; set; }
        public int RequestAccepted { get; set; }
        public long NotificationTypeId { get; set; }
        public string NuserName { get; set; }
    }
}
