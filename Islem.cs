
using System.ComponentModel.DataAnnotations;

namespace proje.Models
{
    public class Islem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İşlem adı zorunludur.")]
        [StringLength(100, ErrorMessage = "İşlem adı en fazla 100 karakter olabilir.")]
        public string Ad { get; set; }

        [Range(1, 480, ErrorMessage = "İşlem süresi 1 ile 480 dakika arasında olmalıdır.")]
        public double Sure { get; set; } // Dakika cinsinden

        [Range(0.01, 10000, ErrorMessage = "Ücret 0.01 ile 10000 arasında olmalıdır.")]
        public decimal Ucret { get; set; }

        [Required(ErrorMessage = "Salon ID zorunludur.")]
        public int SalonId { get; set; }

        public Salon Salon { get; set; }
    }
}
