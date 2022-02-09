using System;
using System.ComponentModel.DataAnnotations;

namespace Totem.Business.Core.DataTransferModels.Account
{
    public class LoginResponseModel : CreateAccountRequestModel
    {
        public long Id { get; set; }
       // public bool IsAdmin { get; set; }
        public string Token { get; set; }
    }

    public class Login 
    {
        public string EmailId { get; set; } 
        public string Password { get; set; }       
        public string Fcm { get; set; }
        public int? SignInType { get; set; }

    }

    public class GetSpotify
    {
        public long Id { get; set; }

    }

    public class checkmail
    {
        public string EmailId { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }

    }

    public class EmailModel
    {
        public string EmailId { get; set; }
       
    }

    public class checkEmailToken
    {
        public string EmailToken { get; set; }

    }

    public class ResetPassword
    {
        public Guid EmailToken { get; set; }
        public string Password { get; set; }
    }

    public class ChangePassword
    {
        public long accountId { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }
}
