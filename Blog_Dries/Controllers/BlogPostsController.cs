using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog_Dries.Data;
using Blog_Dries.Models;
using Blog_Dries.Interfaces;

namespace Blog_Dries.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogPostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index()
        {
            var blogsposts = _context.BlogPosts.Include(b => b.Categoryid);
            if (User.IsInRole("Admin"))
            {
                return View(await blogsposts.ToListAsync());
            }
            else
            {
                return View(await blogsposts.Where(a => a.Author == User.Identity.Name).ToListAsync());
            }
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.Categoryid)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            var blogpost = new DetailsPostInterface
            {
                Title = blogPost.Title,
                Descritption = blogPost.Descritption,
                Category_Id = blogPost.Categoryid.CategoryName,
                Datetime = blogPost.Datetime,
                Author = blogPost.Author,
                Username = blogPost.Username,
                PostImage = _context.BlogPostImages.Where(a => a.BlogPost_Id == id).FirstOrDefault().ImageData
            };

            return View(blogpost);
        }

        // GET: BlogPosts/Create
        public IActionResult Create()
        {
            ViewData["Category_Id"] = new SelectList(_context.Categories, "Id", "CategoryName");
            return View();
        }

        // POST: BlogPosts/Create
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Descritption,Category_Id,Datetime,Author,Username,PostImage")] CreatePostInterface blogPost)
        {
            var blogpost = new BlogPost();
            if (ModelState.IsValid)
            {
                var user = _context.ApplicationUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
                blogpost = new BlogPost
                {
                    Title = blogPost.Title,
                    Descritption = blogPost.Descritption,
                    Category_Id = blogPost.Category_Id,
                    Datetime = DateTime.Now,
                    Author = User.Identity.Name,
                    Username = user.Firstname + " " + user.Lastname
                };
                _context.Add(blogpost);
                await _context.SaveChangesAsync();
                var blogphoto = new BlogPostImage
                {
                    ImageData = blogPost.PostImage,
                    BlogPost_Id = blogpost.Id
                };
                _context.BlogPostImages.Add(blogphoto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category_Id"] = new SelectList(_context.Categories, "Id", "CategoryName", blogPost.Category_Id);
            return View(blogpost);
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            ViewData["Category_Id"] = new SelectList(_context.Categories, "Id", "CategoryName", blogPost.Category_Id);
            var blogpost = new CreatePostInterface
            {
                Id = (int)id,
                Title = blogPost.Title,
                Descritption = blogPost.Descritption,
                Category_Id = blogPost.Category_Id,
                Datetime = blogPost.Datetime,
                Author = blogPost.Author,
                Username = blogPost.Username,
                PostImage = _context.BlogPostImages.Where(a => a.BlogPost_Id == id).FirstOrDefault().ImageData
            };
            return View(blogpost);
        }

        // POST: BlogPosts/Edit/5
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Descritption,Category_Id,Datetime,Author,Username,PostImage")] CreatePostInterface blogPost)
        {
            var blogpost = new BlogPost();
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = _context.ApplicationUsers.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

                try
                {
                    blogpost = new BlogPost
                    {
                        Id = blogPost.Id,
                        Title = blogPost.Title,
                        Descritption = blogPost.Descritption,
                        Category_Id = blogPost.Category_Id,
                        Datetime = DateTime.Now,
                        Author = User.Identity.Name,
                        Username = user.Firstname + " " + user.Lastname
                    };
                    _context.Update(blogpost);
                    await _context.SaveChangesAsync();
                    var blogphoto = new BlogPostImage
                    {
                        ImageData = blogPost.PostImage,
                        BlogPost_Id = blogpost.Id
                    };
                    _context.BlogPostImages.Update(blogphoto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category_Id"] = new SelectList(_context.Categories, "Id", "CategoryName", blogPost.Category_Id);
            return View(blogpost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.Categoryid)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult SearchPosts(string searchTitle)
        {
            var posts = _context.BlogPosts.Where(a => a.Title.StartsWith(searchTitle)).ToList();
            var newblogposts = new List<CreatePostInterface>();
            foreach (var item in posts)
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
    }
}
