using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GenReq.Data;
using GenReq.Models;

namespace GenReq.Controllers
{
    public class UserRegistrationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string GetUserID()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string UserID = currentUser.Identity.Name;
            return UserID;
        }

        public UserRegistrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserRegistrations
        public async Task<IActionResult> Index()
        {
            string UserID = GetUserID();
            var obj = await _context.UserRegistration
                .Where(b => b.Name == UserID)
                .ToListAsync();
            return View(obj);
        }

        // GET: UserRegistrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string UserID = GetUserID();
            var userRegistration = await _context.UserRegistration
                .Where(b => b.Name == UserID)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRegistration == null)
            {
                return NotFound();
            }

            return View(userRegistration);
        }

        // GET: UserRegistrations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,RegistrationType,RegistrationStarted,RegistrationEnded")] UserRegistration userRegistration)
        {
            if (ModelState.IsValid)
            {
                userRegistration.Name = GetUserID();
                userRegistration.RegistrationStarted = DateTime.Today;
                _context.Add(userRegistration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userRegistration);
        }

        // GET: UserRegistrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string UserID = GetUserID();
            var userRegistration = await _context.UserRegistration
                .Where(b => b.Name == UserID)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (userRegistration == null)
            {
                return NotFound();
            }
            return View(userRegistration);
        }

        // POST: UserRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,RegistrationType,RegistrationStarted,RegistrationEnded")] UserRegistration userRegistration)
        {
            if (id != userRegistration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRegistration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRegistrationExists(userRegistration.Id))
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
            return View(userRegistration);
        }

        // GET: UserRegistrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string UserID = GetUserID();
            var userRegistration = await _context.UserRegistration
                .Where(b => b.Name == UserID)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (userRegistration == null)
            {
                return NotFound();
            }

            return View(userRegistration);
        }

        // POST: UserRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string UserID = GetUserID();
            var userRegistration = await _context.UserRegistration
                .Where(b => b.Name == UserID)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRegistration != null)
            {
                _context.UserRegistration.Remove(userRegistration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRegistrationExists(int id)
        {
            return _context.UserRegistration.Any(e => e.Id == id);
        }
    }
}
