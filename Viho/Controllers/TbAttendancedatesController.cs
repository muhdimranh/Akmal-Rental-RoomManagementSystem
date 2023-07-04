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
    public class TbAttendancedatesController : Controller
    {
        private readonly DbRentalContext _context;

        public TbAttendancedatesController(DbRentalContext context)
        {
            _context = context;
        }

        // GET: TbAttendancedates
        public async Task<IActionResult> Index()
        {
            var dbRentalContext = _context.TbAttendancedates.Include(t => t.Att);
            return View(await dbRentalContext.ToListAsync());
        }

        // GET: TbAttendancedates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbAttendancedates == null)
            {
                return NotFound();
            }

            var tbAttendancedate = await _context.TbAttendancedates
                .Include(t => t.Att)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbAttendancedate == null)
            {
                return NotFound();
            }

            return View(tbAttendancedate);
        }

        // GET: TbAttendancedates/Create
        public IActionResult Create()
        {
            ViewData["Attid"] = new SelectList(_context.TbAttendanceCleaners, "AttId", "AttId");
            return View();
        }

        // POST: TbAttendancedates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Attid,Attdate")] TbAttendancedate tbAttendancedate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbAttendancedate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Attid"] = new SelectList(_context.TbAttendanceCleaners, "AttId", "AttId", tbAttendancedate.Attid);
            return View(tbAttendancedate);
        }

        // GET: TbAttendancedates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbAttendancedates == null)
            {
                return NotFound();
            }

            var tbAttendancedate = await _context.TbAttendancedates.FindAsync(id);
            if (tbAttendancedate == null)
            {
                return NotFound();
            }
            ViewData["Attid"] = new SelectList(_context.TbAttendanceCleaners, "AttId", "AttId", tbAttendancedate.Attid);
            return PartialView("Edit", tbAttendancedate);
        }

        // POST: TbAttendancedates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Attid,Attdate")] TbAttendancedate tbAttendancedate)
        {
            if (id != tbAttendancedate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbAttendancedate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbAttendancedateExists(tbAttendancedate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "TbAttendancesCleaner");
            }
            ViewData["Attid"] = new SelectList(_context.TbAttendanceCleaners, "AttId", "AttId", tbAttendancedate.Attid);
           
            return PartialView("Edit", tbAttendancedate);
        }

        // GET: TbAttendancedates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbAttendancedates == null)
            {
                return NotFound();
            }

            var tbAttendancedate = await _context.TbAttendancedates
                .Include(t => t.Att)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbAttendancedate == null)
            {
                return NotFound();
            }

            return View(tbAttendancedate);
        }

        // POST: TbAttendancedates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbAttendancedates == null)
            {
                return Problem("Entity set 'DbRentalContext.TbAttendancedates'  is null.");
            }
            var tbAttendancedate = await _context.TbAttendancedates.FindAsync(id);
            if (tbAttendancedate != null)
            {
                _context.TbAttendancedates.Remove(tbAttendancedate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbAttendancedateExists(int id)
        {
          return (_context.TbAttendancedates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
