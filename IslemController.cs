using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

using proje.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace proje.Controllers
{
    public class IslemController : Controller
    {
        private readonly KuaforDbContext _context;

        public IslemController(KuaforDbContext context)
        {
            _context = context;
        }

        // GET: /Islemler/Index
        public async Task<IActionResult> Index()
        {
            var islemler = await _context.Islemler.ToListAsync();
            return View(islemler);
        }

        // GET: /Islemler/Create
        public IActionResult Create()
        {
            ViewBag.SalonId = new SelectList(_context.Salonlar, "Id", "Ad");
            return View();
        }

        // POST: /Islemler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Islem islem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(islem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            _context.Add(islem);
            await _context.SaveChangesAsync();
            ViewBag.SalonId = new SelectList(_context.Salonlar, "Id", "Ad");
            return View(islem);
        }

        // GET: /Islemler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var islem = await _context.Islemler.FindAsync(id);
            if (islem == null)
            {
                return NotFound();
            }
            ViewBag.SalonId = new SelectList(_context.Salonlar, "Id", "Ad", islem.SalonId);
            return View(islem);
        }

        // POST: /Islemler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Islem islem)
        {
            if (id != islem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(islem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IslemExists(islem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            _context.Update(islem);
            await _context.SaveChangesAsync();
            ViewBag.SalonId = new SelectList(_context.Salonlar, "Id", "Ad", islem.SalonId);
            return View(islem);
        }

        // GET: /Islemler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var islem = await _context.Islemler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (islem == null)
            {
                return NotFound();
            }

            return View(islem);
        }

        // POST: /Islemler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var islem = await _context.Islemler.FindAsync(id);
            _context.Islemler.Remove(islem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IslemExists(int id)
        {
            return _context.Islemler.Any(e => e.Id == id);
        }
    }
}
