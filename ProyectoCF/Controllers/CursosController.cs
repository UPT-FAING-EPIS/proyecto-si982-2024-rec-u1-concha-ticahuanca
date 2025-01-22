using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCF.Models;


namespace ProyectoCF.Controllers
{
    public class CursosController : Controller
    {
        private readonly Connection _context;

        public CursosController(Connection context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador" && rol != "Docente")
            {
                return Forbid();
            }

            var cursos = _context.Cursos
                .Select(curso => new CursoViewModel
                {
                    Id = curso.Id,
                    Nombre = curso.Nombre,
                    Descripcion = curso.Descripcion,
                    DocenteNombre = _context.Usuarios
                        .Where(u => u.Id == curso.DocenteId)
                        .Select(u => u.Nombre + " " + u.Apellido)
                        .FirstOrDefault() ?? "No asignado"
                })
                .ToList();

            return View(cursos);
        }


        public IActionResult Details(int id)
        {
            var curso = _context.Cursos
                .Where(c => c.Id == id)
                .Select(c => new Curso
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    DocenteId = c.DocenteId,
                    Materiales = _context.Materiales
                        .Where(m => m.CursoId == c.Id)
                        .ToList()
                })
                .FirstOrDefault();

            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }


        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador" && HttpContext.Session.GetString("Rol") != "Docente")
            {
                return Forbid();
            }

            ViewBag.Docentes = _context.Usuarios.Where(u => u.Rol == "Docente").ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Curso curso)
        {
            if (ModelState.IsValid)
            {
                _context.Cursos.Add(curso);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Docentes = _context.Usuarios.Where(u => u.Rol == "Docente").ToList();
            return View(curso);
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador" && HttpContext.Session.GetString("Rol") != "Docente")
            {
                return Forbid();
            }

            var curso = _context.Cursos.Find(id);
            if (curso == null)
            {
                return NotFound();
            }

            ViewBag.Docentes = _context.Usuarios.Where(u => u.Rol == "Docente").ToList();
            return View(curso);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Curso curso)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador" && HttpContext.Session.GetString("Rol") != "Docente")
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                _context.Cursos.Update(curso);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Docentes = _context.Usuarios.Where(u => u.Rol == "Docente").ToList();
            return View(curso);
        }


        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador" && HttpContext.Session.GetString("Rol") != "Docente")
            {
                return Forbid();
            }

            var curso = _context.Cursos.Find(id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador" && HttpContext.Session.GetString("Rol") != "Docente")
            {
                return Forbid();
            }

            var curso = _context.Cursos.Find(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
