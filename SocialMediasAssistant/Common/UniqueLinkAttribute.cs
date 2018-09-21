using SocialMediasAssistant.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialMediasAssistant.Common
{
    public class UniqueLinkAttribute : ValidationAttribute
    {
        //private bool replace;
        //public UniqueLinkAttribute(bool replace)
        //{
        //    this.replace = replace;
        //}
        public override bool IsValid(object value)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            if (context.Contents.Any(c => c.Link == value.ToString()))
            {
                return false;
            }
            else
            {   
                return true;
            }
        }


    }
}