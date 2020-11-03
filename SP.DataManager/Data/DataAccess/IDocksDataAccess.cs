using SP.DataManager.Models;
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
        Task IncreaseDockCapacity(int? dockId);
        Task DecreaseDockCapacity(int? dockId);
        void ApiStateModified(Docks docks);
        Task ApiSaveChanges();
        bool ValidateDockEdit(Docks docks);
    }
}