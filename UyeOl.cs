using System.ComponentModel.DataAnnotations;

namespace proje.Models
{
    public class UyeOl
    {
            [Key]
            public int Id { get; set; }
            [Required(ErrorMessage = "Kullanıcı adı alanı zorunludur.")]
            public string KullaniciAdi { get; set; }

            [Required(ErrorMessage = "E-posta adresi alanı zorunludur.")]
            [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Şifre alanı zorunludur.")]
            [StringLength(20, MinimumLength = 8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
            public string Sifre { get; set; }

            [Required(ErrorMessage = "Ad soyad alanı zorunludur.")]
            public string AdSoyad { get; set; }

            [Required(ErrorMessage = "Doğum tarihi alanı zorunludur.")]
            [DataType(DataType.Date)]
            public DateTime DogumTarihi { get; set; }

            [Required(ErrorMessage = "Cinsiyet alanı zorunludur.")]
            public string Cinsiyet { get; set; }

            [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
            public string Telefon { get; set; }
        
    }
}

