namespace Totem.Business.Core.AppSettings
{
    /// <summary>
    /// Represents App settings section in AppSettings file
    /// </summary>
    public class AppSettings
    {
        public AWS AWS { get; set; }
        public Firebase Firebase { get; set; }
        public string ForgetPassword { get; set; }
        public string Login { get; set; }
        public EmailConfig Emailconfig { get; set; }
        public JWT Jwt { get; set; }
    }
    public class AWS
    {
        public string AccessKey { get; set; }
        public string SecretAccessKey { get; set; }
        public string BucketName { get; set; }
        public string BaseUrl { get; set; }
    }

    public class Firebase
    {
        public string Authorization { get; set; }
        public string SenderId { get; set; }        
        public string BaseUrl { get; set; }
    }
    public class EmailConfig
    {
        public int Port { get; set; }
        public string BaseUrl { get; set; }
        public string FromEmailAddress { get; set; }
        public string EmailPassword { get; set; }
        public string SMTPUrl { get; set; }
        public string ApiKey { get; set; }
    }

    public class JWT
    {
        public string Token { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
