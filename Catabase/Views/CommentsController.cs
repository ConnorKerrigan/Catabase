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
    }
}
