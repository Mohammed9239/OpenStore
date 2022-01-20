using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class Province
    {
        public Province()
        {
            Users = new HashSet<User>();
            Orders = new HashSet<Order>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        //FK
        public int CityID { get; set; }
        public virtual City City { get; set; }

        //ICollection
        public virtual ICollection<User> Users { get; set; }

        //ICollection
        public virtual ICollection<Order> Orders { get; set; }
    }
}
