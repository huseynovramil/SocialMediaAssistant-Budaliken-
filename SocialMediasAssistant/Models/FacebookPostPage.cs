using SocialMediasAssistant.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SocialMediasAssistant.Models
{
    [Table("FacebookPostPages")]
    public class FacebookPostPage : Content
    {
        public string PageId { get; set; }
        public string DecryptedAccessToken { get; set; }
        HttpClient client = new HttpClient();
        public async Task<string> GetPageLongLivedAccessToken(string shortLivedAccessToken)
        {
            client.BaseAddress = new Uri("https://graph.facebook.com/v3.1/");

            client.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string urlParametersForLongLiveToken = "oauth/access_token?grant_type=fb_exchange_token&" +
                "client_id="+ConfigurationManager.AppSettings["FacebookAppId"] +"&" +
                "client_secret="+ConfigurationManager.AppSettings["FacebookAppSecret"] +"&" +
                "fb_exchange_token=" + shortLivedAccessToken;

            HttpResponseMessage messageForLongLiveAccessToken = client.GetAsync(urlParametersForLongLiveToken).Result;
            if (messageForLongLiveAccessToken.IsSuccessStatusCode)
            {
                LongLiveAccessToken accessToken = await messageForLongLiveAccessToken.Content.ReadAsAsync<LongLiveAccessToken>();
                return accessToken.access_token;
            }
            return null;
        }

        private class LongLiveAccessToken
        {
            public string access_token { get; set; }
        }
        [NotMapped]
        public string AccessToken
        {
            get
            {
                if (DecryptedAccessToken != null)
                    return Cipher.Decrypt(DecryptedAccessToken, ConfigurationManager.AppSettings["cipherPassword"]);
                else
                    return null;
            }
            set
            {
                if (value != null)
                    DecryptedAccessToken = Cipher.Encrypt(GetPageLongLivedAccessToken(value).Result, ConfigurationManager.AppSettings["cipherPassword"]);
                else
                    DecryptedAccessToken = null;
            }
        }


        public async Task<List<string>> GetLikersAsync(string accessToken)
        {
            client.BaseAddress = new Uri("https://graph.facebook.com/v3.1/");

            client.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            if (accessToken != null)
            {
                string urlParametersForLikes = "me/likes?access_token=" + accessToken;
                HttpResponseMessage message = client.GetAsync(urlParametersForLikes).Result;
                FacebookResponseForLikes facebookResponse = new FacebookResponseForLikes();
                if (message.IsSuccessStatusCode)
                {
                    facebookResponse = await message.Content.ReadAsAsync<FacebookResponseForLikes>();
                }
                return facebookResponse.data.Select(u => u.id).ToList();
            }
            return null;
        }

        private class FacebookResponseForLikes
        {
            public class User
            {
                public string id { get; set; }
                public string name { get; set; }
            }
            public class Paging
            {
                public class Cursors
                {
                    public string before { get; set; }
                    public string after { get; set; }
                }
                public Cursors cursors { get; set; }
            }

            public Paging paging { get; set; }
            public User[] data { get; set; }
        }
    }
}