using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class OrderProduct
    {
        public int ID { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        //FK
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        //FK
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}
