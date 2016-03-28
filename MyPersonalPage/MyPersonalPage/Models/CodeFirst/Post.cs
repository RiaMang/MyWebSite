using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPersonalPage.Models
{
    public class Post
    {
        
        public int Id { get; set; }
        //[DisplayFormat(DataFormatString="{0:dd/mm/yy}", ApplyFormatInEditMode=true)]
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [AllowHtml]
        public string Body { get; set; }
        public string Cat { get; set; }
        public string MediaURL { get; set; }
        
        public string Slug { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}