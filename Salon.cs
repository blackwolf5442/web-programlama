namespace proje.Models
{
    public class Salon
    {
       
            public int Id { get; set; }
            public string Ad { get; set; }
            public string CalismaSaatleri { get; set; } // Örneğin: 09:00-18:00
            public List<Islem> Islemler { get; set; } = new();
            public List<Calisan> Calisanlar { get; set; } = new();
            
            //public ICollection<Calisan> Calisanlar { get; set; }


    }
}
