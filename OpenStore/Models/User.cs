using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class User:IdentityUser
    {
        public User()
        {
            Evaluations = new HashSet<Evaluation>();
            Comments = new HashSet<Comment>();
            Phones = new HashSet<Phone>();
            Notifications = new HashSet<Notification>();
            Orders = new HashSet<Order>();
            Products = new HashSet<Product>();
        }

        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public DateTime Date { get; set; }
        public string? PhotoUrl { get; set; }

        //for store
        public string? CommercialNumber { get; set; }

        //FK
        public int ProvinceID { get; set; }
        public virtual Province Province { get; set; }

        //ICollection
        public virtual ICollection<Evaluation> Evaluations { get; set; }

        //ICollection
        public virtual ICollection<Comment> Comments { get; set; }

        //ICollection
        public virtual ICollection<Phone> Phones { get; set; }

        //ICollection
        public virtual ICollection<Notification> Notifications { get; set; }

        //ICollection
        public virtual ICollection<Order> Orders { get; set; }

        //ICollection < For Store >
        public virtual ICollection<Product> Products { get; set; }
    }
}
