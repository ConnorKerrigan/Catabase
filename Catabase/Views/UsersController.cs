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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Hosting;

namespace Catabase.Views
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private UserManager<CatabaseUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<CatabaseUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? pageNumber)
        {
            var user = await _userManager.GetUserAsync(User);
            var users = _context.CatabaseUsers.Include(u=>u.Profile).Include(u=>u.Posts).Include(u=>u.Cats)
                .AsNoTracking();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.UserName.Contains(searchString));
            }

            int pageSize = 3;
            return View(await PaginatedList<CatabaseUser>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.CatabaseUsers == null)
            {
                return NotFound();
            }

            var user = await _context.CatabaseUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Cats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.CatabaseUsers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cats'  is null.");
            }
            var user = await _context.CatabaseUsers.FindAsync(id);
            if (user != null)
            {
                _context.Follows.RemoveRange(_context.Follows.Where(f=>f.UserId == user.Id || f.ProfileId == _context.Profiles.SingleOrDefault(p=>p.UserId == user.Id).ProfileId));
                _context.Likes.RemoveRange(_context.Likes.Where(f => f.UserId == user.Id || f.Post.CatabaseUserId == user.Id));
                _context.Comments.RemoveRange(_context.Comments.Where(f=>f.UserId==user.Id || f.Post.CatabaseUserId == user.Id));
                _context.Profiles.RemoveRange(_context.Profiles.Where(f => f.UserId == user.Id));
                _context.PostAttributions.RemoveRange(_context.PostAttributions.Where(f => f.Post.CatabaseUserId == user.Id));
                _context.Cats.RemoveRange(_context.Cats.Where(f => f.OwnerID == user.Id));
                _context.Posts.RemoveRange(_context.Posts.Where(f => f.CatabaseUserId == user.Id));
                _context.CatabaseUsers.Remove(user);
            }


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
