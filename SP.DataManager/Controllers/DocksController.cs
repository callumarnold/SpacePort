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
    public class DocksController : Controller
    {
        private readonly IDockManagersDataAccess _dockManagersDataAccess;
        private readonly IDocksDataAccess _docksDataAccess;


        public DocksController(IDockManagersDataAccess dockManagersDataAccess, IDocksDataAccess docksDataAccess)
        {
            _dockManagersDataAccess = dockManagersDataAccess;
            _docksDataAccess = docksDataAccess;
        }

        // GET: Docks
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _docksDataAccess.GetDocks());
        }

        // GET: Docks/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            var docks = await _docksDataAccess.GetDockById(id);
            if (docks == null)
            {
                return NotFound();
            }
            else
            {
                return View(docks);
            }
        }

        // GET: Docks/Create
        [Authorize(Roles = "Manager, Admin")]
        public IActionResult Create()
        {
            //ViewData["ManagerId"] = new SelectList(_context.DockManagers, "Id", "FirstName");
            ViewData["ManagerId"] = new SelectList(_dockManagersDataAccess.ReturnDockManagers(), "Id", "FirstName");
            return View();
        }

        // POST: Docks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,ManagerId,MaxCapacity,CurrentCapacity")] Docks docks)
        {
            if (ModelState.IsValid)
            {
                await _docksDataAccess.CreateDock(docks);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_dockManagersDataAccess.ReturnDockManagers(), "Id", "FirstName", docks.ManagerId);
            return View(docks);
        }

        // GET: Docks/Edit/5
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docks = await _docksDataAccess.GetDockById(id);
            if (docks == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_dockManagersDataAccess.ReturnDockManagers(), "Id", "FirstName", docks.ManagerId);
            return View(docks);
        }

        // POST: Docks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager, Admin")]
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
                    await _docksDataAccess.EditDocks(docks);
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
            ViewData["ManagerId"] = new SelectList(_dockManagersDataAccess.ReturnDockManagers(), "Id", "FirstName", docks.ManagerId);
            return View(docks);
        }

        [Authorize(Roles = "Admin")]
        // GET: Docks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docks = await _docksDataAccess.GetDockById(id);
            if (docks == null)
            {
                return NotFound();
            }

            return View(docks);
        }

        // POST: Docks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _docksDataAccess.DeleteDock(id);
            return RedirectToAction(nameof(Index));
        }

        private bool DocksExists(int id)
        {
            return _docksDataAccess.CheckDockExists(id);
        }
    }
}
