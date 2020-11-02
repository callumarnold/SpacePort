using SP.DataManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SP.DataManager.Data.DataAccess
{
    public interface ISpaceshipsDataAccess
    {
        bool CheckSpaceshipsExists(int id);
        Task CreateSpaceship(Spaceships spaceships);
        Task DeleteSpaceships(int id);
        Task EditSpaceships(Spaceships spaceships);
        Task<List<Spaceships>> GetSpaceships();
        Task<Spaceships> GetSpaceshipsById(int? id);
        dynamic ReturnSpaceships();
        Task<bool> CanAddSpaceshipToDock(int id);
        void ApiStateModified(Spaceships spaceships);
        Task ApiSaveChanges();
    }
}