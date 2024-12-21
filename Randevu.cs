using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proje.Models
{
    public class Randevu
    {
        [Key]
        public int Id { get; set; }  // Birincil anahtar

        [Required]
        public int CalisanId { get; set; }  // Yabancı Anahtar - Çalışan

        [ForeignKey("CalisanId")]
        public Calisan Calisan { get; set; }  // Navigation Property

        [Required]
        public int IslemId { get; set; }  // Yabancı Anahtar - İşlem

        [ForeignKey("IslemId")]
        public Islem Islem { get; set; }  // Navigation Property

        [Required]
        public DateTime TarihSaat { get; set; }  // Randevu Tarihi ve Saati

        public bool Onaylandi { get; set; } = false;  // Onay durumu
    }

}

