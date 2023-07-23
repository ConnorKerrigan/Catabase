using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Catabase.Data;
using Catabase.Models;
using Microsoft.AspNetCore.Identity;
using System.IO;
using System.Web;
using Microsoft.Extensions.Hosting;
using Humanizer;
using Microsoft.AspNetCore.Authorization;

namespace Catabase.Views
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<CatabaseUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PostsController(ApplicationDbContext context, UserManager<CatabaseUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var posts = _context.Posts
                .Include(p => p.Likes)
                .Include(p => p.PostAttributions)
                .Include(p => p.CatabaseUser.Profile.Follows)
                .AsNoTracking();

            foreach (var p in posts)
            {
                if (p.Likes != null)
                {
                    p.LikeCount = _context.Likes.Where(l => l.Post.PostId == p.PostId).Count();
                    _context.Update(p);
                    
                }
            }
            await _context.SaveChangesAsync();
            
            return posts != null ?
                      View(await _context.Posts.ToListAsync()) :
                      Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
        }

        [Authorize]
        public async Task<IActionResult> Following()
        {
            var posts = _context.Posts
                .Include(p => p.Likes)
                .Include(p => p.PostAttributions)
                .Include(p => p.CatabaseUser.Profile.Follows)
                .AsNoTracking();

            foreach (var p in posts)
            {
                if (p.Likes != null)
                {
                    p.LikeCount = _context.Likes.Where(l => l.Post.PostId == p.PostId).Count();
                    _context.Update(p);

                }
            }
            await _context.SaveChangesAsync();

            return posts != null ?
                      View(await _context.Posts.ToListAsync()) :
                      Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
        }


        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(p => p.CatabaseUser.Profile).Include(p=> p.PostAttributions)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public IActionResult Create()
        {

            var items = _context.Cats.ToList();
            if (items != null)
            {
                ViewBag.data = items.Where(c => c.OwnerID == _userManager.GetUserId(User));
            }
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("PostId,Caption,ImageUrl,LikeCount,PostTime")] Post post, int[] catId, [Bind("file")] IFormFile file)
        {
            var allowedExtensions = new[] {
                ".Jpg", ".png", ".jpg", "jpeg"
            };
            string webRootPath = _webHostEnvironment.WebRootPath;
            if (!ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                post.CatabaseUser = user;
                post.LikeCount = 0;
                post.PostTime = DateTime.Now;
                var fileName = Path.GetFileName(file.FileName);
                var ext = Path.GetExtension(file.FileName);


                if (allowedExtensions.Contains(ext)) //check what type of extension  
                {
                    foreach (var i in catId)
                    {
                        var postAttribution = new PostAttribution
                        {
                            Post = post,
                            Cat = _context.Cats.SingleOrDefault(c => c.CatId == i)

                        };
                        await _context.AddAsync(postAttribution);
                    }
                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    string myfile = name + "_" + post.PostId + ext; //appending the name with id  
                                                                    // store the file inside ~/project folder(Img)  
                    var path = Path.Combine(webRootPath, "Images", myfile);
                    post.ImageUrl = myfile;
                    _context.Add(post);

                    await _context.SaveChangesAsync();
                    using (Stream fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        fileStream.Close();
                    }

                }
                else
                {
                    ViewBag.message = "Please choose only Image file";
                }



                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }



        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Caption,ImageUrl,LikeCount,PostTime")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
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
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
