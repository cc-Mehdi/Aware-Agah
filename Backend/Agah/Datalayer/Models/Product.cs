using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Datalayer.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MinLength(3, ErrorMessage = "مقدار {0} باید حداقل 3 کاراکتر باشد")]
        [MaxLength(300, ErrorMessage = "مقدار {0} باید حداکثر 300 کاراکتر باشد")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "تاریخ حذف")]
        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }
    }
}
