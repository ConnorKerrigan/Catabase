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
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Catabase.Views
{
    public class LikesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<CatabaseUser> _userManager;

        public LikesController(ApplicationDbContext context, UserManager<CatabaseUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Likes
        public async Task<IActionResult> Index()
        {
            
              return _context.Likes != null ? 
                          View(await _context.Likes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Likes'  is null.");
        }

        // GET: Likes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Likes == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .FirstOrDefaultAsync(m => m.LikeId == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // GET: Likes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Likes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LikeId")] Like like)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(like);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(like);
        }

        public async Task<IActionResult> SubmitLike(int postId)
        {
            var user = await _userManager.GetUserAsync(User);
            var post = _context.Posts.SingleOrDefault(c => c.PostId == postId);
            if (user == null)
            {
                return Redirect("~/Identity/Account/Login");
            }
            if (_context.Likes.Where(l => l.User == user).Where(l => l.Post.PostId == postId).Count() <= 0)
            {
                
                var likeAtt = new Like
                {
                    Post = post,
                    User = user
                };
                _context.Add(likeAtt);
                
                
            }
            else if(_context.Likes.Where(l => l.User == user).Where(l => l.Post.PostId == postId).Count() > 0)
            {
                var like = _context.Likes.Where(l => l.User == user).Where(l => l.Post.PostId == postId).Select(l=>l);
                _context.RemoveRange(like);
            }
            await _context.SaveChangesAsync();
            post.LikeCount = post.Likes.Count();

            return RedirectToAction("Index", "Posts");
        }

        // GET: Likes/Edit/5
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Likes == null)
            {
                return NotFound();
            }

            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }
            return View(like);
        }

        // POST: Likes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("LikeId")] Like like)
        {
            if (id != like.LikeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(like);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LikeExists(like.LikeId))
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
            return View(like);
        }

        // GET: Likes/Delete/5
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Likes == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .FirstOrDefaultAsync(m => m.LikeId == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Likes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Likes'  is null.");
            }
            var like = await _context.Likes.FindAsync(id);
            if (like != null)
            {
                _context.Likes.Remove(like);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LikeExists(int id)
        {
          return (_context.Likes?.Any(e => e.LikeId == id)).GetValueOrDefault();
        }
    }
}
