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
                CurrentUser.Points += 10;
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
                CurrentUser.Points += 10;
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
        public async Task<ActionResult> AddPageAsync(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                string pageId = form["Page.PageId"];
                if (!context.FacebookPostPages.Any(p => p.PageId == pageId))
                {
                    FacebookPostPage page = new FacebookPostPage
                    {
                        PageId = pageId,
                        AccessToken = form["Page.AccessToken"],
                        ApplicationUser = CurrentUser,
                        Link = "https://facebook.com/" + pageId
                    };
                    context.FacebookPostPages.Add(page);
                    await context.SaveChangesAsync();
                    return RedirectToAction("Pages");
                }
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
        public async Task<ActionResult> AddPagePostAsync(FormCollection form)
        {
            FacebookPagePost content = new FacebookPagePost();
            string pageId = form["Page.PageId"];
            if (ModelState.IsValid)
            {

                if (context.FacebookPostPages == null)
                {
                    content.Page = new FacebookPostPage();
                    content.Page.PageId = pageId;
                    content.Page.ApplicationUser = CurrentUser;
                    content.Page.Link = "https://facebook.com/" + pageId;
                }
                else if (!context.FacebookPostPages.Any(p => p.PageId == pageId))
                {
                    content.Page = new FacebookPostPage();
                    content.Page.PageId = pageId;
                    content.Page.ApplicationUser = CurrentUser;
                    content.Page.Link = "https://facebook.com/" + pageId;
                }
                else
                {
                    content.Page = context.FacebookPostPages.SingleOrDefault(p => p.PageId == pageId);
                }
                content.Page.AccessToken = form["Page.AccessToken"];
                content.Link = form["Link"];
                content.ApplicationUser = CurrentUser;
                context.FacebookPagePosts.Add(content);
                await context.SaveChangesAsync();
                return RedirectToAction("PagePosts");
            }
            return View();
        }
    }
}