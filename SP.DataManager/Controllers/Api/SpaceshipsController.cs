using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public class SpaceshipsController : ControllerBase
    {
        private readonly ISpaceshipsDataAccess _spaceshipsDataAccess;
        private readonly IDocksDataAccess _docksDataAccess;

        public SpaceshipsController(ISpaceshipsDataAccess spaceshipsDataAccess, IDocksDataAccess docksDataAccess)
        {
            _spaceshipsDataAccess = spaceshipsDataAccess;
            _docksDataAccess = docksDataAccess;
        }

        // GET: api/Spaceships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Spaceships>>> GetSpaceships()
        {
            //return await _context.Spaceships.ToListAsync();
            return await _spaceshipsDataAccess.GetSpaceships();
        }

        // GET: api/Spaceships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Spaceships>> GetSpaceships(int id)
        {
            //var spaceships = await _context.Spaceships.FindAsync(id);
            var spaceships = await _spaceshipsDataAccess.GetSpaceshipsById(id);

            if (spaceships == null)
            {
                return NotFound();
            }

            return spaceships;
        }

        // PUT: api/Spaceships/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutSpaceships(int id, Spaceships spaceships)
        {
            if (id != spaceships.Id)
            {
                return BadRequest();
            }

            //_context.Entry(spaceships).State = EntityState.Modified;
            _spaceshipsDataAccess.ApiStateModified(spaceships);

            try
            {
                //await _context.SaveChangesAsync();
                await _spaceshipsDataAccess.ApiSaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpaceshipsExists(id))
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

        // POST: api/Spaceships
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Spaceships>> PostSpaceships(Spaceships spaceships)
        {
            //_context.Spaceships.Add(spaceships);
            //await _context.SaveChangesAsync();
            bool canAddSpaceship = await _spaceshipsDataAccess.CanAddSpaceshipToDock(spaceships.DockId);
            if (canAddSpaceship)
            {

                await _spaceshipsDataAccess.CreateSpaceship(spaceships);
                await _docksDataAccess.IncreaseDockCapacity(spaceships.DockId);
            }
            else
            {
                return BadRequest();
            }
            await _spaceshipsDataAccess.CreateSpaceship(spaceships);

            return CreatedAtAction("GetSpaceships", new { id = spaceships.Id }, spaceships);
        }

        // DELETE: api/Spaceships/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Spaceships>> DeleteSpaceships(int id)
        {
            //var spaceships = await _context.Spaceships.FindAsync(id);
            var spaceships = await _spaceshipsDataAccess.GetSpaceshipsById(id);
            int dockId = spaceships.DockId;
            if (spaceships == null)
            {
                return NotFound();
            }

            //_context.Spaceships.Remove(spaceships);
            //await _context.SaveChangesAsync();
            await _docksDataAccess.DecreaseDockCapacity(dockId);
            await _spaceshipsDataAccess.DeleteSpaceships(id);

            return spaceships;
        }

        private bool SpaceshipsExists(int id)
        {
            //return _context.Spaceships.Any(e => e.Id == id);
            return _spaceshipsDataAccess.CheckSpaceshipsExists(id);
        }
    }
}
