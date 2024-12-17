namespace proje.Models
{
    public class Calisan
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public List<string> Uzmanliklar { get; set; } = new();
        public string UygunlukSaatleri { get; set; } // Örneğin: 09:00-12:00, 14:00-18:00
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
    }
}
