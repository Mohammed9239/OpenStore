using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class CategoryLev1
    {
        public CategoryLev1()
        {
            CategoryLev2s = new HashSet<CategoryLev2>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public int Index { get; set; }

        //ICollection
        public virtual ICollection<CategoryLev2> CategoryLev2s { get; set; }
    }
}
