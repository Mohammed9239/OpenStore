using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class Evaluation
    {
        public int ID { get; set; }
        public int Value { get; set; }

        //FK
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        //FK
        public string UserID { get; set; }
        public virtual User User { get; set; }
    }
}
