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
    public class GenRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GenRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GenRequests
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reminders.ToListAsync());
        }

        // GET: GenRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genRequest = await _context.Reminders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genRequest == null)
            {
                return NotFound();
            }

            return View(genRequest);
        }

        // GET: GenRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GenRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CreatedDate,GeneratedDate,Actor,Status")] GenRequest genRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genRequest);
        }

        // GET: GenRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genRequest = await _context.Reminders.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CreatedDate,GeneratedDate,Actor,Status")] GenRequest genRequest)
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genRequest = await _context.Reminders
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genRequest = await _context.Reminders.FindAsync(id);
            if (genRequest != null)
            {
                _context.Reminders.Remove(genRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenRequestExists(int id)
        {
            return _context.Reminders.Any(e => e.Id == id);
        }
    }
}
