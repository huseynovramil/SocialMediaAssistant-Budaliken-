using Microsoft.AspNet.Identity.EntityFramework;
using SocialMediasAssistant.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SocialMediasAssistant.Models
{
    public class AccessToken
    {
        public string Provider { get; set; }
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(500)]
        public string DecryptedAccessToken { get; set; }
        [NotMapped]
        public string AccessTokenValue
        {
            get
            {
                if (DecryptedAccessToken != null)
                    return Cipher.Decrypt(DecryptedAccessToken, ConfigurationManager.AppSettings["cipherPassword"]);
                else
                    return null;
            }
            set
            {
                if (value != null)
                    DecryptedAccessToken = Cipher.Encrypt(value, ConfigurationManager.AppSettings["cipherPassword"]);
                else
                    DecryptedAccessToken = "not-set";
            }
        }
    }
}