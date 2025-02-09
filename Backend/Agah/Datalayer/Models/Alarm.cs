using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Datalayer.Models
{
    public class Alarm
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام لاتین")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MinLength(3, ErrorMessage = "مقدار {0} باید حداقل 3 کاراکتر باشد")]
        [MaxLength(300, ErrorMessage = "مقدار {0} باید حداکثر 300 کاراکتر باشد")]
        [DataType(DataType.Text)]
        public string EnglishName { get; set; }

        [Display(Name = "نام فارسی")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MinLength(3, ErrorMessage = "مقدار {0} باید حداقل 3 کاراکتر باشد")]
        [MaxLength(300, ErrorMessage = "مقدار {0} باید حداکثر 300 کاراکتر باشد")]
        [DataType(DataType.Text)]
        public string PersianName { get; set; }

        [Display(Name = "قیمت اعلان")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [Range(0, 9999999, ErrorMessage = "مقدار {0} باید بین 0 تا 9,999,999 کاراکتر باشد")]
        [DataType(DataType.Currency)]
        [DefaultValue(0)]
        public decimal AlarmPrice { get; set; }

        [Display(Name = "شرح مختصر")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "مقدار {0} باید حداکثر 500 کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string? ShortDescription { get; set; }

        [Display(Name = "وضعیت")]
        [DefaultValue(0)]
        public bool IsActive { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "تاریخ حذف")]
        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }
    }
}
