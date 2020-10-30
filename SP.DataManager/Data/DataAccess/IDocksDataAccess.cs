﻿using SP.DataManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SP.DataManager.Data.DataAccess
{
    public interface IDocksDataAccess
    {
        Task CreateDock(Docks docks);
        Task DeleteDock(int id);
        Task EditDocks(Docks docks);
        Task<Docks> GetDockById(int? id);
        Task<List<Docks>> GetDocks();
        dynamic ReturnDocks();
        bool CheckDockExists(int id);
    }
}