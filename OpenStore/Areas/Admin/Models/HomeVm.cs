using OpenStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Areas.Admin.Models
{
    public class HomeVm
    {
        public int ID { get; set; }
        public int Sales { get; set; }
        public int UnCompletedOrder { get; set; }
        public int CompletedOrder { get; set; }
        public int Inquire { get; set; }
        public int Saler { get; set; }
        public int Customer { get; set; }

        public List<OrderVm> Orders { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<UserVm> Users { get; set; }
    }
}
