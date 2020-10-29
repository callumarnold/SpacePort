using SP.DataManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SP.DataManager.Data.DataAccess
{
    public interface IDockManagersDataAccess
    {
        Task<List<DockManagers>> GetDockManagers();
    }
}