using Microsoft.AspNetCore.Mvc;
using proje.Models;
using System.Linq;

namespace proje.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RandevuApiController : ControllerBase
    {
        private readonly KuaforDbContext _context;

        public RandevuApiController(KuaforDbContext context)
        {
            _context = context;
        }

        // 1. Tüm Randevuları Getir (LINQ Kullanımıyla)
        [HttpGet]
        public IActionResult GetAllRandevular()
        {
            var randevular = _context.Randevular
                .Select(r => new
                {
                    r.Id,
                    r.MusteriAd,
                    r.MusteriSoyad,
                    r.MusteriEmail,
                    Calisan = r.Calisan != null ? r.Calisan.Ad : "Belirtilmedi",
                    Islem = r.Islem != null ? r.Islem.Ad : "Belirtilmedi",
                    r.TarihSaat,
                    r.Onaylandi
                })
                .ToList();

            return Ok(randevular);
        }

        // 2. Belirli Bir Çalışanın Randevularını Getir
        [HttpGet("Calisan/{calisanId}")]
        public IActionResult GetRandevularByCalisan(int calisanId)
        {
            var randevular = _context.Randevular
                .Where(r => r.CalisanId == calisanId)
                .Select(r => new
                {
                    r.Id,
                    r.MusteriAd,
                    r.MusteriSoyad,
                    r.MusteriEmail,
                    r.TarihSaat,
                    r.Onaylandi
                })
                .ToList();

            if (!randevular.Any())
                return NotFound("Bu çalışanın randevusu bulunamadı.");

            return Ok(randevular);
        }

        // 3. Tarihe Göre Randevuları Getir
        [HttpGet("Tarih")]
        public IActionResult GetRandevularByDate([FromQuery] DateTime tarih)
        {
            var randevular = _context.Randevular
                .Where(r => r.TarihSaat.Date == tarih.Date)
                .OrderBy(r => r.TarihSaat)
                .Select(r => new
                {
                    r.Id,
                    r.MusteriAd,
                    r.MusteriSoyad,
                    r.TarihSaat,
                    OnayDurumu = r.Onaylandi ? "Onaylandı" : "Onay Bekliyor"
                })
                .ToList();

            if (!randevular.Any())
                return NotFound("Bu tarihte randevu bulunamadı.");

            return Ok(randevular);
        }

        // 4. Onay Durumuna Göre Randevuları Getir
        [HttpGet("OnayDurumu/{onaylandi}")]
        public IActionResult GetRandevularByOnayDurumu(bool onaylandi)
        {
            var randevular = _context.Randevular
                .Where(r => r.Onaylandi == onaylandi)
                .Select(r => new
                {
                    r.Id,
                    r.MusteriAd,
                    r.MusteriSoyad,
                    r.TarihSaat
                })
                .ToList();

            return Ok(randevular);
        }

        // 5. Çalışan ve İşlem Bazlı Randevuları Getir
        [HttpGet("Filtreli")]
        public IActionResult GetRandevularByCalisanAndIslem([FromQuery] int calisanId, [FromQuery] int islemId)
        {
            var randevular = _context.Randevular
                .Where(r => r.CalisanId == calisanId && r.IslemId == islemId)
                .Select(r => new
                {
                    r.Id,
                    r.MusteriAd,
                    r.MusteriSoyad,
                    r.TarihSaat
                })
                .ToList();

            if (!randevular.Any())
                return NotFound("Bu filtrelere uygun randevu bulunamadı.");

            return Ok(randevular);
        }
    }
}
