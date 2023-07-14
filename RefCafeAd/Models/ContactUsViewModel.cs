using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RefCafeAd.Models
{
    public class ContactUsViewModel
    {
        [Display(Name = "Ad/Soyad")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string Name { get; set; }

        [Display(Name = "E-Posta")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz!")]
        public string Email { get; set; }

        [Display(Name = "İletişim Numarası")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Mesaj")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string Message { get; set; }
    }
}
