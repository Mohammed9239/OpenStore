using OpenStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.VM
{
    public class HomeVm
    {
        public List<Product> Products { get; set; }
        public List<Product> ProductsDiscount { get; set; }
        public List<CategoryLev1> CategoryLev1s { get; set; }
        public List<ADS> ADs { get; set; }

    }
}
