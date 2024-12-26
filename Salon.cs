
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace proje.Models
{
    public class Salon
    {
        [Key]
        public int Id { get; set; } // Birincil anahtar

        [Required(ErrorMessage = "Salon adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Salon adı en fazla 100 karakter olabilir.")]
        public string Ad { get; set; } // Salon adı

        [Required(ErrorMessage = "Çalışma saatleri zorunludur.")]
        [RegularExpression("^\\d{2}:\\d{2}-\\d{2}:\\d{2}$", ErrorMessage = "Çalışma saatleri formatı geçerli değil. Örnek: 09:00-18:00")]
        public string CalismaSaatleri { get; set; } // Örneğin: 09:00-18:00

        [NotMapped] // Bu alan modelde kullanılmıyor; silinebilir ya da NotMapped bırakılabilir
        public int SalonId { get; set; }

        public List<Islem> Islemler { get; set; } = new(); // İlişkili işlemler

        public List<Calisan> Calisanlar { get; set; } = new(); // İlişkili çalışanlar
    }
}
