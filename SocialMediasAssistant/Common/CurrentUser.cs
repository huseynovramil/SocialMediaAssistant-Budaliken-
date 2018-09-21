using SocialMediasAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediasAssistant.Common
{
    public static class CurrentUser
    {
        public static ApplicationUser GetCurrentUser(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            return context.Users.First(u => u.Id == id);
        }
    }
}