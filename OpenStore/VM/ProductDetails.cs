using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.VM
{
    public class ProductDetails
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public List<string> Imgs { get; set; }
        public int Evaluations { get; set; }
        public int EvaluationCount { get; set; }
        public double Price { get; set; }
        public int? Discount { get; set; }
        public int? Qunatity { get; set; }
        public string Description { get; set; }
        public List<string> Property { get; set; }
        public List<string> PropertyValue { get; set; }


    }
}
