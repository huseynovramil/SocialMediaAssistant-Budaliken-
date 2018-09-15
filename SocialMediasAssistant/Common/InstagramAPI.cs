using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using Newtonsoft.Json;
using SocialMediasAssistant.Models;

namespace SocialMediasAssistant.Common
{
    public static class InstagramAPI
    {
        private static HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://api.instagram.com/")
        };

        public static InstagramMediaInfo GetMediaInfo(string access_token)
        {
            string parameters = "/v1/users/self/media/recent/?access_token=" + access_token;
            InstagramMediaInfo mediaInfo = new InstagramMediaInfo();
            HttpResponseMessage message = client.GetAsync(parameters).Result;
            if (message.IsSuccessStatusCode)
            {
                mediaInfo = message.Content.ReadAsAsync<InstagramMediaInfo>().Result;
                return mediaInfo;
            }
            return null;
        }

        public class InstagramMediaInfo
        {
            public MediaInfo[] data { get; set; }
        }

        public class MediaInfo
        {
            public string link { get; set; }
            public Like likes { get; set; }
        }

        public class Like
        {
            public int count { get; set; }
        }

        public static AccountInfo GetAccountInfo(string access_token)
        {
            string urlParameters = "v1/users/self/?access_token=" + access_token;
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            AccountInfoResponse accountInfo = new AccountInfoResponse();
            if (response.IsSuccessStatusCode)
            {
                accountInfo = response.Content.ReadAsAsync<AccountInfoResponse>().Result;
            }
            return accountInfo.data;
        }

        public class AccountInfoResponse
        {
            public AccountInfo data { get; set; }
        }

        public class AccountInfo
        {
            public string id { get; set; }
            public string username { get; set; }
            public string full_name { get; set; }
            public string profile_picture { get; set; }
            public string bio { get; set; }
            public Count counts { get; set; }
        }

        public class Count
        {
            public int media { get; set; }
            public int follow { get; set; }
            public int followed_by { get; set; }
        }
    }
}