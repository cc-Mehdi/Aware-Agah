using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datalayer.Models
{
    public class Reserve
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public int User_Id { get; set; }
        public User User { get; set; }


        [ForeignKey("Product")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public int Product_Id { get; set; }
        public Product Product { get; set; }


        [ForeignKey("Alarm")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public int Alarm_Id { get; set; }
        public Alarm Alarm { get; set; }

        [Display(Name = "حداقل قیمت")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [Range(0, 9999999999, ErrorMessage = "مقدار {0} باید بین 0 تا 999,999,999 کاراکتر باشد")]
        [DataType(DataType.Currency)]
        [DefaultValue(0)]
        public decimal MinPrice { get; set; }

        [Display(Name = "حداکثر قیمت")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [Range(0, 9999999999, ErrorMessage = "مقدار {0} باید بین 0 تا 999,999,999 کاراکتر باشد")]
        [DataType(DataType.Currency)]
        [DefaultValue(0)]
        public decimal MaxPrice { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}
