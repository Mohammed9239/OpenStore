using OpenStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Areas.Admin.Models
{
    public class GetProVm
    {
        public int ID { get; set; }
        public string Namep { get; set; }
        public List<pro2> PropertyLev2 { get; set; }
    }
}
