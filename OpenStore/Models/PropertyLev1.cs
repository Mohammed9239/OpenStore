using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class PropertyLev1
    {
        public PropertyLev1()
        {
            PropertyLev2s = new HashSet<PropertyLev2>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }


        //FK
        public int CategoryLev2ID { get; set; }
        public virtual CategoryLev2 CategoryLev2 { get; set; }

        //ICollection
        public virtual ICollection<PropertyLev2> PropertyLev2s { get; set; }
    }
}
