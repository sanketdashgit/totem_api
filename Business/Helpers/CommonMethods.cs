using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Totem.Business.Core.AppSettings;
using Totem.Business.Core.DataTransferModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Totem.Externals.Caching;

namespace Totem.Business.Helpers
{
    public class CommonMethods
    {
        #region Constructor
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IConfiguration _configuration;
        private readonly EmailConfig _emailConfig;
        private const string filePath = "*** provide the full path name of the file to upload ***";
        public CommonMethods(IConfiguration configuration, IOptions<AppSettings> appSettings)
        {

            _appSettings = appSettings;
            _emailConfig = appSettings.Value.Emailconfig;
            _configuration = configuration;
        }
        #endregion

        #region Encrypt id
        public string Encrypt(string inputText)
        {
            string EncryptionKey = _configuration.GetSection("EncryptDecryptKey").GetSection("EncryptionKey").Value;
            byte[] keybytes = Encoding.ASCII.GetBytes(EncryptionKey.Length.ToString());
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] plainText = Encoding.Unicode.GetBytes(inputText);
            PasswordDeriveBytes pwdbytes = new PasswordDeriveBytes(EncryptionKey, keybytes);
            using (ICryptoTransform encryptrans = rijndaelCipher.CreateEncryptor(pwdbytes.GetBytes(32), pwdbytes.GetBytes(16)))
            {
                using (MemoryStream mstrm = new MemoryStream())
                {
                    using (CryptoStream cryptstm = new CryptoStream(mstrm, encryptrans, CryptoStreamMode.Write))
                    {
                        cryptstm.Write(plainText, 0, plainText.Length);
                        cryptstm.Close();
                        return Convert.ToBase64String(mstrm.ToArray());
                    }
                }
            }
        }
        #endregion
       
        #region SendMail
        public async Task<bool> SendMail(string email, string htmlResponse, string subject)
        {
            bool obj = false;
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_emailConfig.FromEmailAddress);
            message.To.Add(new MailAddress(email));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = htmlResponse;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = _appSettings.Value.Emailconfig.SMTPUrl;
            smtp.Port = _appSettings.Value.Emailconfig.Port;
            //smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(_appSettings.Value.Emailconfig.FromEmailAddress, _appSettings.Value.Emailconfig.EmailPassword);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
           // smtp.UseDefaultCredentials = true;
            smtp.Send(message);
            obj = true;
            return obj;
            //try
            //{
            //   smtp.Send(message);
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    string errortext = ex.ToString();
            //    return false;
            //}
        }
        #endregion

        #region Send Mail with Admin CC
        public async Task<bool> SendMailAdminCC(string email, string htmlResponse, string subject,string adminMail)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_emailConfig.FromEmailAddress);
            message.To.Add(new MailAddress(email));
            message.Subject = subject;
            message.CC.Add(adminMail);
            message.IsBodyHtml = true;
            message.Body = htmlResponse;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = _appSettings.Value.Emailconfig.SMTPUrl;
            smtp.Port = _appSettings.Value.Emailconfig.Port;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(_appSettings.Value.Emailconfig.FromEmailAddress, _appSettings.Value.Emailconfig.EmailPassword);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Send(message);
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
