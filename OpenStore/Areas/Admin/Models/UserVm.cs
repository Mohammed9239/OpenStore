using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Areas.Admin.Models
{
    public class UserVm
    {
        public string ID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string? FullName { get; set; }

        [Display(Name = "نوع الحساب")]
        public string? RoleID { get; set; }

        public IEnumerable<string>? roles { get; set; }

        [Display(Name = "رقم السجل التجاري")]
        public string? CommercialNumber { get; set; }

        public string? Address { get; set; }
        public DateTime? Date { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public int ProvinceID { get; set; }

        public string PhotoUrl { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "{0} يجب ان تكون اكثر من {2} رموز.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }


    }
}
