﻿using Microsoft.AspNetCore.Authorization;
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
                int profileCompletedValue = user.ProfileCompleted ? 1 : 0;
                HttpContext.Session.SetInt32("ProfileCompleted", profileCompletedValue);
                return RedirectToAction(nameof(AllOffres));
            }
            return View(candidat);
        }


        public async Task<IActionResult> AllOffres()
        {
            var recruitmentDbContext = _context.Offres.Include(o => o.Recruteur).Where(o => o.Publier == true);
            return View(await recruitmentDbContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            var allOffres = await _context.Offres.Include(o => o.Recruteur).Where(o => o.Publier == true).ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allOffres.Where(n => n.Title.ToLower().Contains(searchString.ToLower()) ||  n.Description.ToLower().Contains(searchString.ToLower())).ToList();

                //  var filteredResultNew = allActors.Where(n => string.Equals(n.FullName, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                return View("AllOffres", filteredResult);
            }

            return View("AllOffres", allOffres);
        }


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

        public async Task<IActionResult> Postuler(int? id)
        {
            if (id == null || _context.Offres == null)
            {
                return NotFound();
            }
            var userId = HttpContext.Session.GetInt32("UserId");
            var recruitmentDbContext = _context.Offres.Include(o => o.Recruteur).Where(o => o.Publier == true);

            bool hasAlreadyApplied = _context.OffreCandidates
  .Any(co => co.CandidateId == userId && co.OffreId == id);

            if (hasAlreadyApplied)
            {
                TempData["ErrorMessage"] = "You have already applied for this job offer.";
                return RedirectToAction("AllOffres", await recruitmentDbContext.ToListAsync());
            }
            OffreCandidate oc = new OffreCandidate {
                CandidateId = userId.Value ,
                OffreId = (int)id,
                Status = "Pending"
            };
            _context.OffreCandidates.Add(oc);
            _context.SaveChanges();
           
            TempData["SuccessMessage"] = "Operation was successful.";


            return View("AllOffres", await recruitmentDbContext.ToListAsync());
        }


        public async Task<IActionResult> Suivremescandidatures()
        {

            var userId = HttpContext.Session.GetInt32("UserId");


            var candidatures = _context.OffreCandidates
         .Where(co => co.CandidateId == userId)
         .Include(co => co.Offre.Recruteur);


            return View("Suivremescandidatures", candidatures);


        }


        public async Task<IActionResult> EditProfile(int? id)
        {
            if (id == null || _context.Candidats == null)
            {
                return NotFound();
            }

            var candidat = await _context.Candidats.FindAsync(id);
            if (candidat == null)
            {
                return NotFound();
            }
            return View(candidat);
        }

        // POST: Test/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(int id, [Bind("Id,Name,LastName,University,Date,Framework,Langue,GithubUrl,Tel,StageExperience")] Candidat candidat)
        {
            if (id != candidat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidatExists(candidat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DetailsProfile));
            }
            return View(candidat);
        }

        private bool CandidatExists(int id)
        {
            return (_context.Candidats?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> DetailsProfile()
        {

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId.Value == null || _context.Candidats == null)
            {
                return NotFound();
            }

            var candidat = await _context.Candidats
                .FirstOrDefaultAsync(m => m.Id == userId.Value);
            if (candidat == null)
            {
                return NotFound();
            }

            return View(candidat);
        }

    }
}
