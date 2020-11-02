using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SP.DataManager.Data;
using SP.DataManager.Data.DataAccess;
using SP.DataManager.Models;

namespace SP.DataManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DockManagersController : ControllerBase
    {
        private readonly IDockManagersDataAccess _dockManagersDataAccess;

        public DockManagersController(IDockManagersDataAccess dockManagersDataAccess)
        {
            _dockManagersDataAccess = dockManagersDataAccess;
        }

        // GET: api/DockManagers
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<DockManagers>>> GetDockManagers()
        {
            
            //return await _context.DockManagers.ToListAsync();
            return await _dockManagersDataAccess.GetDockManagers();
        }

        // GET: api/DockManagers/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<DockManagers>> GetDockManagers(int id)
        {
            //var dockManagers = await _context.DockManagers.FindAsync(id);
            var dockManagers = await _dockManagersDataAccess.GetDataManagersById(id);

            if (dockManagers == null)
            {
                return NotFound();
            }

            return dockManagers;
        }

        // PUT: api/DockManagers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutDockManagers(int id, DockManagers dockManagers)
        {
            if (id != dockManagers.Id)
            {
                return BadRequest();
            }

            //_context.Entry(dockManagers).State = EntityState.Modified;
            _dockManagersDataAccess.ApiStateModified(dockManagers);

            try
            {
                await _dockManagersDataAccess.ApiSaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DockManagersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DockManagers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<DockManagers>> PostDockManagers(DockManagers dockManagers)
        {
            //_context.DockManagers.Add(dockManagers);
            //await _context.SaveChangesAsync();
            await _dockManagersDataAccess.CreateDockManager(dockManagers);

            return CreatedAtAction("GetDockManagers", new { id = dockManagers.Id }, dockManagers);
        }

        // DELETE: api/DockManagers/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<DockManagers>> DeleteDockManagers(int id)
        {
            //var dockManagers = await _context.DockManagers.FindAsync(id);
            var dockManagers = await _dockManagersDataAccess.GetDataManagersById(id);
            if (dockManagers == null)
            {
                return NotFound();
            }

            //_context.DockManagers.Remove(dockManagers);
            //await _context.SaveChangesAsync();
            await _dockManagersDataAccess.DeleteDockManager(id);

            return dockManagers;
        }

        private bool DockManagersExists(int id)
        {
            //return _context.DockManagers.Any(e => e.Id == id);
            return _dockManagersDataAccess.CheckDockManagersExists(id);
        }
    }
}
