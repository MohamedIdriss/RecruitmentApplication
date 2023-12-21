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
    public class UsersController : Controller
    {
        private readonly RecruitmentDbContext _context;

        public UsersController(RecruitmentDbContext context)
        {
            _context = context;
        }

       


        // GET: Users/Create
        public IActionResult Inscription()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscription([Bind("Id,Email,Password,Role,ProfileCompleted")] User user)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email is already taken.");
                    return View(user);
                }

                user.ProfileCompleted = false;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                var u = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (u == null)
                {
                    return NotFound();
                }
                HttpContext.Session.SetInt32("UserId", u.Id);
                HttpContext.Session.SetString("UserRole", u.Role);
                if (user.Role == "Recruteur") {
                    return RedirectToAction("InscriptionRH", "Recruteurs");
                }
                return RedirectToAction("InscriptionCA", "Candidates");
            }
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            user.Id = 0;
            user.ProfileCompleted=false; 
            user.Role = "Recruteur";
            
            if (ModelState.IsValid)
            {
                var u = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (u == null)
                {
                    return NotFound();
                }
                HttpContext.Session.SetInt32("UserId", u.Id);
                HttpContext.Session.SetString("UserRole", u.Role);
                if (u.Role == "Recruteur")
                {
                    if (u.ProfileCompleted == false)
                    {
                        return RedirectToAction("InscriptionRH", "Recruteurs");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Offres");
                    }
                    
                }
                else
                {
                    if (u.ProfileCompleted == false)
                    {
                        RedirectToAction("InscriptionCA", "Candidates");
                    }
                    else
                    {
                        return RedirectToAction("AllOffres", "Candidates");
                    }
                }
                
            }
            return View(user);
        }
        public IActionResult Logout()
        {


            // Optionally, clear session data or perform additional logout-related tasks
            HttpContext.Session.Clear();


            // Redirect to the login page or any other desired destination
            return RedirectToAction("Login");
        }


        /*
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,Role")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }*/
    }
}
