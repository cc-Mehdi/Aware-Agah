using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class ProductLog
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public Product Product { get; set; }

        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [Range(0, 9999999999, ErrorMessage = "مقدار {0} باید بین 0 تا 999,999,999 کاراکتر باشد")]
        [DataType(DataType.Currency)]
        [DefaultValue(0)]
        public decimal Price { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}
