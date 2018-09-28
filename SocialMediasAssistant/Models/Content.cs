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
        [UniqueLink(ErrorMessage = "You have already added this post or page")]
        [Required]
        [Index(IsUnique = true)]
        [StringLength(100)]
        public string Link { set; get; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Range(5, 100)]
        [Required]
        public int Points { get; set; }
    }
}