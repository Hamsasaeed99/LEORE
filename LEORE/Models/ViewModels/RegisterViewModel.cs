using System.ComponentModel.DataAnnotations;

namespace LEORE.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "أدخل بريد إلكتروني صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 50 حرف")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمتا المرور غير متطابقتين")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [StringLength(50, ErrorMessage = "الاسم الأول لا يمكن أن يزيد عن 50 حرف")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        [StringLength(50, ErrorMessage = "اسم العائلة لا يمكن أن يزيد عن 50 حرف")]
        [Display(Name = "اسم العائلة")]
        public string LastName { get; set; }

        [StringLength(50, ErrorMessage = "رقم الهاتف لا يمكن أن يزيد عن 50 رقم")]
        [Phone(ErrorMessage = "أدخل رقم هاتف صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string Phone { get; set; }

        [StringLength(50, ErrorMessage = "العنوان لا يمكن أن يزيد عن 50 حرف")]
        [Display(Name = "العنوان")]
        public string Address { get; set; }
    }
}