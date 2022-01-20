using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class City
    {
        public City()
        {
            Provinces = new HashSet<Province>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        //ICollection
        public virtual ICollection<Province> Provinces { get; set; }
    }
}
