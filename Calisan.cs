
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace proje.Models
{
    public class Calisan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ad 100 karakterden uzun olamaz.")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Uzmanlıklar alanı boş bırakılamaz.")]
        public List<string> Uzmanliklar { get; set; } = new();

        [Required(ErrorMessage = "Uygunluk saatleri belirtilmelidir.")]
        [RegularExpression(@"^\d{2}:\d{2}-\d{2}:\d{2}(, \d{2}:\d{2}-\d{2}:\d{2})*$", ErrorMessage = "Uygunluk saatleri formatı doğru değil. Örneğin: 09:00-12:00, 14:00-18:00")]
        public string UygunlukSaatleri { get; set; } // Örneğin: 09:00-12:00, 14:00-18:00

        [Required(ErrorMessage = "Salon bilgisi zorunludur.")]
        public int SalonId { get; set; }

        public Salon Salon { get; set; }

        public ICollection<Randevu> Randevular { get; set; } // Navigation Property (Çalışanın randevuları)
    }
}

