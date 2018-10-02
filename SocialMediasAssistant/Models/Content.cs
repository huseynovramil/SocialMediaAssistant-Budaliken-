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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SocialMediasAssistant.Models
{
    public class Content
    {   public int ID { get; set; }
        [UniqueLink(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "DuplicatePost")]
        [Required(ErrorMessageResourceName = "LinkRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Index(IsUnique = true)]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "NotValidLink")]
        public string Link { set; get; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Range(5, 100, ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "PointsRange")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "PointsRequired")]
        [Display(Name = "Points", ResourceType = typeof(Resources.Resource))]
        public int Points { get; set; }
    }
}