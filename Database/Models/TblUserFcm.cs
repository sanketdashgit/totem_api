using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblUserFcm
    {
        public long Id { get; set; }
        public long FcmId { get; set; }
        public string Fcm { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Login { get; set; }
    }
}
