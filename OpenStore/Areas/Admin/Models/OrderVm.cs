using OpenStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Areas.Admin.Models
{
    public class OrderVm
    {
        public int ID { get; set; }
        public string SallerName { get; set; }
        public double Cost { get; set; }
        public State State { get; set; }
        public DateTime Date { get; set; }
        public int BankNum { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Street { get; set; }
        public string BankImg { get; set; }
        public List<Phone> Phone { get; set; }
        public int OrderProductsID { get; set; }
        public IEnumerable<OrderProduct> OrderProducts { get; set; }

        public int ProvinceID { get; set; }
        public int CityID { get; set; }
        public List<City> Cities { get; set; }


    }
}
