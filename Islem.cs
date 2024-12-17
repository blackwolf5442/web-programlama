namespace proje.Models
{
    public class Islem
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public double Sure { get; set; } // Dakika cinsinden
        public decimal Ucret { get; set; }
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
    }
}
