using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public class Product
    {
        public Product()
        {
            Imgs = new HashSet<Img>();
            Evaluations = new HashSet<Evaluation>();
            Comments = new HashSet<Comment>();
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Qunatity { get; set; }
        public int? Special { get; set; }
        public int? Discount { get; set; }

        public DateTime Date { get; set; }

        //FK
        public int GetCategoryLev2ID { get; set; }
        public virtual CategoryLev2 GetCategoryLev2 { get; set; }

        //FK
        public int? GetCategoryLev3ID { get; set; }
        public virtual CategoryLev3 GetCategoryLev3 { get; set; }

        //FK < For Store >
        [ForeignKey("User")]
        public string StoreID { get; set; }
        public virtual User Store { get; set; }

        //ICollection
        public virtual ICollection<Img> Imgs { get; set; }

        //ICollection
        public virtual ICollection<Evaluation> Evaluations { get; set; }

        //ICollection
        public virtual ICollection<Comment> Comments { get; set; }

        //ICollection
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

        //ICollection
        public virtual ICollection<ProdectProperty> ProdectProperties { get; set; }


    }
}
