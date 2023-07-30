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
using Microsoft.Data.SqlClient;

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
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "PostTime_desc" : "";


            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            //Queries the data to obtain data which matches search string in multiple fields
            var posts = from s in _context.Posts
                .Include(p => p.Likes)
                .Include(p => p.PostAttributions)
                .Include(p => p.CatabaseUser.Profile.Follows)
                .AsNoTracking()
                        select s;

            //foreach (var p in posts)
            //{
            //    if (p.Likes != null)
            //    {
            //        p.LikeCount = _context.Likes.Where(l => l.Post.PostId == p.PostId).Count();
            //        _context.Update(p);

            //    }
            //}
            await _context.SaveChangesAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Caption.Contains(searchString)
                                       || s.CatabaseUser.UserName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "PostTime":
                    posts = posts.OrderBy(s => s.PostTime);
                    break;
                case "PostTime_desc":
                    posts = posts.OrderByDescending(s => s.PostTime);
                    break;
                default:
                    posts = posts.OrderBy(s => s.PostTime);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Post>.CreateAsync(posts.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        [Authorize]
        public async Task<IActionResult> Following(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "PostTime_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;


            var posts1 = from s in _context.Posts.Include(p => p.Likes)
                .Include(p => p.PostAttributions)
                .Include(p => p.CatabaseUser.Profile.Follows)
                .AsNoTracking()
                         select s;

            var posts = Enumerable.Empty<Post>();



            foreach (var p in posts1)
            {


                if (p.CatabaseUser.Profile.Follows.Where(f => f.UserId == user.Id).Count() > 0)
                {
                    var h = _context.Posts.Where(h => h == p).AsNoTracking();
                    posts = posts.Concat(h);
                }
            }

            //Queries the data to obtain data which matches search string in multiple fields
            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Caption.Contains(searchString)
                                       || s.CatabaseUser.UserName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "PostTime":
                    posts = posts.OrderBy(s => s.PostTime);
                    break;
                case "PostTime_desc":
                    posts = posts.OrderByDescending(s => s.PostTime);
                    break;
                default:
                    posts = posts.OrderBy(s => s.PostTime);
                    break;
            }
            int pageSize = 3;
            return View(PaginatedList<Post>.Create(posts.AsQueryable().AsNoTracking(), pageNumber ?? 1, pageSize));

        }


        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(p => p.CatabaseUser.Profile).Include(p => p.PostAttributions)
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
            long maxFileSize = 18000000;
            long fileSize = file.Length;
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

                if (fileSize > maxFileSize)
                {
                    return Content("File is too large (Max 18mb)");
                }
                else
                {
                    if (allowedExtensions.Contains(ext)) //confirm valid extension 
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
                        string name = Path.GetFileNameWithoutExtension(fileName); //filename without extension  
                        string myfile = name + "_" + post.PostId + ext; //append id to the name 
                                                                        // store inside root folder (Images)  
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
                        return Content("Please only use supported filetypes (.Jpg, .jpg, .png, .jpeg)");
                    }

                }

                
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null || user.Id != post.CatabaseUser.Id)
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
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Caption")] Post post)
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
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null || user.Id != post.CatabaseUser.Id)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
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
