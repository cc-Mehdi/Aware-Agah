using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Datalayer.Models
{
    public class User
    {
        public User()
        {
            this.CreatedAt = DateTime.Now;    
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "نام کامل")]
        [MaxLength(300, ErrorMessage = "مقدار {0} باید حداکثر 300 کاراکتر باشد")]
        [DataType(DataType.Text)]
        public string? Fullname { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MinLength(5, ErrorMessage = "مقدار {0} باید حداقل 5 کاراکتر باشد")]
        [MaxLength(300, ErrorMessage = "مقدار {0} باید حداکثر 300 کاراکتر باشد")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DefaultValue(false)]
        public bool IsEmailVerified { get; set; }
        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationTokenExpiry { get; set; }

        [Display(Name = "تلفن")]
        [MinLength(11, ErrorMessage = "مقدار {0} باید 11 کاراکتر باشد")]
        [MaxLength(11, ErrorMessage = "مقدار {0} باید 11 کاراکتر باشد")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [DefaultValue(false)]
        public bool IsPhoneVerivied { get; set; }
        public string? PhoneVerificationToken { get; set; }
        public DateTime? PhoneVerificationTokenExpiry { get; set; }


        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MinLength(6, ErrorMessage = "مقدار {0} باید 6 کاراکتر باشد")]
        [MaxLength(300, ErrorMessage = "مقدار {0} باید 300 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string HashedPassword { get; set; }

        [Display(Name = "سمت")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} باید 300 کاراکتر باشد")]
        [DataType(DataType.Text)]
        public string Role { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }


        [Display(Name = "تاریخ حذف")]
        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }
    }
}
