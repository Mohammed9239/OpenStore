using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class ProdectProperty
    {
        public int ID { get; set; }

        //FK
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        //FK
        public int PropertyLev2ID { get; set; }
        public virtual PropertyLev2 PropertyLev2 { get; set; }
    }
}
