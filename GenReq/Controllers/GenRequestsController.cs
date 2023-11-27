using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GenReq.Data;
using GenReq.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Graph;

namespace GenReq.Controllers
{
    public class GenRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GenRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserID()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string UserID = currentUser.Identity.Name;
            return UserID;
        }

        // GET: GenRequests
        [Authorize]
        public async Task<IActionResult> Index()
        {
            Response.Headers.Append("Refresh", "60");
            string UserID = GetUserID();
            // .Where(b => b.Url.Contains("dotnet"))
            var obj = await _context.GenRequest
                .Where(b => b.OwningUserId == UserID)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
            return View(obj);
        }

        // GET: GenRequests/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string UserID = GetUserID();
            var genRequest = await _context.GenRequest
                .Where(b => b.OwningUserId == UserID)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (genRequest == null)
            {
                return NotFound();
            }

            return View(genRequest);
        }

        // GET: GenRequests/Create
        [Authorize]
        public IActionResult Create()
        {
            var genRequest = new GenRequest();
            genRequest.CreatedDate = DateTime.Now;
            genRequest.Status = "Requested";
            genRequest.OwningUserId = GetUserID();
            return View(genRequest);
        }

        // POST: GenRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,OwningUserId,Title,CreatedDate,GeneratedDate,Actor,Status")] GenRequest genRequest)
        {
            if (ModelState.IsValid)
            {
                genRequest.CreatedDate = DateTime.Now;
                genRequest.Status = "Requested";
                genRequest.OwningUserId = GetUserID();
                _context.Add(genRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genRequest);
        }

        // GET: GenRequests/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string UserID = GetUserID();
            var genRequest = await _context.GenRequest
                .Where(b => b.OwningUserId == UserID)
                .FirstOrDefaultAsync(m => m.Id == id); // was FindAsync
            if (genRequest == null)
            {
                return NotFound();
            }
            return View(genRequest);
        }

        // POST: GenRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwningUserId,Title,CreatedDate,GeneratedDate,Actor,Status")] GenRequest genRequest)
        {
            if (id != genRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenRequestExists(genRequest.Id))
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
            return View(genRequest);
        }

        // GET: GenRequests/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string UserID = GetUserID();
            var genRequest = await _context.GenRequest
                .Where(b => b.OwningUserId == UserID)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genRequest == null)
            {
                return NotFound();
            }

            return View(genRequest);
        }

        // POST: GenRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string UserID = GetUserID();
            var genRequest = await _context.GenRequest
                .Where(b => b.OwningUserId == UserID)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genRequest != null)
            {
                _context.GenRequest.Remove(genRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenRequestExists(int id)
        {
            return _context.GenRequest
                .Where(b => b.OwningUserId == GetUserID())
                .Any(e => e.Id == id);
        }
    }
}
