using SocialMediasAssistant.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace SocialMediasAssistant.Models
{
    public class Content
    {   public int ID { get; set; }
        public string Link { set; get; }
        public virtual ApplicationUser ApplicationUser { get; set; }  
    }
}