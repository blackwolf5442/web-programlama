using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using proje.Models;
using System.Linq;
namespace proje.Controllers
{
    

    public class SalonController : Controller
    {
        private readonly KuaforDbContext _context;

        public SalonController(KuaforDbContext context)
        {
            _context = context;
        }

        // GET: /Salon/
        public IActionResult Index()
        {
            var salonlar = _context.Salonlar.ToList();
            return View(salonlar);
        }

        // GET: /Salon/Create
        public IActionResult Create()
        {
            //ViewBag.SalonId = new SelectList(_context.Salonlar, "Id", "Ad");
            return View();
        }

        // POST: /Salon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Salon salon)
        {
            if (ModelState.IsValid)
            {
                _context.Salonlar.Add(salon);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(salon);
        }

        // GET: /Salon/Edit/5
        public IActionResult Edit(int id)
        {
            var salon = _context.Salonlar.Find(id);
            if (salon == null) return NotFound();
            return View(salon);
        }

        // POST: /Salon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Salon salon)
        {
            if (id != salon.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Salonlar.Update(salon);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(salon);
        }

        // GET: /Salon/Delete/5
        public IActionResult Delete(int id)
        {
            var salon = _context.Salonlar.Find(id);
            if (salon == null) return NotFound();
            return View(salon);
        }

        // POST: /Salon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var salon = _context.Salonlar.Find(id);
            if (salon != null)
            {
                _context.Salonlar.Remove(salon);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }

}
