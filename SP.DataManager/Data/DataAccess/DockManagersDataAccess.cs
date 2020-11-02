using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SP.DataManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace SP.DataManager.Data.DataAccess
{

    public class DockManagersDataAccess : IDockManagersDataAccess
    {
        private readonly SPDataContext _context;
        public DockManagersDataAccess(SPDataContext context)
        {
            _context = context;
        }

        public async Task<List<DockManagers>> GetDockManagers()
        {
            return await _context.DockManagers.ToListAsync();
        }

        public async Task<DockManagers> GetDataManagersById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var dockManagers = await _context.DockManagers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dockManagers == null)
            {
                return null;
            }

            return dockManagers;

        }

        public async Task CreateDockManager(DockManagers dockManagers)
        {
            _context.Add(dockManagers);
            await _context.SaveChangesAsync();
        }

        public async Task EditDockManagers(DockManagers dockManagers)
        {
            _context.Update(dockManagers);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDockManager(int id)
        {
            var dockManagers = await _context.DockManagers.FindAsync(id);
            _context.DockManagers.Remove(dockManagers);
            await _context.SaveChangesAsync();
        }

        public bool CheckDockManagersExists(int id)
        {
            return _context.DockManagers.Any(e => e.Id == id);
        }

        public dynamic ReturnDockManagers()
        {
            return _context.DockManagers;
        }

        public void ApiStateModified(DockManagers dockManagers)
        {
            _context.Entry(dockManagers).State = EntityState.Modified;
        }

        public async Task ApiSaveChanges()
        {
            await _context.SaveChangesAsync();
        }

    }
}
