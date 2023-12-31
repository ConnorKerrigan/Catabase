﻿using System;
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
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<CatabaseUser> _userManager;

        public ProfilesController(ApplicationDbContext context, UserManager<CatabaseUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Profiles.Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Profiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .Include(p => p.User).Include(p => p.User.Posts).Include(p => p.Follows)
                .FirstOrDefaultAsync(m => m.ProfileId == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // GET: Profiles/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.CatabaseUsers, "Id", "Id");
            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Create([Bind("ProfileId,ProfilePicPath,UserId")] Profile profile)
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    profile.User = user;
                    profile.UserId = user.Id;
                    _context.Add(profile);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(profile);
            }
            return View(profile);
        }
        [Authorize]
        public async Task<IActionResult> FollowUser(int profileId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("~/Identity/Account/Login");//redirect to login if not logged in
            }
            if (_context.Follows.Where(l => l.User == user).Where(l => l.Profile.ProfileId == profileId).Count() <= 0)
            {
                //follow instance does not already exist
                var profile = _context.Profiles.SingleOrDefault(c => c.ProfileId == profileId);
                var follow = new Follow
                {
                    ProfileId = profileId,
                    User = user
                };
                _context.Add(follow);//create follow instance between current user and selected user

                
            }
            else if(_context.Follows.Where(l => l.User == user).Where(l => l.Profile.ProfileId == profileId).Count() > 0)
            {
                //if we get here, the follow already exists, therefore unfollow.
                var follow = _context.Follows.Where(l => l.UserId == user.Id).Where(l => l.ProfileId == profileId).Select(l => l);
                _context.RemoveRange(follow);
            }
            await _context.SaveChangesAsync();
            var profile1 = await _context.Profiles.FindAsync(profileId);
            return RedirectToAction("Details", "Profiles", new {id=profileId});
        }
        // GET: Profiles/Edit/5
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.CatabaseUsers, "Id", "Id", profile.UserId);
            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("ProfileId,ProfilePicPath,UserId")] Profile profile)
        {
            if (id != profile.ProfileId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.ProfileId))
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
            ViewData["UserId"] = new SelectList(_context.CatabaseUsers, "Id", "Id", profile.UserId);
            return View(profile);
        }

        // GET: Profiles/Delete/5
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProfileId == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Profiles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Profiles'  is null.");
            }
            var profile = await _context.Profiles.FindAsync(id);
            if (profile != null)
            {
                _context.Profiles.Remove(profile);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileExists(int id)
        {
          return (_context.Profiles?.Any(e => e.ProfileId == id)).GetValueOrDefault();
        }
    }
}
