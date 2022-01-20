using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenStore.Models
{
    public enum Page
    {
        [Display(Name = "الرئيسية")]
        الرئيسية,

        [Display(Name = "الاصناف")]
        الاصناف,

        [Display(Name = "التسوق")]
        التسوق,

        [Display(Name = "السلة")]
        السلة,

        [Display(Name = "الاشعارات")]
        الاشعارات,

        [Display(Name = "تواصل")]
        تواصل,

        [Display(Name = " تفاصيل الاشعارات")]
        تفاصيل_الاشعارات,

        [Display(Name = " اكمال الطلب")]
        اكمال_الطلب,

    }
    public enum Section
    {
        [Display(Name = "الاول")]
        الاول,

        [Display(Name = "الثاني")]
        الثاني,
    }
    public class ADS
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Photo { get; set; }

        public Page Page { get; set; }
        public Section Section { get; set; }
    }
}
