using System;
using System.Collections.Generic;

namespace SP.DataManager.Models
{
    public partial class Spaceships
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public int CrewSize { get; set; }
        public int DockId { get; set; }

        public virtual Docks Dock { get; set; }
    }
}
