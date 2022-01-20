using OpenStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Areas.Admin.Models
{
    public class EditUser
    {
        public string UserId { get; set; }

        public string UserName { get; set; }
        public List<RoleVm> Roles { get; set; }
        public string? RoleName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? CommercialNumber { get; set; }
        public List<Province> Province { get; set; }
        public int ProvinceID { get; set; }
        public List<City> Cities { get; set; }
        public int? CityID { get; set; }
        public string? Address { get; set; }
        public string? PhotoUrl { get; set; }




    }
}
