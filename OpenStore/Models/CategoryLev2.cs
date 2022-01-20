using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class CategoryLev2
    {
        public CategoryLev2()
        {
            Products = new HashSet<Product>();
            PropertyLev1s = new HashSet<PropertyLev1>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public int Index { get; set; }



        //FK
        public int CategoryLev1ID { get; set; }
        public virtual CategoryLev1 CategoryLev1 { get; set; }

        //ICollection
        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<CategoryLev3> CategoryLev3s { get; set; }
        public virtual ICollection<PropertyLev1> PropertyLev1s { get; set; }


    }
}
