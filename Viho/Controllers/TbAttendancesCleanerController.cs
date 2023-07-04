using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Viho.web.DataDB;

namespace Viho.web.Controllers
{
    public class TbAttendancesCleanerController : Controller
    {
        private readonly DbRentalContext _context;

        public TbAttendancesCleanerController(DbRentalContext context)
        {
            _context = context;
        }




        // GET: TbAttendances
        public async Task<IActionResult> Index()
        {
            int previousMonth = TempData["PreviousMonth"] != null ? (int)TempData["PreviousMonth"] : DateTime.Now.Month;

            var tbAttendanceCleaners = await _context.TbAttendanceCleaners
                .Include(t => t.AttL)
                .ToListAsync();

            // Retrieve the LCode values for TbLocations
            var lCodeDictionary = await _context.TbLocations.ToDictionaryAsync(l => l.LId, l => l.LCode);

            // Map the LCode values to the corresponding AttLid in TbAttendanceCleaners
            foreach (var attendanceCleaner in tbAttendanceCleaners)
            {
                if (lCodeDictionary.TryGetValue(attendanceCleaner.AttLid, out var lCode))
                {
                    attendanceCleaner.AttL.LCode = lCode;
                }
            }

            TempData["PreviousMonth"] = DateTime.Now.Month;

            ViewData["PreviousMonth"] = previousMonth;
            return View(tbAttendanceCleaners);
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAttendance = await _context.TbAttendanceCleaners
                .Include(t => t.AttL)
                .Include(t => t.TbAttendancedates) // Include TbAttendancedates
                .FirstOrDefaultAsync(m => m.AttId == id);

            if (tbAttendance == null)
            {
                return NotFound();
            }

            return PartialView("Details", tbAttendance);
        }


        // GET: TbAttendances/Create
        public IActionResult Create()
        {
            ViewData["AttLid"] = new SelectList(_context.TbLocations, "LId", "LCode");
            return View();
        }

        // POST: TbAttendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttId,AttLid,AttCount")] TbAttendanceCleaner tbAttendanceCleaner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbAttendanceCleaner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttLid"] = new SelectList(_context.TbLocations, "LId", "LCode", tbAttendanceCleaner.AttLid);
            return View(tbAttendanceCleaner);
        }


        // GET: TbAttendanceCleaner2/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbAttendanceCleaners == null)
            {
                return NotFound();
            }

            var tbAttendanceCleaner = await _context.TbAttendanceCleaners.FindAsync(id);
            if (tbAttendanceCleaner == null)
            {
                return NotFound();
            }

            ViewData["AttLid"] = new SelectList(_context.TbLocations, "LId", "LCode");
            return PartialView("Edit", tbAttendanceCleaner);
        }

        // POST: TbAttendanceCleaner2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("AttId,AttLid,AttCount,TbAttendancedates")] TbAttendanceCleaner tbAttendanceCleaner, DateTime? NewAttendanceDate)
        {
            if (id != tbAttendanceCleaner.AttId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update existing attendance dates
                    _context.Update(tbAttendanceCleaner);

                    // Add new attendance date if provided
                    if (NewAttendanceDate.HasValue)
                    {
                        var newAttendanceDate = new TbAttendancedate
                        {
                            Attid = tbAttendanceCleaner.AttId,
                            Attdate = NewAttendanceDate.Value
                        };
                        _context.Add(newAttendanceDate);
                    }

                    // Update the attendance count
                    var attendance = await _context.TbAttendanceCleaners.FindAsync(id);
                    attendance.AttCount = tbAttendanceCleaner.AttCount;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbAttendanceCleanerExists(tbAttendanceCleaner.AttId))
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

            ViewData["AttLid"] = new SelectList(_context.TbLocations, "LId", "LCode", tbAttendanceCleaner.AttLid);
            return PartialView("Edit", tbAttendanceCleaner);
        }









        // GET: TbAttendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbAttendanceCleaners == null)
            {
                return NotFound();
            }

            var tbAttendance = await _context.TbAttendanceCleaners
                .Include(t => t.AttL)
                .Include(t => t.TbAttendancedates) // Include TbAttendancedates
                .FirstOrDefaultAsync(m => m.AttId == id);

            if (tbAttendance == null)
            {
                return NotFound();
            }

            return View(tbAttendance);
        }


        // POST: TbAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbAttendanceCleaners == null)
            {
                return Problem("Entity set 'DbRentalContext.TbAttendances'  is null.");
            }
            var tbAttendance = await _context.TbAttendanceCleaners.FindAsync(id);
            if (tbAttendance != null)
            {
                _context.TbAttendanceCleaners.Remove(tbAttendance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbAttendanceCleanerExists(int id)
        {
            return (_context.TbAttendanceCleaners?.Any(e => e.AttId == id)).GetValueOrDefault();
        }


       


    }
}
