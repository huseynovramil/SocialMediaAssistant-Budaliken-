using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SocialMediasAssistant.Models
{
    [Table("FacebookPagePosts")]
    public class FacebookPagePost:Content
    {
        public string PostId
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                string link = Link;
                int postIdIndex = link.IndexOf("posts/") + ("posts/").Length;
                stringBuilder.Append(link.Substring(postIdIndex, link.Length - postIdIndex));
                return stringBuilder.ToString();
            }
        }

        public string FullPostId
        {
            get
            {
                return Page.PageId + "_" + PostId;
            }
        }

        HttpClient client = new HttpClient();
        public async Task<string> GetPageAccessToken()
        {
            client.BaseAddress = new Uri("https://graph.facebook.com/v3.1/");

            client.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string urlParametersForAccounts = ApplicationUser.Logins.Last().ProviderKey + "/accounts"
                + "?access_token=" + ApplicationUser.AccessTokens;
            HttpResponseMessage messageForAccounts = client.GetAsync(urlParametersForAccounts).Result;
            if (messageForAccounts.IsSuccessStatusCode)
            {
                FacebookResponseForAccounts accountsResponse = new FacebookResponseForAccounts();
                accountsResponse = await messageForAccounts.Content.ReadAsAsync<FacebookResponseForAccounts>();
                string accessToken = accountsResponse.data.SingleOrDefault(u => u.id == Page.PageId).access_token;
                return accessToken;
            }
            return null;
        }

        public async Task<List<string>> GetLikersAsync()
        {
            client.BaseAddress = new Uri("https://graph.facebook.com/v3.1/");

            client.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string accessToken = Page.AccessToken;
            if (accessToken != null)
            {
                string urlParametersForLikes = GetUrlParameters(accessToken);
                HttpResponseMessage message = client.GetAsync(urlParametersForLikes).Result;
                FacebookResponseForLikes facebookResponse = new FacebookResponseForLikes();
                if (message.IsSuccessStatusCode)
                {
                    facebookResponse = await message.Content.ReadAsAsync<FacebookResponseForLikes>();
                }
                return facebookResponse.data.Select(u => u.name).ToList();
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

        private class FacebookResponseForAccounts
        {
            public class Page
            {
                public string access_token { get; set; }
                public string id { get; set; }
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

            public Page[] data { get; set; }
            public Paging paging { get; set; }
        }
        public string GetUrlParameters(string _accessToken)
        {
            StringBuilder stringBuilder = new StringBuilder();

            int startIndexOfPageId = Link.IndexOf(".com/") + 5;
            int endIndexOfPageId = Link.IndexOf("/posts");
            string idOfPage = Link.Substring(startIndexOfPageId, endIndexOfPageId - startIndexOfPageId);

            int startIndexOfPostId = Link.IndexOf("/posts/") + 7;

            string idOfPost = Link.Substring(startIndexOfPostId);

            string accessToken = "?access_token=" + _accessToken;

            stringBuilder.Append(idOfPage).Append("_" + idOfPost)
                .Append("/likes").Append(accessToken);
            return stringBuilder.ToString();
        }
        public virtual FacebookPostPage Page { get; set; }
    }
}