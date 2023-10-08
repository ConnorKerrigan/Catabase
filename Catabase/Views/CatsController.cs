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
    public class CatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private UserManager<CatabaseUser> _userManager;

        public CatsController(ApplicationDbContext context, UserManager<CatabaseUser> userManager)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            _context = context;
            _userManager = userManager;
        }

        // GET: Cats
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cats = _context.Cats.Where(c => c.OwnerID == user.Id)
                .Include(c => c.Owner)
                .AsNoTracking();
            return cats != null ?
                        View(await cats.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Cats'  is null.");
        }

        // GET: Cats/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cats == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats
                .Include(c => c.Owner)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }

        // GET: Cats/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("CatId,Name,Breed,Sex,Colour,Bio,DateOfBirth")] Cat cat)
        {
            if (!ModelState.IsValid)
            {
                if (cat.DateOfBirth > DateTime.Now)
                {
                    ModelState.AddModelError("", "Time travelling cats are not allowed!!");
                    return View(cat);
                }
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    cat.Owner = user;
                    _context.Add(cat);
                    await _context.SaveChangesAsync();

                }

                return RedirectToAction(nameof(Index));
            }
            return View(cat);
        }

        // GET: Cats/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id == null || _context.Cats == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats.FindAsync(id);
            if (cat == null)
            {
                return NotFound();
            }
            if (_context.UserRoles.Where(ur => ur.UserId == user.Id).Where(ur => ur.RoleId == _context.Roles.SingleOrDefault(r => r.Name == "Admin").Id).Count() <= 0)
            {
                if (user.Id != cat.OwnerID)
                {
                    return NotFound();
                }
            }
            return View(cat);
        }

        // POST: Cats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("CatId,Name,Breed,Sex,Colour,Bio,DateOfBirth")] Cat cat)
        {
            if (id != cat.CatId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    cat.OwnerID = _userManager.GetUserId(User);
                    _context.Update(cat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatExists(cat.CatId))
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
            return View(cat);
        }

        // GET: Cats/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cats == null)
            {
                return NotFound();
            }

            var cat = await _context.Cats
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }

        // POST: Cats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cats == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cats'  is null.");
            }
            var cat = await _context.Cats.FindAsync(id);
            if (cat != null)
            {
                _context.Cats.Remove(cat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatExists(int id)
        {
            return (_context.Cats?.Any(e => e.CatId == id)).GetValueOrDefault();
        }
    }
}
