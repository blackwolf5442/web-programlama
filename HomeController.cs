
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proje.Models;

namespace proje.Controllers
{
    public class HomeController : Controller
    {
        private readonly KuaforDbContext _context;

        // Constructor'da DbContext'i enjekte ediyoruz
        public HomeController(KuaforDbContext context)
        {
            _context = context;
        }

        // Ana Sayfa: Login ve Randevu Al Seçenekleri
        public IActionResult Index()
        {
            return View();
        }

        // Admin Login Sayfasý
        public IActionResult Login()
        {
            return View();
        }

        // POST: Admin Login
        [HttpPost]
        public IActionResult Login(string username, int password)
        {
            // Kullanýcý adý ve þifre doðrulamasý
            if ((username == "B221210404@sakarya.edu.tr" || username == "b221210404@sakarya.edu.tr") && password == 1)
            {
                // Admin oturumunu baþlat
                HttpContext.Session.SetString("Admin", "true");
                return RedirectToAction("Index", "Admin");
            }

            // Hatalý giriþ durumu
            ViewBag.Error = "Hatalý kullanýcý adý veya þifre!";
            return View();
        }

        // Randevu Al Sayfasý
        public IActionResult Randevu()
        {
            return RedirectToAction("RandevuOlustur");
        }

        // Randevularýn Listesi
        public IActionResult Randevular()
        {
            try
            {
                // Tüm randevularý çalýþan ve iþlem bilgileri ile birlikte getir
                var randevular = _context.Randevular
                    .Include(r => r.Calisan)
                    .Include(r => r.Islem)
                    .Where(r => r.Onaylandi) // Sadece onaylananlar
                    .ToList();

                return View(randevular);
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya bilgi ver
                ViewBag.Error = "Randevular yüklenirken bir hata oluþtu: " + ex.Message;
                return View("Error");
            }
            
        }

        // GET: Randevu Oluþturma Sayfasý
        public IActionResult RandevuOlustur()
        {
            try
            {
                // Çalýþanlar ve iþlemleri ViewBag ile doldur
                ViewBag.Calisanlar = _context.Calisanlar.Select(c => new { c.Id, c.Ad }).ToList();
                ViewBag.Islemler = _context.Islemler.Select(i => new { i.Id, i.Ad }).ToList();
                

                return View();
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya bilgi ver
                ViewBag.Error = "Randevu oluþturma sayfasý yüklenirken bir hata oluþtu: " + ex.Message;
                return View("Error");
            }
        }

        public IActionResult RandevuOnayla(int id)
        {
            try
            {
                // Randevuyu bul
                var randevu = _context.Randevular.Find(id);
                if (randevu == null)
                {
                    return NotFound("Randevu bulunamadý.");
                }

                // Randevuyu onayla
                randevu.Onaylandi = true;
                _context.Randevular.Update(randevu);
                _context.SaveChanges();

                return RedirectToAction("Randevular"); // Listeye geri dön
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Randevu onaylanýrken bir hata oluþtu: " + ex.Message;
                return View("Error");
            }
        }


        // POST: Yeni Randevu Oluþturma
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RandevuOlustur(Randevu yeniRandevu)
        {
            if (yeniRandevu == null)
                return BadRequest("Randevu bilgileri eksik.");

            try
            {
                var calisan = _context.Calisanlar.Find(yeniRandevu.CalisanId);
                if (calisan == null)
                    return NotFound("Çalýþan bulunamadý.");

                var islem = _context.Islemler.Find(yeniRandevu.IslemId);
                if (islem == null)
                    return NotFound("Ýþlem bulunamadý.");
                
               

                // Randevu çakýþma kontrolü
                var cakisma = _context.Randevular.Any(r =>
                    r.CalisanId == yeniRandevu.CalisanId &&
                    r.TarihSaat < yeniRandevu.TarihSaat.AddMinutes(islem.Sure) &&
                    r.TarihSaat.AddMinutes(islem.Sure) > yeniRandevu.TarihSaat);

                if (cakisma)
                    return BadRequest("Bu tarih ve saat için çalýþan müsait deðil.");

                // Yeni randevuyu ekle ve kaydet
                _context.Randevular.Add(yeniRandevu);
                _context.SaveChanges();

                return RedirectToAction("Randevular");
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya bilgi ver
                ViewBag.Error = "Randevu kaydedilirken bir hata oluþtu: " + ex.Message;
                return View("Error");
            }
        }
    }
}


