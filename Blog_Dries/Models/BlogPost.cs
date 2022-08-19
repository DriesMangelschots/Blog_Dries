using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Dries.Models
{
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Descritption { get; set; }

        [ForeignKey("Categoryid")]
        public int Category_Id { get; set; }
        
        public Category Categoryid { get; set; }
        public DateTime Datetime { get; set; }
        [Required]
        public string Author { get; set; }
        public string Username { get; set; }

    }
}
