﻿using Microsoft.EntityFrameworkCore;
using SP.DataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP.DataManager.Data.DataAccess
{
    public class DocksDataAccess : IDocksDataAccess
    {
        private readonly SPDataContext _context;

        public DocksDataAccess(SPDataContext context)
        {
            _context = context;
        }

        public async Task<List<Docks>> GetDocks()
        {
            var dataContext = _context.Docks.Include(d => d.Manager);
            return await dataContext.ToListAsync();
        }

        public async Task<Docks> GetDockById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var docks = await _context.Docks
                .Include(d => d.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docks == null)
            {
                return null;
            }
            return docks;
        }

        public async Task CreateDock(Docks docks)
        {
            _context.Add(docks);
            await _context.SaveChangesAsync();
        }

        public async Task EditDocks(Docks docks)
        {
            _context.Update(docks);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDock(int id)
        {
            var docks = await _context.Docks.FindAsync(id);
            _context.Docks.Remove(docks);
            await _context.SaveChangesAsync();
        }

        public dynamic ReturnDocks()
        {
            return _context.Docks;
        }

        public bool CheckDockExists(int id)
        {
            return _context.Docks.Any(e => e.Id == id);
        }

        public async Task IncreaseDockCapacity(int? dockId)
        {
            var docks = await GetDockById(dockId);
            docks.CurrentCapacity++;
            await EditDocks(docks);
        }

        public async Task DecreaseDockCapacity(int? dockId)
        {
            var docks = await GetDockById(dockId);
            docks.CurrentCapacity--;
            await EditDocks(docks);
        }

        public void ApiStateModified(Docks docks)
        {
            _context.Entry(docks).State = EntityState.Modified;
        }

        public async Task ApiSaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public bool ValidateDockEdit(Docks docks)
        {
            if(docks.MaxCapacity > docks.CurrentCapacity)
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
