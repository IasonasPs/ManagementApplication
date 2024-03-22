﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManagementApplication.Data;
using ManagementApplication.Models;
using NuGet.Packaging;

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

            var t = _context.Candidate.Where(c => c.CandidateId == candidate.CandidateId).FirstOrDefault();

            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
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

            ViewData["ApplicationStatus"] = enumData;

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
        public async Task<IActionResult> Create([Bind("CandidateId,LastName,FirstName,Email,Mobile,Degrees,ApplicationStatus,Comments,CreationTime")] Candidate candidate,List<Guid> SelectedDegrees)
        {
            if (ModelState.IsValid)
            {
                candidate.CandidateId = Guid.NewGuid();
                candidate.CreationTime = DateTime.Now;
                SelectedDegrees.ForEach( id =>
                {
                    var test = _context.Degree.Where(d => d.Id == id).FirstOrDefault();

                    //candidate.Degrees.Add(_context.Degree.Where(degree => degree.Id == id).FirstOrDefault());
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

        #region CandidatesDegrees
        // GET: Candidates/CandidateDegrees/5
        public async Task<IActionResult> CandidateDegrees(Guid Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var candidate = await _context.Candidate.FindAsync(Id);
            if (candidate == null)
            {
                return NotFound();
            }

            var degreesViewList = (await _context.Degree.ToListAsync()).Select(degree => new SelectListItem
            {
                Value = degree.Id.ToString(),
                Text = degree.Name
            });

            ViewData["degreesViewList"] = degreesViewList;
            return View(candidate);
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CandidateDegrees(Candidate candidate)
        {
            //if (id != candidate.CandidateId)
            //{
            //    return NotFound();
            //}

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
                return RedirectToAction(nameof(CandidateDegrees));
            }
            return View(candidate);
        }

        #endregion
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
