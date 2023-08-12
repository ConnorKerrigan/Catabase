using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Catabase.Data;
using Catabase.Models;
using Microsoft.AspNetCore.Authorization;

namespace Catabase.Views
{
    public class PostAttributionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostAttributionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PostAttributions
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Index()
        {
            var postAttributions = _context.PostAttributions
                .Include(p=>p.Post)
                .Include(p=>p.Cat);
              return postAttributions != null ? 
                          View(await _context.PostAttributions.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.PostAttributions'  is null.");
        }

        // GET: PostAttributions/Details/5
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PostAttributions == null)
            {
                return NotFound();
            }

            var postAttribution = await _context.PostAttributions
                .FirstOrDefaultAsync(m => m.PostAttributionId == id);
            if (postAttribution == null)
            {
                return NotFound();
            }

            return View(postAttribution);
        }

        // GET: PostAttributions/Create
        [Authorize(Policy = "RequireAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostAttributions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Create([Bind("PostAttributionId")] PostAttribution postAttribution)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(postAttribution);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postAttribution);
        }

        // GET: PostAttributions/Edit/5
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PostAttributions == null)
            {
                return NotFound();
            }

            var postAttribution = await _context.PostAttributions.FindAsync(id);
            if (postAttribution == null)
            {
                return NotFound();
            }
            return View(postAttribution);
        }

        // POST: PostAttributions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("PostAttributionId")] PostAttribution postAttribution)
        {
            if (id != postAttribution.PostAttributionId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(postAttribution);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostAttributionExists(postAttribution.PostAttributionId))
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
            return View(postAttribution);
        }

        // GET: PostAttributions/Delete/5
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PostAttributions == null)
            {
                return NotFound();
            }

            var postAttribution = await _context.PostAttributions
                .FirstOrDefaultAsync(m => m.PostAttributionId == id);
            if (postAttribution == null)
            {
                return NotFound();
            }

            return View(postAttribution);
        }

        // POST: PostAttributions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PostAttributions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PostAttributions'  is null.");
            }
            var postAttribution = await _context.PostAttributions.FindAsync(id);
            if (postAttribution != null)
            {
                _context.PostAttributions.Remove(postAttribution);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostAttributionExists(int id)
        {
          return (_context.PostAttributions?.Any(e => e.PostAttributionId == id)).GetValueOrDefault();
        }
    }
}
