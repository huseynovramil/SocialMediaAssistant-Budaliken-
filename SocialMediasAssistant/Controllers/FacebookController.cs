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

            return Json(context.GetFacebookPagePosts(CurrentUser.UserName)
                .Skip(10 * order).Take(10).ToList().Select(
                c => new
                {
                    c.Link,
                    AuthorEmail = c.ApplicationUser.UserName,
                    c.FullPostId
                }).ToList(),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPages(int order)
        {

            return Json(context.GetFacebookPostPages(CurrentUser.UserName)
                .Skip(10 * order).Take(10).ToList().Select(
                c => new
                {
                    c.Link,
                    AuthorEmail = c.ApplicationUser.UserName
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

        public async Task<JsonResult> GetNumberOfLikes(string link, int numberOfPrevLikes = 0, bool increasePoints = false)
        {
            FacebookPostPage page = context.FacebookPostPages
                .FirstOrDefault(c => c.Link == link);
            int count = await page.GetNumberOfLikes();
            if (increasePoints)
            {
                if(count > numberOfPrevLikes)
                {
                    lock (CurrentUser)
                    {
                        CurrentUser.Points += 10;
                        context.SaveChangesAsync();
                    }
                    
                }
            }
            return Json(count, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> AddPageAsync(FacebookPostPage page, FormCollection form)
        {
            page.ApplicationUser = CurrentUser;
            if(page.ApplicationUser.Points - page.Points < 0)
            {
                ModelState.AddModelError("points", "You do not have enough points, Please earn points");
            }
            if (ModelState.IsValid)
            { 
                context.FacebookPostPages.Add(page);
                lock (page.ApplicationUser) {
                page.ApplicationUser.Points -= (int)page.Points;
                }
                await context.SaveChangesAsync();
                return RedirectToAction("Pages");
            }

            return View(page);
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
            post.ApplicationUser = CurrentUser;
            if(post.ApplicationUser.Points-post.Points < 0)
            {
                ModelState.AddModelError("points", "You do not have enough points, Please earn points");
            }
            if (context.FacebookPostPages.Any(p => p.PageId == post.Page.PageId))
            {
                post.Page = context.FacebookPostPages.First(p => p.PageId == post.Page.PageId);
                ModelState["Page.Link"].Errors.Clear();
            }
            post.Page.Points = post.Points;
            ModelState["Page.Points"].Errors.Clear();
            if (ModelState.IsValid)
            {
                context.FacebookPagePosts.Add(post);
                lock (post.ApplicationUser)
                {
                    post.ApplicationUser.Points -= post.Points;
                }
                try
                {
                    await context.SaveChangesAsync();
                }
                catch(DbEntityValidationException e)
                {

                }
                return RedirectToAction("PagePosts");
            }
            return View(post);
        }
    }
}