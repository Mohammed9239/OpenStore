using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Areas.Admin.Models
{
    public class ProductVm
    {
        public int ID { get; set; }
        public string StoreName { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Qunatity { get; set; }
        public string CategoryLev1 { get; set; }
        public string CategoryLev2 { get; set; }
        public string CategoryLev3 { get; set; }
        public int? Discount { get; set; }
        public DateTime Date { get; set; }
        public int? Special { get; set; }
        public List<string> PropertyLev1 { get; set; }
        public List<string> PropertyLev2 { get; set; }
        public List<string> Imgs { get; set; }


    }
}
