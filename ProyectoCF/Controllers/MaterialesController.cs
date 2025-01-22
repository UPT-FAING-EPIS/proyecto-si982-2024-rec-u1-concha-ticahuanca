using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCF.Models;

namespace ProyectoCF.Controllers
{
    public class MaterialesController : Controller
    {
        private readonly Connection _context;

        public MaterialesController(Connection context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var materiales = _context.Materiales
                .Include(m => m.Curso)
                .ToList();
            return View(materiales);
        }

        public IActionResult Create()
        {
            ViewBag.Cursos = _context.Cursos.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Material material)
        {
            _context.Materiales.Add(material);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var material = _context.Materiales.Find(id);
            if (material == null)
            {
                return NotFound();
            }
            ViewBag.Cursos = _context.Cursos.ToList();
            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Material material)
        {
            _context.Materiales.Update(material);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var material = _context.Materiales.Find(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var material = _context.Materiales.Find(id);
            if (material != null)
            {
                _context.Materiales.Remove(material);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
