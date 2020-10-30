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
    public class SpaceshipsController : Controller
    {
        //private readonly SPDataContext _context;
        private readonly ISpaceshipsDataAccess _spaceshipsDataAccess;
        private readonly IDocksDataAccess _docksDataAccess;

        public SpaceshipsController(/*SPDataContext context,*/ 
            ISpaceshipsDataAccess spaceshipsDataAccess, IDocksDataAccess docksDataAccess)
        {

            _spaceshipsDataAccess = spaceshipsDataAccess;
            _docksDataAccess = docksDataAccess;
        }

        // GET: Spaceships
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _spaceshipsDataAccess.GetSpaceships());
        }

        // GET: Spaceships/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            var spaceships = await _spaceshipsDataAccess.GetSpaceshipsById(id);
            if(spaceships == null)
            {
                return NotFound();
            }
            else
            {
                return View(spaceships);
            }
        }

        // GET: Spaceships/Create
        [Authorize(Roles = "Manager, Admin")]
        public IActionResult Create()
        {
            ViewData["DockId"] = new SelectList(_docksDataAccess.ReturnDocks(), "Id", "Name");
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
                await _spaceshipsDataAccess.CreateSpaceship(spaceships);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DockId"] = new SelectList(_docksDataAccess.ReturnDocks(), "Id", "Name", spaceships.DockId);
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

            var spaceships = await _spaceshipsDataAccess.GetSpaceshipsById(id);
            if (spaceships == null)
            {
                return NotFound();
            }
            ViewData["DockId"] = new SelectList(_docksDataAccess.ReturnDocks(), "Id", "Name", spaceships.DockId);
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
                    await _spaceshipsDataAccess.EditSpaceships(spaceships);
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
            ViewData["DockId"] = new SelectList(_docksDataAccess.ReturnDocks(), "Id", "Name", spaceships.DockId);
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
            var spaceships = await _spaceshipsDataAccess.GetSpaceshipsById(id);
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
            await _spaceshipsDataAccess.DeleteSpaceships(id);
            return RedirectToAction(nameof(Index));
        }

        private bool SpaceshipsExists(int id)
        {
            return _spaceshipsDataAccess.CheckSpaceshipsExists(id);
        }
    }
}
