using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using proje.Models;
using System.Linq;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
namespace proje.Controllers
{
   

    public class CalisanController : Controller
    {
        private readonly KuaforDbContext _context;

        public CalisanController(KuaforDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var calisanlar = _context.Calisanlar.Include(c => c.Salon).ToList();//var calisanlar = _context.Calisanlar.ToList();
            return View(calisanlar);
        }

        public IActionResult Create()
        {
            ViewBag.SalonId = new SelectList(_context.Salonlar, "Id", "Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Calisan calisan)
        {
            if (ModelState.IsValid)
            {
                _context.Calisanlar.Add(calisan);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            _context.Calisanlar.Add(calisan);
            _context.SaveChanges();
            
            ViewBag.SalonId = new SelectList(_context.Salonlar, "Id", "Ad");
            return View(calisan);

        }

        public IActionResult Edit(int id)
        {
            var calisan = _context.Calisanlar.Find(id);
            if (calisan == null) return NotFound();
            ViewBag.SalonId = new SelectList(_context.Salonlar, "Id", "Ad", calisan.SalonId);
            return View(calisan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Calisan calisan)
        {
            if (id != calisan.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Calisanlar.Update(calisan);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SalonId = new SelectList(_context.Salonlar, "Id", "Ad", calisan.SalonId);
            return View(calisan);
        }

        public IActionResult Delete(int id)
        {
            var calisan = _context.Calisanlar.Find(id);
            if (calisan == null) return NotFound();
            return View(calisan);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var calisan = _context.Calisanlar
                .Include(c => c.Randevular) // Çalışana bağlı randevuları da yükle
                .FirstOrDefault(c => c.Id == id);

            if (calisan != null)
            {
                // Çalışana bağlı tüm randevuları sil
                if (calisan.Randevular != null && calisan.Randevular.Any())
                {
                    _context.Randevular.RemoveRange(calisan.Randevular);
                }

                // Çalışanı sil
                _context.Calisanlar.Remove(calisan);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

    }

}
