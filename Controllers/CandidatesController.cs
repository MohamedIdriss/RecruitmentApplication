using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentApplication.Models;

namespace RecruitmentApplication.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly RecruitmentDbContext _context;

        public CandidatesController(RecruitmentDbContext context)
        {
            _context = context;
        }

        public IActionResult InscriptionCA()
        {
            return View();
        }

        // POST: Recruteurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InscriptionCA( Candidat candidat)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId != null)
                {
                    candidat.Id = userId.Value;
                }
                _context.Add(candidat);
                var user = _context.Users.Find(userId);
                if (user != null)
                {
                    user.ProfileCompleted = true;
                    _context.SaveChanges();
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(AllOffres));
            }
            return View(candidat);
        }


        public async Task<IActionResult> AllOffres()
        {
            var recruitmentDbContext = _context.Offres.Include(o => o.Recruteur);
            return View(await recruitmentDbContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            var allOffres = await _context.Offres.Include(o => o.Recruteur).ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allOffres.Where(n => n.Title.ToLower().Contains(searchString.ToLower()) ||  n.Description.ToLower().Contains(searchString.ToLower())).ToList();

                //  var filteredResultNew = allActors.Where(n => string.Equals(n.FullName, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                return View("AllOffres", filteredResult);
            }

            return View("AllOffres", allOffres);
        }
    }
}
