using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class CategoryLev3
    {
        public CategoryLev3()
        {
            Products = new HashSet<Product>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public int Index { get; set; }


        //FK
        public int CategoryLev2ID { get; set; }
        public virtual CategoryLev2 CategoryLev2 { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
