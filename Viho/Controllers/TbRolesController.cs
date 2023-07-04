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
    public class TbRolesController : Controller
    {
        private readonly DbRentalContext _context;

        public TbRolesController(DbRentalContext context)
        {
            _context = context;
        }

        // GET: TbRoles
        public async Task<IActionResult> Index()
        {
              return _context.TbRoles != null ? 
                          View(await _context.TbRoles.ToListAsync()) :
                          Problem("Entity set 'DbRentalContext.TbRoles'  is null.");
        }

        // GET: TbRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbRoles == null)
            {
                return NotFound();
            }

            var tbRole = await _context.TbRoles
                .FirstOrDefaultAsync(m => m.RlId == id);
            if (tbRole == null)
            {
                return NotFound();
            }

            return View(tbRole);
        }

        // GET: TbRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RlId,RlDesc")] TbRole tbRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbRole);
        }

        // GET: TbRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbRoles == null)
            {
                return NotFound();
            }

            var tbRole = await _context.TbRoles.FindAsync(id);
            if (tbRole == null)
            {
                return NotFound();
            }
            return View(tbRole);
        }

        // POST: TbRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RlId,RlDesc")] TbRole tbRole)
        {
            if (id != tbRole.RlId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbRoleExists(tbRole.RlId))
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
            return View(tbRole);
        }

        // GET: TbRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbRoles == null)
            {
                return NotFound();
            }

            var tbRole = await _context.TbRoles
                .FirstOrDefaultAsync(m => m.RlId == id);
            if (tbRole == null)
            {
                return NotFound();
            }

            return View(tbRole);
        }

        // POST: TbRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbRoles == null)
            {
                return Problem("Entity set 'DbRentalContext.TbRoles'  is null.");
            }
            var tbRole = await _context.TbRoles.FindAsync(id);
            if (tbRole != null)
            {
                _context.TbRoles.Remove(tbRole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbRoleExists(int id)
        {
          return (_context.TbRoles?.Any(e => e.RlId == id)).GetValueOrDefault();
        }
    }
}
