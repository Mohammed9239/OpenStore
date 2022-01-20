using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class Phone
    {
        public int ID { get; set; }
        public string Number { get; set; }

        //FK
        public string UserID { get; set; }
        public virtual User User { get; set; }
    }
}
