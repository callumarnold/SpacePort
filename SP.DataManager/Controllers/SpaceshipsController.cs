using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SP.DataManager.Data;
using SP.DataManager.Models;

namespace SP.DataManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpaceshipsController : ControllerBase
    {
        private readonly SPDataContext _context;

        public SpaceshipsController(SPDataContext context)
        {
            _context = context;
        }

        // GET: api/Spaceships
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Spaceships>>> GetSpaceships()
        {
            return await _context.Spaceships.ToListAsync();
        }

        // GET: api/Spaceships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Spaceships>> GetSpaceships(int id)
        {
            var spaceships = await _context.Spaceships.FindAsync(id);

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
        public async Task<IActionResult> PutSpaceships(int id, Spaceships spaceships)
        {
            if (id != spaceships.Id)
            {
                return BadRequest();
            }

            _context.Entry(spaceships).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
        public async Task<ActionResult<Spaceships>> PostSpaceships(Spaceships spaceships)
        {
            _context.Spaceships.Add(spaceships);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpaceships", new { id = spaceships.Id }, spaceships);
        }

        // DELETE: api/Spaceships/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Spaceships>> DeleteSpaceships(int id)
        {
            var spaceships = await _context.Spaceships.FindAsync(id);
            if (spaceships == null)
            {
                return NotFound();
            }

            _context.Spaceships.Remove(spaceships);
            await _context.SaveChangesAsync();

            return spaceships;
        }

        private bool SpaceshipsExists(int id)
        {
            return _context.Spaceships.Any(e => e.Id == id);
        }
    }
}
