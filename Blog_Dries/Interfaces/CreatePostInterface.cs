using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Dries.Interfaces
{
    public class CreatePostInterface
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Descritption { get; set; }
        public int Category_Id { get; set; }
        public DateTime Datetime { get; set; }
        
        public string Author { get; set; }
        public string Username { get; set; }

        public string PostImage { get; set; }

    }

    public class DetailsPostInterface
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Descritption { get; set; }
        public string Category_Id { get; set; }
        public DateTime Datetime { get; set; }

        public string Author { get; set; }
        public string Username { get; set; }

        public string PostImage { get; set; }

    }
}
