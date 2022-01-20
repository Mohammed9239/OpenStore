using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public enum State
    {

        [Display(Name ="في السلة")] incart = 1,
        [Display(Name ="جاري الطلب")] Demand = 2,
        [Display(Name ="جاري التوصيل")] Connettivity = 3,
        [Display(Name ="تم التوصيل")] Delivered = 4,
        [Display(Name ="لم يتم التوصيل")] NotDelivered = 5
    }
    public class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int ID { get; set; }
        public double Cost { get; set; }
        public State State { get; set; }
        public DateTime Date { get; set; }
        public string BankImg { get; set; }
        public int BankNum { get; set; }
        public string Street { get; set; }

        //FK
        public string UserID { get; set; }
        public virtual User User { get; set; }

        //FK
        public int ProvinceID { get; set; }
        public virtual Province Province { get; set; }

        //ICollection
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

    }
}
