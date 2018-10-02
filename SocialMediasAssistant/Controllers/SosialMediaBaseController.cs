using Microsoft.AspNet.Identity;
using SocialMediasAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMediasAssistant.Controllers
{
    public class SocialMediaBaseController : Controller
    {
        protected ApplicationDbContext context = new ApplicationDbContext();

        protected ApplicationUser CurrentUser = new ApplicationUser();

        // GET: MediaAssistant

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userId = User.Identity.GetUserId();
            CurrentUser = context.Users.SingleOrDefault(u => u.Id == userId);
            base.OnActionExecuting(filterContext);
        }
    }
}