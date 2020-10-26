using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SP.DataManager.Data;
using SP.DataManager.Models;

namespace SP.DataManager.Controllers
{
    public class SpaceshipsController : Controller
    {
        private readonly SPDataContext _context;

        public SpaceshipsController(SPDataContext context)
        {
            _context = context;
        }

        // GET: Spaceships
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var sPDataContext = _context.Spaceships.Include(s => s.Dock);
            return View(await sPDataContext.ToListAsync());
        }

        // GET: Spaceships/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spaceships = await _context.Spaceships
                .Include(s => s.Dock)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spaceships == null)
            {
                return NotFound();
            }

            return View(spaceships);
        }

        // GET: Spaceships/Create
        [Authorize(Roles = "Manager, Admin")]
        public IActionResult Create()
        {
            ViewData["DockId"] = new SelectList(_context.Docks, "Id", "Name");
            return View();
        }

        // POST: Spaceships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("Id,Name,Owner,CrewSize,DockId")] Spaceships spaceships)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spaceships);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DockId"] = new SelectList(_context.Docks, "Id", "Name", spaceships.DockId);
            return View(spaceships);
        }

        // GET: Spaceships/Edit/5
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spaceships = await _context.Spaceships.FindAsync(id);
            if (spaceships == null)
            {
                return NotFound();
            }
            ViewData["DockId"] = new SelectList(_context.Docks, "Id", "Name", spaceships.DockId);
            return View(spaceships);
        }

        // POST: Spaceships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Owner,CrewSize,DockId")] Spaceships spaceships)
        {
            if (id != spaceships.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spaceships);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpaceshipsExists(spaceships.Id))
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
            ViewData["DockId"] = new SelectList(_context.Docks, "Id", "Name", spaceships.DockId);
            return View(spaceships);
        }

        // GET: Spaceships/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spaceships = await _context.Spaceships
                .Include(s => s.Dock)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spaceships == null)
            {
                return NotFound();
            }

            return View(spaceships);
        }

        // POST: Spaceships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spaceships = await _context.Spaceships.FindAsync(id);
            _context.Spaceships.Remove(spaceships);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpaceshipsExists(int id)
        {
            return _context.Spaceships.Any(e => e.Id == id);
        }
    }
}
