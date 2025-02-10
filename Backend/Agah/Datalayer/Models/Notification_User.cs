using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models
{
    public class Notification_User
    {
        public Notification_User()
        {
            this.CreatedAt = DateTime.Now;    
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Display(Name = "عنوان پیام")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} باید حداکثر 300 کاراکتر باشد")]
        [DataType(DataType.Text)]
        public string MessageSubject { get; set; }

        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "مقدار {0} باید حداکثر 500 کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string MessageContent { get; set; }

        [Display(Name = "خوانده شده")]
        [DefaultValue(0)]
        public bool IsRead { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "تاریخ حذف")]
        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }
    }
}
