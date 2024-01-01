using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecruitmentApplication.Models;

namespace RecruitmentApplication.Controllers
{
    public class RecruteursController : Controller
    {
        private readonly RecruitmentDbContext _context;

        public RecruteursController(RecruitmentDbContext context)
        {
            _context = context;
        }

       

   

        // GET: Recruteurs/Create
        public IActionResult InscriptionRH()
        {
            return View();
        }

        // POST: Recruteurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InscriptionRH([Bind("Id,Name,LastName,CompanyName,Url,Address")] Recruteur recruteur)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if(userId != null)
                {
                    recruteur.Id = userId.Value;
                }
                _context.Add(recruteur);
                var user = _context.Users.Find(userId);
                if (user != null)
                {
                    user.ProfileCompleted = true;
                    _context.SaveChanges();
                }
                await _context.SaveChangesAsync();
                int profileCompletedValue = user.ProfileCompleted ? 1 : 0;
                HttpContext.Session.SetInt32("ProfileCompleted", profileCompletedValue);

                return RedirectToAction("Index","Offres");
            }
            return View(recruteur);
        }

        public async Task<IActionResult> lescandidaturesSelonOffre()
        {

            var userId = HttpContext.Session.GetInt32("UserId");
            var candidatures = _context.OffreCandidates
            .Where(co => co.Offre.RecruteurId == userId.Value)
             .Include(co => co.Offre.OffreCandidates)
             .ThenInclude(co => co.Candidate)
             .ToList();




            return View("lescandidaturesSelonOffre", candidatures);


        }

        public async Task<IActionResult> Edit(int? idOffre, int? idCandidate)
        {
            if (idOffre == null || idCandidate == null || _context.OffreCandidates == null)
            {
                return NotFound();
            }

            var offreCandidate = await _context.OffreCandidates.Include(o => o.Candidate)
                .Include(o => o.Offre)
                .FirstOrDefaultAsync(m => m.CandidateId == idCandidate && m.OffreId == idOffre); ;
            if (offreCandidate == null)
            {
                return NotFound();
            }
            ViewData["CandidateId"] = new SelectList(_context.Candidats, "Id", "Id", offreCandidate.CandidateId);
            ViewData["OffreId"] = new SelectList(_context.Offres, "Id", "Id", offreCandidate.OffreId);
            return View(offreCandidate);
        }

        // POST: OffreCandidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("CandidateId,OffreId,Status")] OffreCandidate offreCandidate)
        {
            
           
                try
                {
                    _context.Update(offreCandidate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!OffreCandidateExists(offreCandidate.CandidateId))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(lescandidaturesSelonOffre));
            
            //ViewData["CandidateId"] = new SelectList(_context.Candidats, "Id", "Id", offreCandidate.CandidateId);
            //ViewData["OffreId"] = new SelectList(_context.Offres, "Id", "Id", offreCandidate.OffreId);
            //return View(offreCandidate);
        }
        private bool OffreCandidateExists(int id)
        {
            return (_context.OffreCandidates?.Any(e => e.CandidateId == id)).GetValueOrDefault();
        }


       
        public async Task<IActionResult> DetailsProfile()
        {

            var userId = HttpContext.Session.GetInt32("UserId");

            var recruteur = await _context.Recruteurs
                .FirstOrDefaultAsync(m => m.Id == userId.Value);
            if (recruteur == null)
            {
                return NotFound();
            }

            return View(recruteur);
        }


        public async Task<IActionResult> EditProfile(int? id)
        {
            if (id == null || _context.Recruteurs == null)
            {
                return NotFound();
            }

            var recruteur = await _context.Recruteurs.FindAsync(id);
            if (recruteur == null)
            {
                return NotFound();
            }
            return View(recruteur);
        }

        // POST: Recruteurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(int id, [Bind("Id,Name,LastName,CompanyName,Url,Address")] Recruteur recruteur)
        {
            if (id != recruteur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recruteur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecruteurExists(recruteur.Id))
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
            return View(recruteur);
        }
        private bool RecruteurExists(int id)
        {
            return (_context.Recruteurs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // POST: Recruteurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,CompanyName,Url,Address")] Recruteur recruteur)
        //{
        //    if (id != recruteur.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(recruteur);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!RecruteurExists(recruteur.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(recruteur);
        //}


        /*     // GET: Recruteurs/Details/5
                public async Task<IActionResult> Details(int? id)
                {
                    if (id == null || _context.Recruteurs == null)
                    {
                        return NotFound();
                    }

                    var recruteur = await _context.Recruteurs
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (recruteur == null)
                    {
                        return NotFound();
                    }

                    return View(recruteur);
                }



                // GET: Recruteurs/Edit/5
                public async Task<IActionResult> Edit(int? id)
                {
                    if (id == null || _context.Recruteurs == null)
                    {
                        return NotFound();
                    }

                    var recruteur = await _context.Recruteurs.FindAsync(id);
                    if (recruteur == null)
                    {
                        return NotFound();
                    }
                    return View(recruteur);
                }

                // POST: Recruteurs/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,CompanyName,Url,Address")] Recruteur recruteur)
                {
                    if (id != recruteur.Id)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(recruteur);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!RecruteurExists(recruteur.Id))
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
                    return View(recruteur);
                }



                private bool RecruteurExists(int id)
                {
                  return (_context.Recruteurs?.Any(e => e.Id == id)).GetValueOrDefault();
                }*/
    }
}
