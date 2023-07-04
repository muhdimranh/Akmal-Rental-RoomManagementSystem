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
    public class TbPaymentsController : Controller
    {
        private readonly DbRentalContext _context;

        public TbPaymentsController(DbRentalContext context)
        {
            _context = context;
        }

        // GET: TbPayments
        public async Task<IActionResult> Index()
        {
            var sales = await _context.TbPayments.Include(t => t.PTenant).ThenInclude(r => r.TRoom).ThenInclude(t => t.RLocation).ToListAsync();
            return View(sales);
        }

        // GET: TbPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbPayments == null)
            {
                return NotFound();
            }

            var tbPayment = await _context.TbPayments
                .Include(t => t.PTenant)
                .FirstOrDefaultAsync(m => m.PId == id);
            if (tbPayment == null)
            {
                return NotFound();
            }

            return View(tbPayment);
        }

        // GET: TbPayments/Create
        public IActionResult Create()
        {
            ViewData["PTenantid"] = new SelectList(_context.TbTenants, "TId", "TId");
            return View();
        }

        // POST: TbPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PId,PTenantid,PDate,PAmount,PType,PRoomNo")] TbPayment tbPayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PTenantid"] = new SelectList(_context.TbTenants, "TId", "TId", tbPayment.PTenantid);
            return View(tbPayment);
        }

        // GET: TbPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbPayments == null)
            {
                return NotFound();
            }

            var tbPayment = await _context.TbPayments.FindAsync(id);
            if (tbPayment == null)
            {
                return NotFound();
            }
            ViewData["PTenantid"] = new SelectList(_context.TbTenants, "TId", "TId", tbPayment.PTenantid);
            return View(tbPayment);
        }

        // POST: TbPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PId,PTenantid,PDate,PAmount,PType,PRoomNo")] TbPayment tbPayment)
        {
            if (id != tbPayment.PId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbPaymentExists(tbPayment.PId))
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
            ViewData["PTenantid"] = new SelectList(_context.TbTenants, "TId", "TId", tbPayment.PTenantid);
            return View(tbPayment);
        }

        // GET: TbPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbPayments == null)
            {
                return NotFound();
            }

            var tbPayment = await _context.TbPayments
                .Include(t => t.PTenant)
                .FirstOrDefaultAsync(m => m.PId == id);
            if (tbPayment == null)
            {
                return NotFound();
            }

            return View(tbPayment);
        }

        // POST: TbPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbPayments == null)
            {
                return Problem("Entity set 'DbRentalContext.TbPayments'  is null.");
            }
            var tbPayment = await _context.TbPayments.FindAsync(id);
            if (tbPayment != null)
            {
                _context.TbPayments.Remove(tbPayment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbPaymentExists(int id)
        {
            return (_context.TbPayments?.Any(e => e.PId == id)).GetValueOrDefault();
        }
    }
}