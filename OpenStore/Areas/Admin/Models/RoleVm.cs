using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Areas.Admin.Models
{
    public class RoleVm
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }
}
