using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class PropertyLev2
    {
        public PropertyLev2()
        {
            ProdectProperties = new HashSet<ProdectProperty>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string ImgUrl { get; set; }


        //FK
        public int PropertyLev1ID { get; set; }
        public virtual PropertyLev1 PropertyLev1 { get; set; }

        //ICollection
        public virtual ICollection<ProdectProperty> ProdectProperties { get; set; }

    }
}
