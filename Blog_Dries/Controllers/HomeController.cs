using Blog_Dries.Data;
using Blog_Dries.Interfaces;
using Blog_Dries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Dries.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var blogposts = _context.BlogPosts.ToList();
            var newblogposts = new List<CreatePostInterface>(); 
            foreach (var item in blogposts)
            {
                var images = _context.BlogPostImages.Where(a => a.BlogPost_Id == item.Id).FirstOrDefault();
                var post = new CreatePostInterface
                {
                    Id = item.Id,
                    Title = item.Title,
                    Descritption = item.Descritption,
                    Datetime = item.Datetime,
                    Username = item.Username,
                    Author = item.Author,
                    Category_Id = item.Category_Id,
                    PostImage = images.ImageData
                };
                newblogposts.Add(post);
            }
            return View(newblogposts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
