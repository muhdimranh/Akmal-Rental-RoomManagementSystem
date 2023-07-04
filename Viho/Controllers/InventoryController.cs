using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Viho.web.DataDB;

namespace AKMAL_RENTAL.Controllers
{
    public class InventoryController : Controller
    {
        private readonly DbRentalContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InventoryController(DbRentalContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: TbInventory
        public async Task<IActionResult> Index()
        {
            var inventories = await _context.TbInventories
                .Include(i => i.ILocation)
                .ToListAsync();

            return View(inventories);
        }


        // GET: TbInventory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbInventories == null)
            {
                return NotFound();
            }

            var tbInventory = await _context.TbInventories
                .Include(i => i.ILocation)
                .FirstOrDefaultAsync(m => m.IId == id);
            if (tbInventory == null)
            {
                return NotFound();
            }

            return PartialView("Details", tbInventory);
        }

        // GET: TbInventory/Create
        public IActionResult Create()
        {
            ViewData["ILocationid"] = new SelectList(_context.TbLocations, "LId", "LCode");
            return View();
        }

        // POST: TbInventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IId,ILocationid,IItem,IQuantity")] TbInventory tbInventory)
        {
            if (ModelState.IsValid)
            {
                if (tbInventory.IQuantity < 0)
                {
                    ModelState.AddModelError("IQuantity", "Quantity cannot be below 0.");
                    ViewData["ILocationid"] = new SelectList(_context.TbLocations, "LId", "LCode", tbInventory.ILocationid);
                    return View(tbInventory);
                }

                _context.Add(tbInventory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ILocationid"] = new SelectList(_context.TbLocations, "LId", "LCode", tbInventory.ILocationid);
            return View(tbInventory);
        }

        // GET: TbInventory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbInventories == null)
            {
                return NotFound();
            }

            var tbInventory = await _context.TbInventories.FindAsync(id);
            if (tbInventory == null)
            {
                return NotFound();
            }
            ViewData["ILocationid"] = new SelectList(_context.TbLocations, "LId", "LCode", tbInventory.ILocationid);
            return View(tbInventory);
        }

        // POST: TbInventory/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IId,ILocationid,IItem,IQuantity")] TbInventory tbInventory)
        {
            if (id != tbInventory.IId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (tbInventory.IQuantity < 0)
                {
                    ModelState.AddModelError("IQuantity", "Quantity cannot be below 0.");
                    ViewData["ILocationid"] = new SelectList(_context.TbLocations, "LId", "LCode", tbInventory.ILocationid);
                    return View(tbInventory);
                }

                try
                {
                    _context.Update(tbInventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbInventoryExists(tbInventory.IId))
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
            ViewData["ILocationid"] = new SelectList(_context.TbLocations, "LId", "LCode", tbInventory.ILocationid);
            return View(tbInventory);
        }

        // GET: TbInventory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbInventories == null)
            {
                return NotFound();
            }

            var tbInventory = await _context.TbInventories
                .Include(i => i.ILocation)
                .FirstOrDefaultAsync(m => m.IId == id);
            if (tbInventory == null)
            {
                return NotFound();
            }

            return View(tbInventory);
        }

        // POST: TbInventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbInventory = await _context.TbInventories.FindAsync(id);
            if (tbInventory == null)
            {
                return NotFound();
            }

            _context.TbInventories.Remove(tbInventory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbInventoryExists(int id)
        {
            return (_context.TbInventories?.Any(e => e.IId == id)).GetValueOrDefault();
        }
    }
}
