using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Datalayer.Models
{
    public class Product
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

        [Display(Name = "آیکون")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} باید حداکثر 300 کاراکتر باشد")]
        [DataType(DataType.Text)]
        [DefaultValue("fa-solid fa-medal")]
        public string IconName { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "تاریخ حذف")]
        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }
    }
}
