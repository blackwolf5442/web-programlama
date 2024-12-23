
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

        // Ana Sayfa: Login ve Randevu Al Se�enekleri
        public IActionResult Index()
        {
            return View();
        }

        // Admin Login Sayfas�
        public IActionResult Login()
        {
            return View();
        }

        // POST: Admin Login
        [HttpPost]
        public IActionResult Login(string username, int password)
        {
            // Kullan�c� ad� ve �ifre do�rulamas�
            if ((username == "B221210404@sakarya.edu.tr" || username == "b221210404@sakarya.edu.tr") && password == 1)
            {
                // Admin oturumunu ba�lat
                HttpContext.Session.SetString("Admin", "true");
                return RedirectToAction("Index", "Admin");
            }

            // Hatal� giri� durumu
            ViewBag.Error = "Hatal� kullan�c� ad� veya �ifre!";
            return View();
        }

        // Randevu Al Sayfas�
        public IActionResult Randevu()
        {
            return RedirectToAction("RandevuOlustur");
        }

        // Randevular�n Listesi
        public IActionResult Randevular()
        {
            try
            {
                // T�m randevular� �al��an ve i�lem bilgileri ile birlikte getir
                var randevular = _context.Randevular
                    .Include(r => r.Calisan)
                    .Include(r => r.Islem)
                    .Where(r => r.Onaylandi) // Sadece onaylananlar
                    .ToList();

                return View(randevular);
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya bilgi ver
                ViewBag.Error = "Randevular y�klenirken bir hata olu�tu: " + ex.Message;
                return View("Error");
            }
            
        }

        // GET: Randevu Olu�turma Sayfas�
        public IActionResult RandevuOlustur()
        {
            try
            {
                // �al��anlar ve i�lemleri ViewBag ile doldur
                ViewBag.Calisanlar = _context.Calisanlar.Select(c => new { c.Id, c.Ad }).ToList();
                ViewBag.Islemler = _context.Islemler.Select(i => new { i.Id, i.Ad }).ToList();
                

                return View();
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya bilgi ver
                ViewBag.Error = "Randevu olu�turma sayfas� y�klenirken bir hata olu�tu: " + ex.Message;
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
                    return NotFound("Randevu bulunamad�.");
                }

                // Randevuyu onayla
                randevu.Onaylandi = true;
                _context.Randevular.Update(randevu);
                _context.SaveChanges();

                return RedirectToAction("Randevular"); // Listeye geri d�n
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Randevu onaylan�rken bir hata olu�tu: " + ex.Message;
                return View("Error");
            }
        }


        // POST: Yeni Randevu Olu�turma
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
                    return NotFound("�al��an bulunamad�.");

                var islem = _context.Islemler.Find(yeniRandevu.IslemId);
                if (islem == null)
                    return NotFound("��lem bulunamad�.");
                
               

                // Randevu �ak��ma kontrol�
                var cakisma = _context.Randevular.Any(r =>
                    r.CalisanId == yeniRandevu.CalisanId &&
                    r.TarihSaat < yeniRandevu.TarihSaat.AddMinutes(islem.Sure) &&
                    r.TarihSaat.AddMinutes(islem.Sure) > yeniRandevu.TarihSaat);

                if (cakisma)
                    return BadRequest("Bu tarih ve saat i�in �al��an m�sait de�il.");

                // Yeni randevuyu ekle ve kaydet
                _context.Randevular.Add(yeniRandevu);
                _context.SaveChanges();

                return RedirectToAction("Randevular");
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya bilgi ver
                ViewBag.Error = "Randevu kaydedilirken bir hata olu�tu: " + ex.Message;
                return View("Error");
            }
        }
    }
}


