using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GenReq.Data;
using GenReq.Models;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string UserID = GetUserID();
            var obj = await _context.UserRegistration
                .Where(b => b.Name == UserID)
                .ToListAsync();
            return View(obj);
        }

        // GET: UserRegistrations/Create
        [Authorize]
        public IActionResult Create()
        {
            UserRegistration userRegistration = new UserRegistration();
            userRegistration.Name = GetUserID();
            userRegistration.RegistrationStarted = DateTime.Today;
            return View(userRegistration);
        }

        // POST: UserRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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

        private bool UserRegistrationExists(int id)
        {
            return _context.UserRegistration.Any(e => e.Id == id);
        }
    }
}
