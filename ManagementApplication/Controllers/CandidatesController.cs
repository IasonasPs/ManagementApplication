using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManagementApplication.Data;
using ManagementApplication.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ManagementApplication.Controllers
{
    public class CandidatesController:Controller
    {
        private readonly ManagementApplicationContext _context;

        public CandidatesController(ManagementApplicationContext context)
        {
            _context = context;
        }

        // GET: Candidates
        public async Task<IActionResult> Index(ApplicationStatus? statusFilter)
        {
            ViewData["CurrentFilter"] = statusFilter;

            var candidates =  _context.Candidate.AsQueryable();

            ViewBag.ApplicationStatusSelectList = new SelectList(Enum.GetValues(typeof(ApplicationStatus)));


            if (statusFilter != null)
            {
                candidates = candidates.Where(c => c.ApplicationStatus == statusFilter);
            }

            return View(await candidates.ToListAsync());
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

            List<Degree?> degreeList = FindDegrees(id);
            ViewData["CandidatesDegreeList"] = degreeList;


            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        private List<Degree?> FindDegrees(Guid? id)
        {
            var degreeIds = _context.CandidateDegree.Where(cd => cd.CandidateId == id).Select(cd => cd.DegreeId).ToList();
            List<Degree?> degreeList = new List<Degree?>();
            foreach (var degreeId in degreeIds)
            {
                degreeList.Add(_context.Degree.Where(d => d.Id == degreeId).FirstOrDefault());
            }

            return degreeList;
        }

        // GET: Candidates/Create
        public async Task<IActionResult> Create()
        {
            var enumData = from ApplicationStatus appStatus in Enum.GetValues(typeof(ApplicationStatus))
                           select new SelectListItem
                           {
                               Value = ((int)appStatus).ToString(),
                               Text = appStatus.ToString()
                           };

            var applicationStatus = enumData.ToList();
            applicationStatus.First().Selected = true;
            ViewData["ApplicationStatus"] = applicationStatus;
            #region degrees View List
            var degreesViewList = (await _context.Degree.ToListAsync()).Select(degree => new SelectListItem
            {
                Value = degree.Id.ToString(),
                Text = degree.Name
            });


            ViewData["degreesViewList"] = degreesViewList;
            #endregion
            return View();
        }

        // POST: Candidates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateId,LastName,FirstName,Email,Mobile,ApplicationStatus,Comments,CreationTime")] Candidate candidate, List<Guid> SelectedDegrees)
        {
            if (candidate.ApplicationStatus != null)
            {
                var test = candidate.ApplicationStatus;
            }


            if (ModelState.IsValid)
            {
                candidate.CandidateId = Guid.NewGuid();
                candidate.CreationTime = DateTime.Now;

                SelectedDegrees.ForEach(id =>
                {

                    try
                    {
                        candidate.Degrees.Add(new CandidateDegree() { CandidateId = candidate.CandidateId, DegreeId = id });

                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                });

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
            ViewData["CandidateStatus"] = candidate.ApplicationStatus;


            var degreesViewList = (await _context.Degree.ToListAsync()).Select(degree => new SelectListItem
            {
                Value = degree.Id.ToString(),
                Text = degree.Name
            });
            ViewData["degreesViewList"] = degreesViewList;
            ViewData["AqcuiredCandidatesDegreeList"] = FindDegrees(id);

            return View(candidate);
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CandidateId,LastName,FirstName,Email,Mobile,ApplicationStatus,Comments,CreationTime")]
        Candidate candidate)
        {
            if (id != candidate.CandidateId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var selectedDegrees = Request.Form["SelectedDegrees"].ToList();
                    _context.CandidateDegree.RemoveRange(_context.CandidateDegree.Where(cd => cd.CandidateId == id).ToList());
                    if (selectedDegrees.Count != 0)
                    {
                        foreach (var item in selectedDegrees)
                        {
                            _context.CandidateDegree.Add(new CandidateDegree() { CandidateId = id, DegreeId = Guid.Parse(item) });
                        }
                    }
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
