using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Viho.web.DataDB;
using Microsoft.AspNetCore.Hosting;
using System.IO;


namespace Viho.web.Controllers
{
    public class TbRoomsController : Controller
    {
        private readonly DbRentalContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;



        public TbRoomsController(DbRentalContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: TbRooms
        public async Task<IActionResult> Index()
        {
            var dbRentalContext = _context.TbRooms.Include(t => t.RLocation);
            var locations = await _context.TbLocations.ToListAsync();

            // Retrieve additional fields for each location based on Lid
            var locationDetails = locations.Select(location => new
            {
                location.LId,
                location.LAddress,
                location.LImglayout1,
                location.LCode
            }).ToList();

            ViewBag.Locations = locations; // Set the ViewBag property with the locations
            ViewBag.LocationDetails = locationDetails; // Set the ViewBag property with the location details

            return View(await dbRentalContext.ToListAsync());
        }


        // GET: TbRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbRooms == null)
            {
                return NotFound();
            }

            var tbRoom = await _context.TbRooms
                .Include(t => t.RLocation)
                .FirstOrDefaultAsync(m => m.RId == id);
            if (tbRoom == null)
            {
                return NotFound();
            }

            return PartialView("_Details", tbRoom);
        }

        // GET: TbRooms/Create
        public IActionResult Create()
        {
            ViewData["RLocationid"] = new SelectList(_context.TbLocations, "LId", "LAddress");
            return View();
        }

        // POST: TbRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RId,RNo,RLocationid,RType,RPrice,RDepositAmount,RDesc,RStatus,RImg1,RImg2")] TbRoom tbRoom, IFormFile? RImg1)
        {
            if (ModelState.IsValid)
            {
                if (RImg1 != null && RImg1.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Picture/Rooms/");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + RImg1.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await RImg1.CopyToAsync(fileStream);
                    }

                    tbRoom.RImg1 = uniqueFileName;
                }

                _context.Add(tbRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RLocationid"] = new SelectList(_context.TbLocations, "LId", "LAddress", tbRoom.RLocationid);
            return View(tbRoom);
        }



        // GET: TbRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbRooms == null)
            {
                return NotFound();
            }

            var tbRoom = await _context.TbRooms.FindAsync(id);
            if (tbRoom == null)
            {
                return NotFound();
            }
            ViewData["RLocationid"] = new SelectList(_context.TbLocations, "LId", "LAddress", tbRoom.RLocationid);
            return View(tbRoom);
        }

        // POST: TbRooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RId,RNo,RLocationid,RType,RPrice,RDepositAmount,RDesc,RStatus,RImg1,RImg2")] TbRoom tbRoom, IFormFile? RImg1)
        {
            if (id != tbRoom.RId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (RImg1 != null && RImg1.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Picture/Rooms/");

                        if (!string.IsNullOrEmpty(tbRoom.RImg1))
                        {
                            string previousFilePath = Path.Combine(uploadsFolder, tbRoom.RImg1);
                            if (System.IO.File.Exists(previousFilePath))
                            {
                                System.IO.File.Delete(previousFilePath);
                            }
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + RImg1.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await RImg1.CopyToAsync(fileStream);
                        }

                        // Update the image property
                        tbRoom.RImg1 = uniqueFileName;
                    }
                    else
                    {
                        // No new file uploaded, retain the existing value
                        tbRoom.RImg1 = _context.TbRooms.AsNoTracking().FirstOrDefault(t => t.RId == id)?.RImg1;
                    }

                    _context.Update(tbRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbRoomExists(tbRoom.RId))
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
            ViewData["RLocationid"] = new SelectList(_context.TbLocations, "LId", "LAddress", tbRoom.RLocationid);
            return View(tbRoom);
        }


        // GET: TbRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbRooms == null)
            {
                return NotFound();
            }

            var tbRoom = await _context.TbRooms
                .Include(t => t.RLocation)
                .FirstOrDefaultAsync(m => m.RId == id);
            if (tbRoom == null)
            {
                return NotFound();
            }

            return View(tbRoom);
        }

        // POST: TbRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbRoom = await _context.TbRooms.FindAsync(id);

            // Check if the room exists
            if (tbRoom == null)
            {
                return NotFound();
            }

            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(tbRoom.RImg1))
            {
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Picture/Rooms", tbRoom.RImg1);

                // Check if the image file exists before deleting
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.TbRooms.Remove(tbRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbRoomExists(int id)
        {
            return (_context.TbRooms?.Any(e => e.RId == id)).GetValueOrDefault();
        }
    }
}