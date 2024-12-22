/*namespace proje.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using proje.Models;
    using System.Linq;

    public class AdminController : Controller
    {
        private readonly KuaforDbContext _context;

        public AdminController(KuaforDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/
        public IActionResult Index()
        {
            ViewBag.SalonSayisi = _context.Salonlar.Count();
            ViewBag.CalisanSayisi = _context.Calisanlar.Count();
            ViewBag.IslemSayisi = _context.Islemler.Count();
            ViewBag.RandevuSayisi = _context.Randevular.Count();

            return View();
        }

        // GET: /Admin/Salonlar
        public IActionResult Salonlar()
        {
            var salonlar = _context.Salonlar.ToList();
            return View(salonlar);
        }

        // GET: /Admin/Calisanlar
        public IActionResult Calisanlar()
        {
            var calisanlar = _context.Calisanlar.ToList();
            return View(calisanlar);
        }

        // GET: /Admin/Islemler
        public IActionResult Islemler()
        {
            var islemler = _context.Islemler.ToList();
            return View(islemler);
        }

        // GET: /Admin/Randevular
        public IActionResult Randevular()
        {
            var randevular = _context.Randevular.ToList();
            return View(randevular);
        }

        // Admin Giriş Sayfası
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "B221210404@sakarya.edu.tr" && password == "sau")
            {
                TempData["AdminGiris"] = "true";
                return RedirectToAction("Index");
            }
            ViewBag.Hata = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        // Admin Çıkış
        public IActionResult Logout()
        {
            TempData.Remove("AdminGiris");
            return RedirectToAction("Login");
        }
    }

}*/
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

        
    }
}


