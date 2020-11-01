using Microsoft.EntityFrameworkCore;
using SP.DataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP.DataManager.Data.DataAccess
{
    public class SpaceshipsDataAccess : ISpaceshipsDataAccess
    {
        private readonly SPDataContext _context;

        public SpaceshipsDataAccess(SPDataContext context)
        {
            _context = context;
        }

        public async Task<List<Spaceships>> GetSpaceships()
        {
            return await _context.Spaceships
                .Include(s => s.Dock)
                .ToListAsync();
        }

        public async Task<Spaceships> GetSpaceshipsById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var spaceships = await _context.Spaceships
                .Include(s => s.Dock)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spaceships == null)
            {
                return null;
            }

            return spaceships;

        }

        public async Task CreateSpaceship(Spaceships spaceships)
        {
            _context.Add(spaceships);
            await _context.SaveChangesAsync();
        }

        public async Task EditSpaceships(Spaceships spaceships)
        {
            _context.Update(spaceships);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSpaceships(int id)
        {
            var spaceships = await _context.Spaceships.FindAsync(id);
            _context.Spaceships.Remove(spaceships);
            await _context.SaveChangesAsync();

        }

        public bool CheckSpaceshipsExists(int id)
        {
            return _context.Spaceships.Any(e => e.Id == id);
        }

        public dynamic ReturnSpaceships()
        {
            return _context.Spaceships;
        }

        public async Task<bool> CanAddSpaceshipToDock(int id)
        {
            var docks = await _context.Docks
                .Include(d => d.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docks.CurrentCapacity < docks.MaxCapacity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
