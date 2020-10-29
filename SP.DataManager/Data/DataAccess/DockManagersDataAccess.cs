﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SP.DataManager.Models;

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


    }
}