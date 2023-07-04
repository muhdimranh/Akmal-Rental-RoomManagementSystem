using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Viho.web.DataDB;

namespace Viho.web.Controllers
{
    public class TbLocationsController : Controller
    {
        private readonly DbRentalContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TbLocationsController(DbRentalContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: TbLocations
        public async Task<IActionResult> Index()
        {
              return _context.TbLocations != null ? 
                          View(await _context.TbLocations.ToListAsync()) :
                          Problem("Entity set 'DbRentalContext.TbLocations'  is null.");
        }

        // GET: TbLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbLocations == null)
            {
                return NotFound();
            }

            var tbLocation = await _context.TbLocations
                .FirstOrDefaultAsync(m => m.LId == id);
            if (tbLocation == null)
            {
                return NotFound();
            }

            return PartialView("Details", tbLocation);
        }


        // GET: TbLocations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LId,LCode,LAddress,LWifi,LModemIp,LCctv,LImglayout1,LImgbuilding1,LReminderDate")] TbLocation tbLocation, IFormFile? LImglayout1)
        {
            if (ModelState.IsValid)
            {
                if (LImglayout1 != null && LImglayout1.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Picture/Layouts/");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + LImglayout1.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await LImglayout1.CopyToAsync(fileStream);
                    }

                    tbLocation.LImglayout1 = uniqueFileName;
                }
                tbLocation.IsPaymentMade = false; // Set the initial value for IsPaymentMade

                _context.Add(tbLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbLocation);
        }

        // GET: TbLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbLocations == null)
            {
                return NotFound();
            }

            var tbLocation = await _context.TbLocations.FindAsync(id);
            if (tbLocation == null)
            {
                return NotFound();
            }
            return View(tbLocation);
        }

        // POST: TbLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LId,LCode,LAddress,LWifi,LModemIp,LCctv,LImglayout1,LImgbuilding1,LReminderDate")] TbLocation tbLocation, IFormFile? LImglayout1)
        {
            if (id != tbLocation.LId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (LImglayout1 != null && LImglayout1.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Picture/Layouts/");

                        if (!string.IsNullOrEmpty(tbLocation.LImglayout1))
                        {
                            string previousFilePath = Path.Combine(uploadsFolder, tbLocation.LImglayout1);
                            if (System.IO.File.Exists(previousFilePath))
                            {
                                System.IO.File.Delete(previousFilePath);
                            }
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + LImglayout1.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await LImglayout1.CopyToAsync(fileStream);
                        }

                        // Update the image property
                        tbLocation.LImglayout1 = uniqueFileName;
                    }
                    else
                    {
                        // No new file uploaded, retain the existing value
                        tbLocation.LImglayout1 = _context.TbLocations.AsNoTracking().FirstOrDefault(t => t.LId == id)?.LImglayout1;
                    }

                    _context.Update(tbLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbLocationExists(tbLocation.LId))
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
            return View(tbLocation);
        }

        // GET: TbLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbLocations == null)
            {
                return NotFound();
            }

            var tbLocation = await _context.TbLocations
                .FirstOrDefaultAsync(m => m.LId == id);
            if (tbLocation == null)
            {
                return NotFound();
            }

            // Check if there are associated rooms
            var associatedRooms = await _context.TbRooms.AnyAsync(r => r.RLocationid == tbLocation.LId);
            if (associatedRooms)
            {
                ViewBag.HasAssociatedRooms = true; // Add this line to pass the information to the view
            }
            else
            {
                ViewBag.HasAssociatedRooms = false; // Add this line to pass the information to the view
            }

            return View(tbLocation);
        }

        // POST: TbLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbLocation = await _context.TbLocations.FindAsync(id);

            // Check if the location exists
            if (tbLocation == null)
            {
                return NotFound();
            }

            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(tbLocation.LImglayout1))
            {
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Picture/Layouts", tbLocation.LImglayout1);

                // Check if the image file exists before deleting
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.TbLocations.Remove(tbLocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbLocationExists(int id)
        {
          return (_context.TbLocations?.Any(e => e.LId == id)).GetValueOrDefault();
        }
    }
}
