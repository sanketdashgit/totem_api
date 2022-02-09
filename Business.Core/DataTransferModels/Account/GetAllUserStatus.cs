using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Account
{

    public class GetAllUserReferedUser
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string RefferedCount { get; set; }

    }
    public class GetAllUserStatusModel
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public string Image { get; set; }
        public bool? BussinessUser { get; set; }
        public bool ProfileVerified { get; set; }
        public bool IsBusinessRequestSend { get; set; }
        public bool IsProfileVarificationRequestSend { get; set; }
        public bool? IsPrivate { get; set; }
    }

    public class GetAllUserStatusModelMobile : UsermanagementDetailsID
    {

        public bool? BussinessUser { get; set; }
        public bool ProfileVerified { get; set; }
        public bool IsBusinessRequestSend { get; set; }
        public bool IsProfileVarificationRequestSend { get; set; }

    }

    public class GetAllUserStatuswithNotification : GetAllUserStatusModelMobile
    {
        public bool MessageNotification { get; set; }
        public bool EventNotification { get; set; }
        public bool FollowNotification { get; set; }
    }
}


