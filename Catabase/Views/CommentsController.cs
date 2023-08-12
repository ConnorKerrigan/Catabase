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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace Catabase.Views
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<CatabaseUser> _userManager;

        public CommentsController(ApplicationDbContext context, UserManager<CatabaseUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(string commentText, int postId)
        {
            var user = await _userManager.GetUserAsync(User);
            var uId = await _userManager.GetUserIdAsync(user);
            var comment = new Comment
            {
                UserId = uId,
                PostId = postId,
                CommentContent = commentText
            };
            await _context.AddAsync(comment);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", "Posts", new { id = postId });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id == null || _context.Comments == null || user == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.Include(c=>c.User).Include(c=>c.Post)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                Json(new { value = "comment null" });
            }
            if (_context.UserRoles.Where(ur => ur.UserId == user.Id).Where(ur => ur.RoleId == _context.Roles.SingleOrDefault(r => r.Name == "Admin").Id).Count() <= 0)
            {
                if (comment == null || user.Id != comment.UserId)
                {
                    return NotFound();
                }
            }

            return View(comment);
        }

        // POST: Cats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id, int postId)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Comments'  is null.");
            }
            var comment = await _context.Comments.Include(c => c.Post).Include(c => c.User).FirstOrDefaultAsync(m => m.CommentId == id);
            
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Posts", new { id = postId });
        }
    }
}
