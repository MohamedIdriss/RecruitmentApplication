using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecruitmentApplication.Models;

namespace RecruitmentApplication.Controllers
{
    public class OffresController : Controller
    {
        private readonly RecruitmentDbContext _context;

        public OffresController(RecruitmentDbContext context)
        {
            _context = context;
        }

        // GET: Offres
        //public async Task<IActionResult> Index()
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");
            
        //    var recruitmentDbContext = _context.Offres.Where(o => o.RecruteurId == userId.Value) ;
        //    return View(await recruitmentDbContext.ToListAsync());
        //}

        //[AllowAnonymous]
        //public async Task<IActionResult> Filter(string searchString)
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");

        //    var allOffres = await _context.Offres.Include(o => o.Recruteur).Where(o => o.RecruteurId == userId.Value).ToListAsync();

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        var filteredResult = allOffres.Where(n => n.Title.ToLower().Contains(searchString.ToLower()) || n.Description.ToLower().Contains(searchString.ToLower())).ToList();

        //        //  var filteredResultNew = allActors.Where(n => string.Equals(n.FullName, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

        //        return View("Index", filteredResult);
        //    }

        //    return View("Index", allOffres);
        //}


        public IActionResult Index(string titre, string type)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            IQueryable list = _context.Offres.Include(o => o.Recruteur).Where(o => o.RecruteurId == userId.Value);
            var lesgenre = new List<string> { "Publier", "Archiver" };
            ViewBag.type = new SelectList(lesgenre);

            if (titre != null && type != null)
            {  if (type == "Publier")
                {
                    list = from m in _context.Offres.Include(o => o.Recruteur).Where(o => o.RecruteurId == userId.Value && o.Title.Contains(titre)
                           && o.Publier == true)
                           select m;

                }
                else
                {
                    list = from m in _context.Offres.Include(o => o.Recruteur).Where(o => o.RecruteurId == userId.Value && o.Title.Contains(titre)
                           && o.Archiver == true)
                           select m;

                }

        }
            else if (titre == null && type == null)
            {
                list = from m in _context.Offres.Include(o => o.Recruteur).Where(o => o.RecruteurId == userId.Value)
                select m;
            }

            else if (titre == null)
            {
                if (type == "Publier")
                {
                    list = from m in _context.Offres.Include(o => o.Recruteur).Where(o => o.RecruteurId == userId.Value 
                           && o.Publier == true)
                           select m;

                }
                else
                {
                    list = from m in _context.Offres.Include(o => o.Recruteur).Where(o => o.RecruteurId == userId.Value 
                           && o.Archiver == true)
                           select m;

                }
            }
            else if (type == null)
            {
                list = from m in _context.Offres.Include(o => o.Recruteur)
                       where m.RecruteurId == userId.Value && m.Title.Contains(titre)
                       select m;
            }
            return View(list);
        }


        // GET: Offres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Offres == null)
            {
                return NotFound();
            }

            var offre = await _context.Offres
                .Include(o => o.Recruteur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offre == null)
            {
                return NotFound();
            }

            return View(offre);
        }

        // GET: Offres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Offres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Publier,Archiver,RecruteurId")] Offre offre)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            offre.RecruteurId = userId.Value;

            try
            {

                _context.Add(offre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(offre);
            }
            
        }

        // GET: Offres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Offres == null)
            {
                return NotFound();
            }

            var offre = await _context.Offres.FindAsync(id);
            if (offre == null)
            {
                return NotFound();
            }
            return View(offre);
        }

        // POST: Offres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Publier,Archiver,RecruteurId")] Offre offre)
        {
            if (id != offre.Id)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            offre.RecruteurId = userId.Value;
           
                try
                {
                    _context.Update(offre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OffreExists(offre.Id))
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

        // GET: Offres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Offres == null)
            {
                return NotFound();
            }

            var offre = await _context.Offres
                .Include(o => o.Recruteur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offre == null)
            {
                return NotFound();
            }

            return View(offre);
        }

        // POST: Offres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Offres == null)
            {
                return Problem("Entity set 'RecruitmentDbContext.Offres'  is null.");
            }
            var offre = await _context.Offres.FindAsync(id);
            if (offre != null)
            {
                _context.Offres.Remove(offre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OffreExists(int id)
        {
          return (_context.Offres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
