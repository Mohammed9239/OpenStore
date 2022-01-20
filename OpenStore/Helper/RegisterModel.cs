using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Helper
{
    public class RegisterModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "{0} يجب ان تكون اكثر من {2} رموز.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Required]
        public int ProvinceID { get; set; }

        [Required]
        public string Streat { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
