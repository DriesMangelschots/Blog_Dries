using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Dries.Models
{
    public class BlogPostImage
    {
        [Key]
        public int Id { get; set; }
        public string ImageData { get; set; }

        [ForeignKey("Blogpostid")]
        public int BlogPost_Id { get; set; } 
        public BlogPost Blogpostid { get; set; }
    }
}
