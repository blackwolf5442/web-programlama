using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proje.Models
{
    public class Randevu
    {
        [Key]
        public int Id { get; set; }  // Birincil anahtar

        [Required(ErrorMessage = "Çalışan seçimi zorunludur.")]
        public int CalisanId { get; set; }  // Yabancı Anahtar - Çalışan

        [ForeignKey("CalisanId")]
        public Calisan Calisan { get; set; }  // Navigation Property

        [Required(ErrorMessage = "İşlem seçimi zorunludur.")]
        public int IslemId { get; set; }  // Yabancı Anahtar - İşlem

        [ForeignKey("IslemId")]
        public Islem Islem { get; set; }  // Navigation Property

        [Required(ErrorMessage = "Randevu tarihi ve saati zorunludur.")]
        public DateTime TarihSaat { get; set; }  // Randevu Tarihi ve Saati

        public bool Onaylandi { get; set; } = false;  // Onay durumu

        [Required]
        [StringLength(50)]
        public string AdminOnayli { get; set; } = "Beklemede"; // Yönetici onay durumu

        [Required(ErrorMessage = "Müşteri adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Müşteri adı en fazla 50 karakter olabilir.")]
        public string MusteriAd { get; set; }  // Müşteri Adı

        [Required(ErrorMessage = "Müşteri soyadı zorunludur.")]
        [StringLength(50, ErrorMessage = "Müşteri soyadı en fazla 50 karakter olabilir.")]
        public string MusteriSoyad { get; set; }  // Müşteri Soyadı

        [Required(ErrorMessage = "Müşteri email adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        [StringLength(100, ErrorMessage = "Müşteri email adresi en fazla 100 karakter olabilir.")]
        public string MusteriEmail { get; set; }  // Müşteri Email Adresi
    }
}



