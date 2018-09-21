using SocialMediasAssistant.Common;
using SocialMediasAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static SocialMediasAssistant.Common.InstagramAPI;

namespace SocialMediasAssistant.Controllers
{
    [CustomAuthorize]
    public class InstagramController : SocialMediaBaseController
    {

        public ActionResult Accounts()
        {
            return View();
        }

        public JsonResult GetNumberOfFollows(string id)
        {
            string accessToken = context.Users.First(u => u.Logins
                .Any(l => l.ProviderKey == id))
                .AccessTokens.First(a => a.Provider == "Instagram").AccessTokenValue;
            AccountInfo accountInfo = GetAccountInfo(accessToken);
            return Json(accountInfo.counts.followed_by, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccounts(int order)
        {
            List<string> accessTokens = context.InstagramAccounts.Include("ApplicationUser")
                .OrderBy(c => c.ID)
                .Skip(10 * order).Take(10).ToList().Select(
                c =>
                    context.Users.First(u => u.Logins
                    .Any(l => l.ProviderKey == c.Link)).AccessTokens
                    .First(a => a.Provider == "Instagram").AccessTokenValue
                ).ToList();
            List<AccountInfo> accountInfos = new List<AccountInfo>();
            foreach (string accessToken in accessTokens)
            {
                accountInfos.Add(GetAccountInfo(accessToken));
            }
            return Json(accountInfos,
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddAccount()
        {
            return View();
        }

        [HttpPost]
        [ActionName("AddAccount")]
        public async Task<ActionResult> AddAccountPostAsync()
        {
            InstagramAccount account = new InstagramAccount
            {
                Link = CurrentUser.Logins.Last().ProviderKey,
                ApplicationUser = CurrentUser
            };
            context.InstagramAccounts.Add(account);

            await context.SaveChangesAsync();
            return RedirectToAction("Accounts");
        }

        public ActionResult Posts()
        {
            return View();
        }

        public JsonResult GetCurrentUserPosts()
        {
            InstagramMediaInfo mediaInfo = GetMediaInfo(CurrentUser.AccessTokens.First(a => a.Provider == "Instagram").AccessTokenValue);
            return Json(mediaInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPosts(int order)
        {

            return Json(context.InstagramPosts.Include("ApplicationUser").OrderBy(c => c.ID)
                .Skip(10 * order).Take(10).ToList().Select(
                c => new
                {
                    c.Link,
                    AuthorEmail = c.ApplicationUser.Email
                }).ToList(),
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetPostLikes(string link, int numberOfPrevLikes=0, bool increasePoints=false)
        {
            InstagramPost post = context.InstagramPosts.First(c => c.Link == link);
            InstagramMediaInfo mediaInfo = GetMediaInfo(post.ApplicationUser
                .AccessTokens.First(a => a.Provider == "Instagram").AccessTokenValue);
            int count = mediaInfo.data.First(d => d.link == link).likes.count;
            if (increasePoints)
            {
                if(count > numberOfPrevLikes)
                {
                    lock (CurrentUser)
                    {
                        CurrentUser.Points += 10;
                    }
                    await context.SaveChangesAsync();
                }
            }
            return Json( count,
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> HasLikedPagePostAsync(string InstagramPostLink,
            bool increasePointsIfLiked = false)
        {
            InstagramPost InstagramPost = context.InstagramPosts.SingleOrDefault(c => c.Link == InstagramPostLink);
            List<string> likers = new List<string>();
            bool hasLiked = likers.Any(liker => liker == CurrentUser.Name);
            if (hasLiked && increasePointsIfLiked)
            {
                CurrentUser.Points += 10;
                await context.SaveChangesAsync();
            }
            return Json(hasLiked, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPost(InstagramPost post)
        {
            if (ModelState.IsValid)
            {
                post.ApplicationUser = CurrentUser;
                context.InstagramPosts.Add(post);
                await context.SaveChangesAsync();
                return RedirectToAction("PagePosts");
            }
            return View();
        }

    }
}