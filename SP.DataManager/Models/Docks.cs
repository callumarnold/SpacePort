using System;
using System.Collections.Generic;

namespace SP.DataManager.Models
{
    public partial class Docks
    {
        public Docks()
        {
            Spaceships = new HashSet<Spaceships>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ManagerId { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentCapacity { get; set; }

        public virtual DockManagers Manager { get; set; }
        public virtual ICollection<Spaceships> Spaceships { get; set; }
    }
}
