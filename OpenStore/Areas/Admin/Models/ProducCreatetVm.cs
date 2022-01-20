using OpenStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Areas.Admin.Models
{
    public class ProducCreatetVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Qunatity { get; set; }
        public int GetCategoryLev3ID { get; set; }
        public int GetCategoryLev2ID { get; set; }
        public int GetCategoryLev1ID { get; set; }
        public int? Discount { get; set; }
        //public List<string> PropertyLev1 { get; set; }
        public List<int> PropertyLev2ID { get; set; }
    }
}
