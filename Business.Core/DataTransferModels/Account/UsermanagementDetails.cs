using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.Account
{
    public partial class UsermanagementDetails
    {
        public UsermanagementDetails()
        {

        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }     
        public bool? ProfileVerified { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public string Password { get; set; }
        public int ReferredBy { get; set; }
        public string Fcm { get; set; }
        public int? SignInType { get; set; }
        
    }


    public partial class UserFcmModel
    {
        public long Id { get; set; }
        public string Fcm { get; set; }       
        public bool Login { get; set; }
    }

    public partial class GetFcmModel
    {
        public List<long> Id { get; set; }
       
    }
    public class EditprivacyModel
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
    }

    public class UserIdModel
    {
        public long Id { get; set; }

    }

    public class AddUserNotification
    {
       
        public long Id { get; set; }
        public bool MessageNotification { get; set; }
        public bool EventNotification { get; set; }
        public bool FollowNotification { get; set; }
    }
    public class InsertUsermanagement :UsermanagementDetails
    {
        public int? Role { get; set; } = 0;
        public string CreatedBy { get; set; } = "";
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; } = "";
        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool? IsEmailVerified { get; set; } = false;
        public string Address { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Bio { get; set; } = "";
        public string Image { get; set; } = "";
        public int? Gender { get; set; } = 0;
        public bool? IsMobileVerified { get; set; } = false;
        public int? MobileOtp { get; set; } = 0;
        public string Token { get; set; } = "";
    }

    public partial class UsermanagementDetailsID 
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
      
        public int? Role { get; set; }

        public bool IsActive { get; set; }
      
        public bool? IsEmailVerified { get; set; }
        
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public int? Gender { get; set; }
        public bool? IsMobileVerified { get; set; }
        //public int? MobileOtp { get; set; }
        public string Token { get; set; }

        public int? SignInType { get; set; }

        public bool? IsPrivate { get; set; }

      
        public bool? ProfileVerified { get; set; }

    }

}
