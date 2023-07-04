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
    public class TbLotsController : Controller
    {
        private readonly DbRentalContext _context;

        public TbLotsController(DbRentalContext context)
        {
            _context = context;
        }

        // GET: TbLots
        public async Task<IActionResult> Index()
        {
            var lots = await _context.TbLots.ToListAsync();

            foreach (var lot in lots)
            {
                var investorCount = _context.TbInvestors.Count(i => i.ILot == lot.LotId);
                var investmentSum = _context.TbInvestors.Where(i => i.ILot == lot.LotId).Sum(i => i.IInvestment);

                if (investorCount == 3 || investmentSum >= 50000)
                {
                    lot.LotStatus = "Full";
                }
                else
                {
                    lot.LotStatus = "Free";
                }
            }

            await _context.SaveChangesAsync();

            return View(lots);
        }

        // GET: TbLots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbLots == null)
            {
                return NotFound();
            }

            var tbLot = await _context.TbLots
                .FirstOrDefaultAsync(m => m.LotId == id);
            if (tbLot == null)
            {
                return NotFound();
            }

            return PartialView("Details", tbLot);
        }

        // GET: TbLots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbLots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LotId,LotName,LotStatus")] TbLot tbLot)
        {
            if (ModelState.IsValid)
            {
                tbLot.LotStatus = "Free";

                _context.Add(tbLot);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(tbLot);
        }

        // GET: TbLots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbLots == null)
            {
                return NotFound();
            }

            var tbLot = await _context.TbLots.FindAsync(id);
            if (tbLot == null)
            {
                return NotFound();
            }
            return View(tbLot);
        }

        // POST: TbLots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LotId,LotName,LotStatus")] TbLot tbLot)
        {
            if (id != tbLot.LotId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbLot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbLotExists(tbLot.LotId))
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
            return View(tbLot);
        }

        // GET: TbLots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbLots == null)
            {
                return NotFound();
            }

            var tbLot = await _context.TbLots
                .Include(lot => lot.TbInvestors)
                .FirstOrDefaultAsync(m => m.LotId == id);

            if (tbLot == null)
            {
                return NotFound();
            }

            if (tbLot.TbInvestors.Count > 0)
            {
                TempData["WarningMessage"] = "Cannot delete lot with investors.";
                return RedirectToAction("Index"); // Redirect to the index page or any other appropriate page
            }

            return View(tbLot);
        }


        // POST: TbLots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbLots == null)
            {
                return Problem("Entity set 'DbRentalContext.TbLots'  is null.");
            }
            var tbLot = await _context.TbLots.FindAsync(id);
            if (tbLot != null)
            {
                _context.TbLots.Remove(tbLot);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbLotExists(int id)
        {
          return (_context.TbLots?.Any(e => e.LotId == id)).GetValueOrDefault();
        }
    }
}
