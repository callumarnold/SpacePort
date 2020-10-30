using SP.DataManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SP.DataManager.Data.DataAccess
{
    public interface IDockManagersDataAccess
    {
        Task<DockManagers> GetDataManagersById(int? id);
        Task<List<DockManagers>> GetDockManagers();
        Task CreateDockManager(DockManagers dockManagers);
        Task EditDockManagers(DockManagers dockManagers);
        Task DeleteDockManager(int id);
        bool CheckDockManagersExists(int id);
        dynamic ReturnDockManagers();
    }
}