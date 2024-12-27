
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

        public IActionResult KayitOl()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult KayitOl(UyeOl model)
        {
            if (ModelState.IsValid)
            {
                // Veritabanına kaydetme işlemi
                _context.Üyeler.Add(model);
                _context.SaveChanges();

                // Başarı mesajını ViewBag'e ata
                ViewBag.SuccessMessage = "Kayıt işleminiz başarıyla tamamlanmıştır.";
                return View();
            }

            return View(model);
        }
        
        public IActionResult Üyeler()
        {
            return RedirectToAction("Üyeler","Admin");
        }


        // Ana Sayfa: Login ve Randevu Al Seçenekleri
        public IActionResult Index()
        {
            return View();
        }

        // Admin Login Sayfası
        public IActionResult Login()
        {
            return View();
        }

        // POST: Admin Login
        [HttpPost]
        public IActionResult Login(string username, int password)
        {
            // Kullanıcı adı ve şifre doğrulaması
            if ((username == "B221210404@sakarya.edu.tr" || username == "b221210404@sakarya.edu.tr") && password == 1)
            {
                // Admin oturumunu başlat
                HttpContext.Session.SetString("Admin", "true");
                return RedirectToAction("Index", "Admin");
            }

            // Hatalı giriş durumu
            ViewBag.Error = "Hatalı kullanıcı adı veya şifre!";
            return View();
        }

        // Randevu Al Sayfası
        public IActionResult Randevu()
        {
            return RedirectToAction("RandevuOlustur");
        }

        // Randevuların Listesi
        public IActionResult Randevular()
        {
            try
            {
                // Tüm randevuları çalışan ve işlem bilgileri ile birlikte getir
                var randevular = _context.Randevular
                    .Include(r => r.Calisan)
                    .Include(r => r.Islem)
                    .Where(r => r.Onaylandi) // Sadece onaylananlar
                    .ToList();

                return View(randevular);
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi ver
                ViewBag.Error = "Randevular yüklenirken bir hata oluştu: " + ex.Message;
                return View("Error");
            }
            
        }

        // GET: Randevu Oluşturma Sayfası
        public IActionResult RandevuOlustur()
        {
            try
            {
                // Çalışanlar ve işlemleri ViewBag ile doldur
                ViewBag.Calisanlar = _context.Calisanlar.Select(c => new { c.Id, c.Ad }).ToList();
                ViewBag.Islemler = _context.Islemler.Select(i => new { i.Id, i.Ad }).ToList();
                

                return View();
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi ver
                ViewBag.Error = "Randevu oluşturma sayfası yüklenirken bir hata oluştu: " + ex.Message;
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
                    return NotFound("Randevu bulunamadı.");
                }

                // Randevuyu onayla
                randevu.Onaylandi = true;
                _context.Randevular.Update(randevu);
                _context.SaveChanges();

                return RedirectToAction("Randevular"); // Listeye geri dön
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Randevu onaylanırken bir hata oluştu: " + ex.Message;
                return View("Error");
            }
        }


        // POST: Yeni Randevu Oluşturma
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
                    return NotFound("Çalışan bulunamadı.");

                var islem = _context.Islemler.Find(yeniRandevu.IslemId);
                if (islem == null)
                    return NotFound("İşlem bulunamadı.");
                
               

                // Randevu çakışma kontrolü
                var cakisma = _context.Randevular.Any(r =>
                    r.CalisanId == yeniRandevu.CalisanId &&
                    r.TarihSaat < yeniRandevu.TarihSaat.AddMinutes(islem.Sure) &&
                    r.TarihSaat.AddMinutes(islem.Sure) > yeniRandevu.TarihSaat);

                if (cakisma)
                    return BadRequest("Bu tarih ve saat için çalışan müsait değil.");

                // Yeni randevuyu ekle ve kaydet
                _context.Randevular.Add(yeniRandevu);
                _context.SaveChanges();

                return RedirectToAction("Randevular");
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi ver
                ViewBag.Error = "Randevu kaydedilirken bir hata oluştu: " + ex.Message;
                return View("Error");
            }
        }
    }
}


