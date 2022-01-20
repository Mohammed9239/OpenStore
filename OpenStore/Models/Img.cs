using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class Img
    {
        public int ID { get; set; }
        public string ImgUrl { get; set; }

        //FK
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}
