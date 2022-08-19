using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Dries.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required,StringLength(50)]
        public string User_name { get; set; }
        [Required, StringLength(100)]
        public string Firstname { get; set; }
        [Required, StringLength(100)]
        public string Lastname { get; set; }
        [Required]
        public int Age { get; set; }

    }
}
