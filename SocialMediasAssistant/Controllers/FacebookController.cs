using Microsoft.AspNet.Identity;
using SocialMediasAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Data.Entity.Validation;
using SocialMediasAssistant.Common;

namespace SocialMediasAssistant.Controllers
{
    [CustomAuthorize]
    public class FacebookController : SocialMediaBaseController
    {
        public ActionResult PagePosts()
        {
            return View();
        }

        public JsonResult GetPagePosts(int order)
        {

            return Json(context.FacebookPagePosts.Include("ApplicationUser").OrderBy(c => c.ID)
                .Skip(10 * order).Take(10).ToList().Select(
                c => new
                {
                    c.Link,
                    AuthorEmail = c.ApplicationUser.Email,
                    c.FullPostId
                }).ToList(),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPages(int order)
        {

            return Json(context.FacebookPostPages.Include("ApplicationUser").OrderBy(c => c.ID)
                .Skip(10 * order).Take(10).ToList().Select(
                c => new
                {
                    c.Link,
                    AuthorEmail = c.ApplicationUser.Email
                }).ToList(),
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> HasLikedPagePostAsync(string contentLink,
            bool increasePointsIfLiked = false)
        {
            FacebookPagePost content = context.FacebookPagePosts.SingleOrDefault(c => c.Link == contentLink);
            List<string> likers = await content.GetLikersAsync();
            bool hasLiked = likers.Any(liker => liker == CurrentUser.Name);
            if (hasLiked && increasePointsIfLiked)
            {
                lock (CurrentUser)
                {
                    CurrentUser.Points += 10;
                }
                await context.SaveChangesAsync();
            }
            return Json(hasLiked, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> HasLikedPageAsync(string contentLink,
            bool increasePointsIfLiked = false)
        {
            FacebookPostPage content = context.FacebookPostPages.SingleOrDefault(c => c.Link == contentLink);
            string accessToken = CurrentUser.AccessTokens.First(a => a.Provider == "Facebook").AccessTokenValue;
            List<string> likers = await content.GetLikersAsync(accessToken);
            bool hasLiked = likers.Any(liked => liked == content.PageId);
            if (hasLiked && increasePointsIfLiked)
            {
                lock (CurrentUser)
                {
                    CurrentUser.Points += 10;
                }
                await context.SaveChangesAsync();
            }
            return Json(hasLiked, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Pages()
        {
            return View();
        }

        public ActionResult AddPage()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("AddPage")]
        public async Task<ActionResult> AddPageAsync(FacebookPostPage page)
        {
            if (ModelState.IsValid)
            { 
                page.ApplicationUser = CurrentUser;
                context.FacebookPostPages.Add(page);
                await context.SaveChangesAsync();
                return RedirectToAction("Pages");
            }

            return View();
        }
        public ActionResult AddPagePost()
        {

            return View();
        }

        public async Task<JsonResult> SaveAccessTokenAsync(string accessToken, string name)
        {
            AccessToken userAccessToken = CurrentUser.AccessTokens.FirstOrDefault(p => p.Provider == "Facebook");
            if (userAccessToken == null)
            {
                userAccessToken = new AccessToken
                {
                    AccessTokenValue = accessToken,
                    Provider = "Facebook"
                };
                CurrentUser.AccessTokens.Add(userAccessToken);
            }
            else
            {
                userAccessToken.AccessTokenValue = accessToken;
            }
            CurrentUser.Name = name;
            await context.SaveChangesAsync();
            return Json(true, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("AddPagePost")]
        public async Task<ActionResult> AddPagePostAsync(FacebookPagePost post)
        {
            post.Page.ApplicationUser = CurrentUser;
            if (context.FacebookPostPages.Any(p => p.PageId == post.Page.PageId))
            {
                post.Page = context.FacebookPostPages.First(p => p.PageId == post.Page.PageId);
                ModelState["Page.Link"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                post.ApplicationUser = CurrentUser;
                context.FacebookPagePosts.Add(post);
                await context.SaveChangesAsync();
                return RedirectToAction("PagePosts");
            }
            return View(post);
        }
    }
}