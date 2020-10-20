using System;
using System.Collections.Generic;

namespace SP.DataManager.Models
{
    public partial class DockManagers
    {
        public DockManagers()
        {
            Docks = new HashSet<Docks>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Docks> Docks { get; set; }
    }
}
