using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManagementApplication.Data;
using ManagementApplication.Models;

namespace ManagementApplication.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly ManagementApplicationContext _context;

        public CandidatesController(ManagementApplicationContext context)
        {
            _context = context;
        }

        // GET: Candidates
        public async Task<IActionResult> Index()
        {
            return View(await _context.Candidate.ToListAsync());
        }

        // GET: Candidates/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidate
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // GET: Candidates/Create
        public IActionResult Create()
        {
            var enumData = from ApplicationStatus appStatus in Enum.GetValues(typeof(ApplicationStatus))
                           select new SelectListItem
                           {
                               Value = ((int)appStatus).ToString(),
                               Text = appStatus.ToString()
                           };

            ViewData["ApplicationStatus"] = enumData;

            return View();
        }

        // POST: Candidates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateId,LastName,FirstName,Email,Mobile,ApplicationStatus,Comments,CreationTime")] Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                candidate.CandidateId = Guid.NewGuid();
                candidate.CreationTime = DateTime.Now;
                _context.Add(candidate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(candidate);
        }

        // GET: Candidates/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidate.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }

            var enumData = from ApplicationStatus appStatus in Enum.GetValues(typeof(ApplicationStatus))
                           select new SelectListItem
                           {
                               Value = ((int)appStatus).ToString(),
                               Text = appStatus.ToString()
                           };

            ViewData["ApplicationStatus"] = enumData;

            

            return View(candidate);
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CandidateId,LastName,FirstName,Email,Mobile,ApplicationStatus,Comments,CreationTime")] Candidate candidate)
        {
            if (id != candidate.CandidateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateExists(candidate.CandidateId))
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
            return View(candidate);
        }

        // GET: Candidates/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidate
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var candidate = await _context.Candidate.FindAsync(id);
            if (candidate != null)
            {
                _context.Candidate.Remove(candidate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidateExists(Guid id)
        {
            return _context.Candidate.Any(e => e.CandidateId == id);
        }
    }
}
