using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SP.DataManager.Data;
using SP.DataManager.Models;

namespace SP.DataManager.Controllers
{
    public class DockManagersController : Controller
    {
        private readonly SPDataContext _context;

        public DockManagersController(SPDataContext context)
        {
            _context = context;
        }

        // GET: DockManagers
        public async Task<IActionResult> Index()
        {
            return View(await _context.DockManagers.ToListAsync());
        }

        // GET: DockManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dockManagers = await _context.DockManagers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dockManagers == null)
            {
                return NotFound();
            }

            return View(dockManagers);
        }

        // GET: DockManagers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DockManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName")] DockManagers dockManagers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dockManagers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dockManagers);
        }

        // GET: DockManagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dockManagers = await _context.DockManagers.FindAsync(id);
            if (dockManagers == null)
            {
                return NotFound();
            }
            return View(dockManagers);
        }

        // POST: DockManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName")] DockManagers dockManagers)
        {
            if (id != dockManagers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dockManagers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DockManagersExists(dockManagers.Id))
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
            return View(dockManagers);
        }

        // GET: DockManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dockManagers = await _context.DockManagers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dockManagers == null)
            {
                return NotFound();
            }

            return View(dockManagers);
        }

        // POST: DockManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dockManagers = await _context.DockManagers.FindAsync(id);
            _context.DockManagers.Remove(dockManagers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DockManagersExists(int id)
        {
            return _context.DockManagers.Any(e => e.Id == id);
        }
    }
}
