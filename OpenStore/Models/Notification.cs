using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public enum Type
    {
        [Display(Name = "رسالة عامة")]
        اشعار,

        [Display(Name = "استفسار")]
        استفسار,
    }

    public class Notification
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public string Message { get; set; }

        public Type Type { get; set; }

        public DateTime Date { get; set; }

        public bool State { get; set; }

        //FK
        public string ReceiverID { get; set; }
        public virtual User Receiver { get; set; }
    }
}
