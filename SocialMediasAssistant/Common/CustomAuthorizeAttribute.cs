using SocialMediasAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMediasAssistant.Common
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string requestedSite = ((MvcHandler)httpContext.Handler)
                .RequestContext.RouteData
                .Values["controller"].ToString();
            string userName = httpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return base.AuthorizeCore(httpContext);
            }
            else
            {
                ApplicationDbContext context = new ApplicationDbContext();
                ApplicationUser user = context.Users.FirstOrDefault(u => u.UserName == userName);
                if (user.Logins.Any(u => u.LoginProvider == requestedSite))
                {
                    return base.AuthorizeCore(httpContext);
                }
                else
                {
                    return false;
                }
            }
        }

    }
}