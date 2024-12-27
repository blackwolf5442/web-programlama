
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using proje.Models;
using Microsoft.EntityFrameworkCore;

namespace proje.Controllers
{
    public class AdminController : Controller
    {
        private readonly KuaforDbContext _context;

        public AdminController(KuaforDbContext context)
        {
            _context = context;
        }
        public IActionResult GoToCalisan()
        {
            return RedirectToAction("Index", "Calisan");
        }

        public IActionResult GoToIslem()
        {
            return RedirectToAction("Index", "Islem");
        }

        public IActionResult GoToSalon(int id)
        {
            return RedirectToAction("Index", "Salon", new { id = id });
        }


        public IActionResult Index()
        {
            // Admin yetkisi kontrolü
            if (HttpContext.Session.GetString("Admin") != "true")
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }
        public IActionResult Üyeler()
        {
            var kullanicilar = _context.Üyeler.ToList();
            return View(kullanicilar);
        }

        // Çalışanları Listele
        public IActionResult Calisanlar()
        {
            var calisanlar = _context.Calisanlar.ToList();
            return View(calisanlar);
        }

        // İşlemleri Listele
        public IActionResult Islemler()
        {
            var islemler = _context.Islemler.ToList();
            return View(islemler);
        }

        // Salonları Listele
        public IActionResult Salonlar()
        {
            var salonlar = _context.Salonlar.ToList();
            return View(salonlar);
        }

        public IActionResult OnayBekleyenRandevular()
        {
            // Onay bekleyen randevuları getir
            var bekleyenRandevular = _context.Randevular
                .Include(r => r.Calisan)
                .Include(r => r.Islem)
                .Where(r => !r.Onaylandi && r.AdminOnayli == "Beklemede")
                .ToList();

            return View(bekleyenRandevular);
        }

        [HttpPost]
        public IActionResult RandevuOnayla(int id)
        {
            try
            {
                var randevu = _context.Randevular.Find(id);
                if (randevu == null)
                    return NotFound("Randevu bulunamadı.");

                randevu.Onaylandi = true;
                //randevu.AdminOnayli = "Onaylandı";
                _context.Randevular.Update(randevu);
                _context.SaveChanges();

                return RedirectToAction("OnayBekleyenRandevular");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Randevu onaylanırken bir hata oluştu: " + ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult RandevuReddet(int id)
        {
            try
            {
                var randevu = _context.Randevular.Find(id);
                if (randevu == null)
                    return NotFound("Randevu bulunamadı.");

                randevu.AdminOnayli = "Reddedildi";
                _context.Randevular.Update(randevu);
                _context.SaveChanges();

                return RedirectToAction("OnayBekleyenRandevular");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Randevu reddedilirken bir hata oluştu: " + ex.Message;
                return View("Error");
            }
        }





    }
}


