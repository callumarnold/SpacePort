using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SP.DataManager.Data;
using SP.DataManager.Data.DataAccess;
using SP.DataManager.Models;

namespace SP.DataManager.Controllers
{
    public class DockManagersController : Controller
    {
        private readonly SPDataContext _context;
        private readonly IDockManagersDataAccess _dockManagersDataAccess;


        public DockManagersController(SPDataContext context, IDockManagersDataAccess dockManagersDataAccess)
        {
            _context = context;
            _dockManagersDataAccess = dockManagersDataAccess;
        }

        // GET: DockManagers
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Index()
        {
            //return View(await _context.GetDockManagers());
            return View(await _dockManagersDataAccess.GetDockManagers());
        }

        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.DockManagers.ToListAsync());
        //}

        // GET: DockManagers/Details/5
        [Authorize(Roles = "Manager, Admin")]
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

        //public async Task<IActionResult> Details(int? id)
        //{
        //    var dockManager = await _context.GetDockManagerById(id);
        //    if(dockManager == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return View(dockManager);
        //    }
        //}

        // GET: DockManagers/Create
        [Authorize(Roles = "Manager, Admin")]
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
        [Authorize(Roles = "Manager, Admin")]
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
        [Authorize(Roles = "Manager, Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
