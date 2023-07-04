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
    public class TbProfitsController : Controller
    {
        private readonly DbRentalContext _context;

        public TbProfitsController(DbRentalContext context)
        {
            _context = context;
        }

        // GET: TbProfits
        public async Task<IActionResult> Index()
        {
              return _context.TbProfits != null ? 
                          View(await _context.TbProfits.ToListAsync()) :
                          Problem("Entity set 'DbRentalContext.TbProfits'  is null.");
        }

        // GET: TbProfits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbProfits == null)
            {
                return NotFound();
            }

            var tbProfit = await _context.TbProfits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbProfit == null)
            {
                return NotFound();
            }

            return View(tbProfit);
        }

        // GET: TbProfits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbProfits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Year,Month,Category,TotalSales,ProfitLoss")] TbProfit tbProfit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbProfit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbProfit);
        }

        // GET: TbProfits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbProfits == null)
            {
                return NotFound();
            }

            var tbProfit = await _context.TbProfits.FindAsync(id);
            if (tbProfit == null)
            {
                return NotFound();
            }
            return View(tbProfit);
        }

        // POST: TbProfits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Year,Month,Category,TotalSales,ProfitLoss")] TbProfit tbProfit)
        {
            if (id != tbProfit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbProfit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbProfitExists(tbProfit.Id))
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
            return View(tbProfit);
        }

        // GET: TbProfits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbProfits == null)
            {
                return NotFound();
            }

            var tbProfit = await _context.TbProfits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbProfit == null)
            {
                return NotFound();
            }

            return View(tbProfit);
        }

        // POST: TbProfits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbProfits == null)
            {
                return Problem("Entity set 'DbRentalContext.TbProfits'  is null.");
            }
            var tbProfit = await _context.TbProfits.FindAsync(id);
            if (tbProfit != null)
            {
                _context.TbProfits.Remove(tbProfit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbProfitExists(int id)
        {
          return (_context.TbProfits?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
