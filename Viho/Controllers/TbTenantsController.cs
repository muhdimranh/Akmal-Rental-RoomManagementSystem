using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Viho.web.DataDB;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace Viho.web.Controllers
{
    [Authorize]
    public class TbTenantsController : Controller
    {


        private readonly DbRentalContext _context;
        

        private readonly IWebHostEnvironment _webHostEnvironment;
        public TbTenantsController(DbRentalContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            
            _webHostEnvironment = webHostEnvironment;
        }



        






        // GET: TbTenants
        public async Task<IActionResult> Index()
        {
            var dbRentalContext = _context.TbTenants.Include(t => t.TRoom).ThenInclude(r => r.RLocation);
            return View(await dbRentalContext.ToListAsync());
        }

        // GET: TbTenants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbTenants == null)
            {
                return NotFound();
            }

            var tbTenant = await _context.TbTenants
                .Include(t => t.TRoom)
                .FirstOrDefaultAsync(m => m.TId == id);
            if (tbTenant == null)
            {
                return NotFound();
            }

            return PartialView("_Details", tbTenant);
        }

        public IActionResult GetRoomPrice(int roomNo)
        {
            using (var dbContext = new DbRentalContext())
            {
                var room = dbContext.TbRooms.FirstOrDefault(r => r.RNo == roomNo);
                if (room != null)
                {
                    var roomPrice = room.RPrice;
                    return Json(new { roomPrice });
                }
            }

            return BadRequest("Failed to retrieve room price.");
        }


        public async Task<IActionResult> AddPayment(int tenantId, double amount, double additionalAmount, DateTime date, string type, bool markAsPaid, int roomNo, IFormFile? receipt)

        {
            using (var dbContext = new DbRentalContext())
            {
                // Check if the tenant ID exists
                var tenant = dbContext.TbTenants.Include(t => t.TRoom).ThenInclude(r => r.RLocation).FirstOrDefault(t => t.TId == tenantId);
                if (tenant == null)
                {
                    // Handle the case where the tenant ID is invalid
                    return BadRequest("Invalid tenant ID");
                }

                // Calculate the total payment amount
                var totalAmount = amount + additionalAmount;
                var locationcode = tenant.TRoom?.RLocation?.LCode;
                // Create a new payment record
                var payment = new TbPayment
                {
                    PTenantid = tenantId,
                    PAmount = totalAmount,
                    PDate = date,
                    PType = type,
                    PRoomNo = roomNo, 
                    PLocationCode = locationcode,
                    //PReceipt = receipt
                };
                if (receipt != null && receipt.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Picture/Receipt/");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + receipt.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await receipt.CopyToAsync(fileStream);
                    }

                    // Set the receipt properties
                    payment.PReceipt = uniqueFileName;
                    
                }


                // Save the payment record to the database
                dbContext.TbPayments.Add(payment);
                dbContext.SaveChanges();

                var paymentId = payment.PId;

                var tenantName = tenant.TName;
                var locationCode = tenant.TRoom?.RLocation?.LCode;
               
                var sales = new TbSale
                {
                    SPaymentid = paymentId,
                    SDate = date,
                    SDetail = $"{tenantName} - Room {roomNo}",
                    SCategory = locationCode, // Assigning LCode to SCategory
                    STransactionType = type,
                    SAmountIn = totalAmount,
                    //SReceipt = receipt

                };

                if (receipt != null && receipt.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Picture/Receipt/");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + receipt.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await receipt.CopyToAsync(fileStream);
                    }

                    // Set the receipt properties
                    
                    sales.SReceipt = uniqueFileName;
                }

                _context.TbSales.Add(sales);
                _context.SaveChanges();




                if (markAsPaid)
                {
                    var tenantToUpdate = dbContext.TbTenants.FirstOrDefault(t => t.TId == tenantId);
                    if (tenantToUpdate != null)
                    {
                        tenantToUpdate.TPaymentStatus = "Paid";
                        tenantToUpdate.IsPaymentCollected = true;

                        // Update the reminder date to 28 days after the current reminder date
                        tenant.LastReminderDate = tenant.LastReminderDate.AddDays(28);
                        dbContext.SaveChanges();
                    }
                }
            }

            // Redirect back to the tenant list view
            return RedirectToAction("Index");
        }


        public IActionResult GetPaymentRecords(int tenantId)
        {
            using (var dbContext = new DbRentalContext())
            {
                // Retrieve the payment records for the tenant
                var paymentRecords = dbContext.TbPayments
                    .Where(p => p.PTenantid == tenantId)
                    .ToList();

                // Pass the payment records to a partial view
                return PartialView("_PaymentRecords", paymentRecords);
            }
        }




        // GET: TbTenants/GetRoomsByLocation?locationId={locationId}
        [HttpGet]
        public IActionResult GetRoomsByLocation(int locationId)
        {
            var assignedRooms = _context.TbTenants.Select(t => t.TRoomid).Distinct().ToList();
            var availableRooms = _context.TbRooms
                .Where(r => !assignedRooms.Contains(r.RId) && r.RLocationid == locationId && r.RStatus == "Available")
                .ToList();
            var roomOptions = availableRooms.Select(r => new { r.RId, r.RNo });
            return Json(roomOptions);
        }







        // GET: TbTenants/Create
        public IActionResult Create()
        {
            var assignedRooms = _context.TbTenants.Select(t => t.TRoomid).Distinct().ToList();
            var availableRooms = _context.TbRooms
                .Where(r => !assignedRooms.Contains(r.RId))
                .ToList();
            var locations = _context.TbLocations.ToList();
            ViewBag.Locations = locations;


            ViewBag.TRoomid = new SelectList(Enumerable.Empty<TbRoom>(), "RId", "RNo");

            return View();
        }









        // POST: TbTenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TId,TName,TIc,TPhone,TAddress,TUploadIc,TEmerContact,TRoomid,TEntrydate,TExitdate,TCardCode,TAddOnDetail,TPaymentStatus,TAgreement")] TbTenant tbTenant, IFormFile? TUploadIc, IFormFile? TAgreement)
        {
            if (ModelState.IsValid)
            {

                tbTenant.TRoom = await _context.TbRooms.FindAsync(tbTenant.TRoomid);
                if (TUploadIc != null && TUploadIc.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + TUploadIc.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await TUploadIc.CopyToAsync(fileStream);
                    }

                    tbTenant.TUploadIc = "/images/" + uniqueFileName;
                }

                if (TAgreement != null && TAgreement.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + TAgreement.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await TAgreement.CopyToAsync(fileStream);
                    }

                    tbTenant.TAgreement = "/images/" + uniqueFileName;
                }
                tbTenant.LastReminderDate = tbTenant.TEntrydate.AddDays(28); // Set the initial reminder date as 21 days after entry date
                tbTenant.IsPaymentCollected = false; // Set the initial value for IsPaymentCollected

                // Update room status to "Not Available"
                var room = await _context.TbRooms.FindAsync(tbTenant.TRoomid);
                if (room != null)
                {
                    room.RStatus = "Not Available";
                }


                _context.Add(tbTenant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            string errors = string.Join("; ", ModelState.Values
               .SelectMany(x => x.Errors)
               .Select(x => x.ErrorMessage));
            ModelState.AddModelError("", errors);
            ViewData["TRoomid"] = new SelectList(_context.TbRooms, "RId", "RNo", tbTenant.TRoomid);
            return View(tbTenant);
        }


        // GET: TbTenants/CheckOut/5
        public async Task<IActionResult> CheckOut(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbTenant = await _context.TbTenants.FindAsync(id);
            if (tbTenant == null)
            {
                return NotFound();
            }

            // Update room status to "Available"
            var room = await _context.TbRooms.FindAsync(tbTenant.TRoomid);
            if (room != null)
            {
                room.RStatus = "Available";
            }

            // Detach the tenant from the assigned room
            tbTenant.TRoomid = null; // Assuming 0 represents no room selection
            tbTenant.IsCheckedOut = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id)
        {
            var tbTenant = await _context.TbTenants.FindAsync(id);
            if (tbTenant == null)
            {
                return NotFound();
            }

            // Update room status to "Available"
            var room = await _context.TbRooms.FindAsync(tbTenant.TRoomid);
            if (room != null)
            {
                room.RStatus = "Available";
            }

            // Detach the tenant from the assigned room
            tbTenant.TRoomid = null; // Use 0 to represent no room selection
            tbTenant.IsCheckedOut = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePaymentStatus(int id)
        {
            var tenant = await _context.TbTenants.FindAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }

            tenant.TPaymentStatus = "Paid"; // Update the payment status to "Paid"
            //tenant.LastReminderDate = tenant.LastReminderDate.Value.AddDays(28); // Update the reminder date to 28 days after the existing reminder date
            tenant.IsPaymentCollected = true; // Set IsPaymentCollected to true

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        // GET: TbTenants/Edit/5
        public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var tbTenant = await _context.TbTenants
        .Include(t => t.TRoom)
        .ThenInclude(r => r.RLocation)
        .FirstOrDefaultAsync(t => t.TId == id);

    if (tbTenant == null)
    {
        return NotFound();
    }

    var assignedRooms = _context.TbTenants
        .Where(t => t.TId != tbTenant.TId)
        .Select(t => t.TRoomid)
        .Distinct()
        .ToList();

            var availableRooms = _context.TbRooms
            .Where(r => (r.RLocationid == (tbTenant.TRoom != null ? tbTenant.TRoom.RLocationid : null)
                       || r.RId == tbTenant.TRoomid || r.RId == tbTenant.TRoomid))
            .ToList();


            ViewBag.TRoomid = new SelectList(availableRooms, "RId", "RNo", tbTenant.TRoomid);

            var locations = _context.TbLocations.ToList();
            ViewBag.Locations = locations;


            ViewBag.CurrentLocationId = tbTenant.TRoom?.RLocationid;

    return View(tbTenant);
}












        // POST: TbTenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TId,TName,TIc,TPhone,TAddress,TUploadIc,TEmerContact,TRoomid,TEntrydate,TExitdate,TCardCode,TAddOnDetail,TPaymentStatus,TAgreement")] TbTenant tbTenant, IFormFile? TUploadIc, IFormFile? TAgreement)
        {
            if (id != tbTenant.TId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                // Get the previous room assignment
                var previousTenant = await _context.TbTenants.AsNoTracking().FirstOrDefaultAsync(t => t.TId == id);
                var previousRoomId = previousTenant.TRoomid;

                if (tbTenant.TRoomid != 0 && tbTenant.TRoomid != previousRoomId)
                {
                    // Update the previous room status to "Available"
                    var previousRoom = await _context.TbRooms.FindAsync(previousRoomId);
                    if (previousRoom != null)
                    {
                        previousRoom.RStatus = "Available";
                    }

                    // Update the new assigned room status to "Not Available"
                    var assignedRoom = await _context.TbRooms.FindAsync(tbTenant.TRoomid);
                    if (assignedRoom != null)
                    {
                        assignedRoom.RStatus = "Not Available";
                    }
                }



                if (tbTenant.TRoomid == null)
                {
                    tbTenant.IsCheckedOut = true;
                }
                else
                {
                    tbTenant.IsCheckedOut = false;
                }

                if (TUploadIc != null && TUploadIc.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + TUploadIc.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await TUploadIc.CopyToAsync(fileStream);
                    }

                    tbTenant.TUploadIc = "/images/" + uniqueFileName;
                }
                else
                {
                    // No new file uploaded, retain the existing value
                    tbTenant.TUploadIc = _context.TbTenants.AsNoTracking().FirstOrDefault(t => t.TId == id)?.TUploadIc;
                }



                if (TAgreement != null && TAgreement.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + TAgreement.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await TAgreement.CopyToAsync(fileStream);
                    }

                    tbTenant.TAgreement = "/images/" + uniqueFileName;
                }
                else
                {
                    // No new file uploaded, retain the existing value
                    tbTenant.TAgreement = _context.TbTenants.AsNoTracking().FirstOrDefault(t => t.TId == id)?.TAgreement;
                }

                try
                {


                    tbTenant.LastReminderDate = tbTenant.TEntrydate.AddDays(21); // Set the initial reminder date as 21 days after entry date

                    // Check if TPaymentStatus is "Paid"
                    if (tbTenant.TPaymentStatus == "Paid")
                    {
                        tbTenant.IsPaymentCollected = true;
                    }
                    else
                    {
                        tbTenant.IsPaymentCollected = false;
                    }

                    // Update room status if it has changed
                    if (tbTenant.TRoomid != null)
                    {
                        var room = await _context.TbRooms.FindAsync(tbTenant.TRoomid);
                        if (room != null && room.RId != tbTenant.TRoomid)
                        {
                            // Current room and assigned room are different, update their statuses
                            room.RStatus = "Available";
                            var assignedRoom = await _context.TbRooms.FindAsync(tbTenant.TRoomid);
                            if (assignedRoom != null)
                            {
                                assignedRoom.RStatus = "Not Available";
                            }
                        }
                    }

                    _context.Update(tbTenant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbTenantExists(tbTenant.TId))
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

            string errors = string.Join("; ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
            ModelState.AddModelError("", errors);
            ViewData["TRoomid"] = new SelectList(_context.TbRooms, "RId", "RNo", tbTenant.TRoomid);
            return View(tbTenant);
        }


        // GET: TbTenants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbTenants == null)
            {
                return NotFound();
            }

            var tbTenant = await _context.TbTenants
                .Include(t => t.TRoom)
                .FirstOrDefaultAsync(m => m.TId == id);
            if (tbTenant == null)
            {
                return NotFound();
            }

            return View(tbTenant);
        }

        // POST: TbTenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbTenants == null)
            {
                return Problem("Entity set 'DbRentalContext.TbTenants'  is null.");
            }
            var tbTenant = await _context.TbTenants.FindAsync(id);
            if (tbTenant != null)
            {
                _context.TbTenants.Remove(tbTenant);
            }

            if (tbTenant.TRoomid != 0)
            {
                // Update room status to "Available" for the assigned room
                var assignedRoom = await _context.TbRooms.FindAsync(tbTenant.TRoomid);
                if (assignedRoom != null)
                {
                    assignedRoom.RStatus = "Available";
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbTenantExists(int id)
        {
            return (_context.TbTenants?.Any(e => e.TId == id)).GetValueOrDefault();
        }
    }
}
