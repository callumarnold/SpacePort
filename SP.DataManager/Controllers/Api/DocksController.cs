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

namespace SP.DataManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocksController : ControllerBase
    {
        private readonly SPDataContext _context;

        public DocksController(SPDataContext context)
        {
            _context = context;
        }

        // GET: api/Docks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Docks>>> GetDocks()
        {
            return await _context.Docks.ToListAsync();
        }

        // GET: api/Docks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Docks>> GetDocks(int id)
        {
            var docks = await _context.Docks.FindAsync(id);

            if (docks == null)
            {
                return NotFound();
            }

            return docks;
        }

        // PUT: api/Docks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutDocks(int id, Docks docks)
        {
            if (id != docks.Id)
            {
                return BadRequest();
            }

            _context.Entry(docks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocksExists(id))
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

        // POST: api/Docks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Docks>> PostDocks(Docks docks)
        {
            _context.Docks.Add(docks);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocks", new { id = docks.Id }, docks);
        }

        // DELETE: api/Docks/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Docks>> DeleteDocks(int id)
        {
            var docks = await _context.Docks.FindAsync(id);
            if (docks == null)
            {
                return NotFound();
            }

            _context.Docks.Remove(docks);
            await _context.SaveChangesAsync();

            return docks;
        }

        private bool DocksExists(int id)
        {
            return _context.Docks.Any(e => e.Id == id);
        }
    }
}
