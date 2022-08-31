using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dna.core.libs.Sender.ConfigType;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace dna.core.libs.Sender
{
    
    public class FCMSender : IFCMSender
    {
        private FCMConfig _config;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver">string</param>
        /// <param name="subject">string</param>
        /// <param name="message">string</param>
        /// <param name="data">FCMOption</param>
        /// <returns></returns>
        public async Task<Response> SendAsync(string receiver, string subject, string message,  object data = null)
        {
            var options = (data as FCMOption);
            var custom = new
            {
                custom_notification = new
                {
                    title = subject,
                    body = message,
                    sound = "default",
                    priority = options.Priority.ToString().ToLower(),
                    targetScreen = options.TargetScreen,
                    parameter = options.JsonData,
                    icon = "ic_notification",
                    show_in_foreground = true
                }
            };
            var notification = new
            {
                to = receiver,
                data = custom,
                priority = options.Priority.ToString().ToLower()

            };
            return await SendNotification( notification );
        }

        
        public async Task<Response> SendMultipleAsync(List<string> receivers, string subject, string message, object data = null)
        {
            var result = new List<Response>();
            foreach ( var item in receivers )
            {
                var response = await SendAsync(item, subject, message, data);
                result.Add(response);
            }
            return result.Count > 0 ? result[0] : new Response();
        }

        public async Task<Response> SendTopicAsync(List<string> topics, string title, string message, object data = null)
        {
            string condition = "";
            foreach ( var item in topics )
            {
                condition += String.Format("/'{0}/' in topics || ");
            }
            condition = condition.Substring(0, condition.Length - 4);
            var notif = new
            {
                data = data,
                condition = condition,
                notification = new
                {
                    body = message,
                    title = title
                }
            };
            return await SendNotification(notif);
        }

        public async Task<Response> SendTopicAsync(string topic, string title, string message, object data = null)
        {
            var notif = new
            {
                to = String.Format("/topics/{0}", topic),
                data = data,
                notification = new
                {
                    body = message,
                    title = title
                }
            };
            return await SendNotification(notif);
        }

        public void SetConfig(SenderConfiguration senderConfiguration)
        {
            _config = senderConfiguration.FirebaseMessaging;
        }

        #region Helper
        protected async Task<Response> SendNotification(object data)
        {
            Response result = new Response();
            try
            {
                result.Successful = true;
                result.Error = null;

                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri("https://fcm.googleapis.com/")
                };

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", String.Format("={0}", _config.ApiKey));
                client.DefaultRequestHeaders.Add("Sender", String.Format("id={0}", _config.SenderId));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                string jsonPost = JsonConvert.SerializeObject(data);
                HttpResponseMessage response = await client.PostAsync("fcm/send",
                    new StringContent(jsonPost, Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                if ( response.IsSuccessStatusCode )
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    result.Message = responseBody;
                }


            }
            catch ( Exception ex )
            {
                result.Successful = false;
                result.Message = null;
                result.Error = ex;
            }
            return result;
        }
        #endregion
    }
}
