using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace proje.Models
{
    public class KuaforDbContext:DbContext
    {
        public KuaforDbContext(DbContextOptions<KuaforDbContext> options) : base(options) { }
        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Islem> Islemler { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Randevu> Randevular { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Calisan)
                .WithMany()
                .HasForeignKey(r => r.CalisanId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete devre dışı

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Islem)
                .WithMany()
                .HasForeignKey(r => r.IslemId)
                .OnDelete(DeleteBehavior.Restrict);  // Cascade delete devre dışı
           
        }
        

    }
}
