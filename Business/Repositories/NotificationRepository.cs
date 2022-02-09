using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Totem.Business.Core.AppSettings;
using Totem.Business.Core.DataTransferModels;
using Totem.Business.RepositoryInterface;
using Totem.Database.Models;

namespace Totem.Business.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        #region Constructor
        private readonly TotemDBContext _dbContext;
        private readonly Firebase _FirebaseSettings;       
        public NotificationRepository(TotemDBContext dbContext, IOptions<AppSettings> appSettings)
        {
            _dbContext = dbContext;
            _FirebaseSettings = appSettings.Value.Firebase;            
        }
        #endregion


        public async Task<bool> NotifyAsync(SendNotification SendNotification)
        {
            try
            {
              
                    string APIKey = _FirebaseSettings.Authorization;
                    string SenderId = _FirebaseSettings.SenderId;
                    var serverKey = string.Format("Key={0}", APIKey);

                    var senderId = string.Format("id={0}", SenderId);                  

                    var jsonBody = JsonConvert.SerializeObject(SendNotification);

                    using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, _FirebaseSettings.BaseUrl))
                    {
                        httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                        httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                        httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                        using (var httpClient = new HttpClient())
                        {
                            var result = await httpClient.SendAsync(httpRequest);

                            if (result.IsSuccessStatusCode)
                            {
                            string abc = result.ToString();
                                return true;
                            }
                            else
                            {
                              //  _logger.LogError($"Error sending notification. Status Code: {result.StatusCode}");
                            }
                        }
                    }
                
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Exception thrown in Notify Service: {ex}");
            }

            return false;
        }
    }
}
