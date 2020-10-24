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
    public class DocksController : Controller
    {
        private readonly SPDataContext _context;

        public DocksController(SPDataContext context)
        {
            _context = context;
        }

        // GET: Docks
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var sPDataContext = _context.Docks.Include(d => d.Manager);
            return View(await sPDataContext.ToListAsync());
        }

        // GET: Docks/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docks = await _context.Docks
                .Include(d => d.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docks == null)
            {
                return NotFound();
            }

            return View(docks);
        }

        // GET: Docks/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.DockManagers, "Id", "FirstName");
            return View();
        }

        // POST: Docks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,ManagerId,MaxCapacity,CurrentCapacity")] Docks docks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(docks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.DockManagers, "Id", "FirstName", docks.ManagerId);
            return View(docks);
        }

        // GET: Docks/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docks = await _context.Docks.FindAsync(id);
            if (docks == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.DockManagers, "Id", "FirstName", docks.ManagerId);
            return View(docks);
        }

        // POST: Docks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ManagerId,MaxCapacity,CurrentCapacity")] Docks docks)
        {
            if (id != docks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocksExists(docks.Id))
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
            ViewData["ManagerId"] = new SelectList(_context.DockManagers, "Id", "FirstName", docks.ManagerId);
            return View(docks);
        }

        [Authorize]
        // GET: Docks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docks = await _context.Docks
                .Include(d => d.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docks == null)
            {
                return NotFound();
            }

            return View(docks);
        }

        // POST: Docks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docks = await _context.Docks.FindAsync(id);
            _context.Docks.Remove(docks);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocksExists(int id)
        {
            return _context.Docks.Any(e => e.Id == id);
        }
    }
}
