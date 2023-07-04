using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Viho.web.DataDB;

namespace Viho.web.Controllers
{
    public class TbInvestorsController : Controller
    {
        private readonly DbRentalContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TbInvestorsController(DbRentalContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;

            _webHostEnvironment = webHostEnvironment;
        }

        // GET: TbInvestors
        public async Task<IActionResult> Index()
        {
            var investors = await _context.TbInvestors
                                            .Include(t => t.IIdNavigation)
                                            .Include(t => t.ILotNavigation)
                                            .ToListAsync();

            return View(investors);
        }

        // GET: TbInvestors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbInvestors == null)
            {
                return NotFound();
            }

            var tbInvestor = await _context.TbInvestors
                .Include(t => t.IIdNavigation)
                .Include(t => t.ILotNavigation)
                .FirstOrDefaultAsync(m => m.IId == id);
            if (tbInvestor == null)
            {
                return NotFound();
            }

            return  PartialView("Details",tbInvestor);
        }

        // GET: TbInvestors/Create
        public IActionResult Create()
        {
            var lots = _context.TbLots.ToList();
            var investorsByLot = _context.TbInvestors
                                    .GroupBy(i => i.ILot)
                                    .ToDictionary(g => g.Key, g => g.Count());

            var availableLots = lots.Where(lot => (!investorsByLot.ContainsKey(lot.LotId) || investorsByLot[lot.LotId] < 3) &&
                                          GetTotalInvestment(lot.LotId) < 50000)
                                .Select(lot => new SelectListItem
                                {
                                    Value = lot.LotId.ToString(),
                                    Text = lot.LotName.ToString()
                                })
                                .ToList();

            ViewData["IId"] = new SelectList(_context.TbUsers, "UId", "UUsername");
            ViewData["ILot"] = new SelectList(availableLots, "Value", "Text");
            return View();

        }

        private double GetTotalInvestment(int lotId)
        {
            return _context.TbInvestors
                .Where(i => i.ILot == lotId)
                .Sum(i => i.IInvestment);
        }

        // POST: TbInvestors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IId,IInvestment,IPercent,ILot")] TbInvestor tbInvestor, string type, IFormFile? receipt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    tbInvestor.IPercent = tbInvestor.IInvestment / 50000 * 100;
                    _context.Add(tbInvestor);

                    var investorName = _context.TbUsers
                                .Where(u => u.UId == tbInvestor.IId)
                                .Select(u => u.UUsername)
                                .FirstOrDefault();

                    var lotNumber = _context.TbLots
                                    .Where(l => l.LotId == tbInvestor.ILot)
                                    .Select(l => l.LotName)
                                    .FirstOrDefault();

                    var sale = new TbSale
                    {
                        SDate = DateTime.Now,  // Use current timestamp
                        SCategory = "General",
                        STransactionType = type,
                        SAmountIn = tbInvestor.IInvestment,
                        SDetail = $"Investment by {investorName} - {lotNumber}"
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
                        sale.SReceipt = uniqueFileName;

                    }

                    _context.TbSales.Add(sale);

                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "User already an Investor: " + ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }
            var lots = _context.TbLots.ToList();
            var investorsByLot = _context.TbInvestors
                                    .GroupBy(i => i.ILot)
                                    .ToDictionary(g => g.Key, g => g.Count());

            var availableLots = lots.Where(lot => (!investorsByLot.ContainsKey(lot.LotId) || investorsByLot[lot.LotId] < 3) &&
                                          GetTotalInvestment(lot.LotId) < 50000)
                                .Select(lot => new SelectListItem
                                {
                                    Value = lot.LotId.ToString(),
                                    Text = lot.LotName.ToString()
                                })
                                .ToList();

            ViewData["IId"] = new SelectList(_context.TbUsers, "UId", "UUsername", tbInvestor.IId);
            //ViewData["ILot"] = new SelectList(availableLots, "Value", "Text");
            ViewData["ILot"] = new SelectList(_context.TbLots, "LotId", "LotName", tbInvestor.ILot);
            return View(tbInvestor);
        }







        // GET: TbInvestors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbInvestors == null)
            {
                return NotFound();
            }

            var tbInvestor = await _context.TbInvestors.FindAsync(id);
            if (tbInvestor == null)
            {
                return NotFound();
            }
            var lots = _context.TbLots.ToList();
            var investorsByLot = _context.TbInvestors
                                    .GroupBy(i => i.ILot)
                                    .ToDictionary(g => g.Key, g => g.Count());

            var availableLots = lots.Where(lot => (!investorsByLot.ContainsKey(lot.LotId) || investorsByLot[lot.LotId] < 3) &&
                                          GetTotalInvestment(lot.LotId) < 50000)
                                .Select(lot => new SelectListItem
                                {
                                    Value = lot.LotId.ToString(),
                                    Text = lot.LotName.ToString()
                                })
                                .ToList();

            ViewData["IId"] = new SelectList(_context.TbUsers, "UId", "UUsername", tbInvestor.IId);
            ViewData["ILot"] = new SelectList(availableLots, "Value", "Text");
            return View(tbInvestor);
        }

        // POST: TbInvestors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IId,IInvestment,IPercent,ILot")] TbInvestor tbInvestor)
        {
            if (id != tbInvestor.IId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //tbInvestor.IPercent = (tbInvestor.IInvestment / 50000) * 100;

                    _context.Update(tbInvestor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbInvestorExists(tbInvestor.IId))
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
            var lots = _context.TbLots.ToList();
            var investorsByLot = _context.TbInvestors
                                    .GroupBy(i => i.ILot)
                                    .ToDictionary(g => g.Key, g => g.Count());

            var availableLots = lots.Where(lot => (!investorsByLot.ContainsKey(lot.LotId) || investorsByLot[lot.LotId] < 3) &&
                                          GetTotalInvestment(lot.LotId) < 50000)
                                .Select(lot => new SelectListItem
                                {
                                    Value = lot.LotId.ToString(),
                                    Text = lot.LotName.ToString()
                                })
                                .ToList();

            ViewData["IId"] = new SelectList(_context.TbUsers, "UId", "UUsername", tbInvestor.IId);
            ViewData["ILot"] = new SelectList(availableLots, "Value", "Text");
            return View(tbInvestor);
        }

        // GET: TbInvestors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbInvestors == null)
            {
                return NotFound();
            }

            var tbInvestor = await _context.TbInvestors
                .Include(t => t.IIdNavigation)
                .Include(t => t.ILotNavigation)
                .FirstOrDefaultAsync(m => m.IId == id);
            if (tbInvestor == null)
            {
                return NotFound();
            }

            return View(tbInvestor);
        }

        // POST: TbInvestors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbInvestors == null)
            {
                return Problem("Entity set 'DbRentalContext.TbInvestors'  is null.");
            }
            var tbInvestor = await _context.TbInvestors.FindAsync(id);
            if (tbInvestor != null)
            {
                _context.TbInvestors.Remove(tbInvestor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbInvestorExists(int id)
        {
          return (_context.TbInvestors?.Any(e => e.IId == id)).GetValueOrDefault();
        }

        public IActionResult InvDashboard(TbProfit tbProfit)
        {
            // Retrieve the shares
            IQueryable<TbShare> tbShares;
            tbShares = _context.TbShares
                    .Include(t => t.ShareInvestorNavigation)
                    .ThenInclude(i => i.IIdNavigation)
                    .OrderByDescending(s => s.ShareDate);

            int currYear = DateTime.Now.Year;
            int currMonth = DateTime.Now.Month;
            int currDay = DateTime.Now.Day;

            var profit = _context.TbProfits.FirstOrDefault(s => s.Year == currYear && s.Month == currMonth);
            var investorsList = _context.TbInvestors.Include(t => t.IIdNavigation)
                                            .Include(t => t.ILotNavigation)
                                            .ToList();
            int investors = _context.TbInvestors.Count();

            //int totalLot = _context.TbBuilding.Where(l => l.Status == "Full").Count();
            int maxLotNo = _context.TbInvestors.Max(i => i.ILot);

            var income = _context.TbSales
                .Where(e => e.SDate.Year == currYear && e.SDate.Month == currMonth && e.SAmountIn.HasValue && e.SCategory != "General")
                .ToList();

            var expenses = _context.TbSales
                .Where(e => e.SDate.Year == currYear && e.SDate.Month == currMonth && e.SAmountOut.HasValue && e.SCategory != "General")
                .ToList();

            double totalIncome = (double)income.Sum(e => e.SAmountIn);
            double totalExpense = (double)expenses.Sum(e => e.SAmountOut);
            double currentProfit = totalIncome - totalExpense;
            double lotProfit = currentProfit * 0.6 / (3 + maxLotNo);

            if (currDay == 1)
            {
                var profitCheck = _context.TbShares.FirstOrDefault(s => s.ShareDate.Year == currYear && s.ShareDate.Month == currMonth);

                if (profitCheck == null)
                {
                    foreach (var investor in investorsList)
                    {
                        double share = lotProfit * investor.IPercent / 100;

                        var shareObject = new TbShare
                        {
                            ShareAmount = share,
                            ShareDate = DateTime.Now,
                            ShareInvestor = investor.IId
                        };

                        _context.TbShares.Add(shareObject);
                    }
                }

                _context.SaveChanges();
            }

            ViewBag.currDate = DateTime.Now.ToString("MMMM yyyy");
            ViewBag.profit = currentProfit;
            ViewBag.share = lotProfit;
            ViewBag.totalInvestor = investors;

            return View(tbShares.ToList());
        }

   

        //public IActionResult InvDashboard(TbProfit tbProfit)
        //{
        //    var userRoleId = User.FindFirst(ClaimTypes.Role)?.Value;

        //    // Retrieve the shares
        //    IQueryable<TbShare> tbShares;

        //    // Check if the user's role ID is 4
        //    if (userRoleId == "4")
        //    {
        //        // Get the shares belonging to the current user
        //        var userId = int.Parse(User.Identity.Name);
        //        tbShares = _context.TbShares
        //            .Include(t => t.ShareInvestorNavigation)
        //            .Where(s => s.ShareInvestorNavigation != null && s.ShareInvestorNavigation.IIdNavigation.UId == userId)
        //            .OrderByDescending(s => s.ShareDate);
        //    }
        //    else
        //    {
        //        // For other roles, retrieve all shares
        //        tbShares = _context.TbShares
        //            .Include(t => t.ShareInvestorNavigation)
        //            .OrderByDescending(s => s.ShareDate);
        //    }

        //    int currYear = DateTime.Now.Year;
        //    int currMonth = DateTime.Now.Month;
        //    int currDay = DateTime.Now.Day;

        //    var profit = _context.TbProfits.FirstOrDefault(s => s.Year == currYear && s.Month == currMonth);
        //    var investorsList = _context.TbInvestors.ToList();
        //    int investors = _context.TbInvestors.Count();

        //    //int totalLot = _context.TbBuilding.Where(l => l.Status == "Full").Count();
        //    //int maxLotNo = _context.TbInvestors.Max(i => i.ILot);
        //    int maxLotNo = _context.TbInvestors
        //                            .Select(i => i.ILot)
        //                            .ToList() // Fetch data into memory
        //                            .DefaultIfEmpty(0)
        //                            .Max();


        //    var income = _context.TbSales
        //        .Where(e => e.SDate.Year == currYear && e.SDate.Month == currMonth && e.SAmountIn.HasValue && e.SCategory != "General")
        //        .ToList();

        //    var expenses = _context.TbSales
        //        .Where(e => e.SDate.Year == currYear && e.SDate.Month == currMonth && e.SAmountOut.HasValue && e.SCategory != "General")
        //        .ToList();

        //    double totalIncome = (double)income.Sum(e => e.SAmountIn);
        //    double totalExpense = (double)expenses.Sum(e => e.SAmountOut);
        //    double currentProfit = totalIncome - totalExpense;
        //    double lotProfit = currentProfit * 0.6 / (3 + maxLotNo);
        //    lotProfit = Math.Round(lotProfit, 2);


        //    if (currDay == 15)
        //    {
        //        var profitCheck = _context.TbShares.FirstOrDefault(s => s.ShareDate.Year == currYear && s.ShareDate.Month == currMonth);

        //        if (profitCheck == null)
        //        {
        //            foreach (var investor in investorsList)
        //            {
        //                double share = lotProfit * investor.IPercent;

        //                var shareObject = new TbShare
        //                {
        //                    ShareAmount = share,
        //                    ShareDate = DateTime.Now,
        //                    ShareInvestor = investor.IId
        //                };

        //                _context.TbShares.Add(shareObject);
        //            }
        //        }

        //        _context.SaveChanges();
        //    }

        //    ViewBag.currDate = DateTime.Now.ToString("MMMM yyyy");
        //    ViewBag.profit = currentProfit;
        //    ViewBag.share = lotProfit;
        //    ViewBag.totalInvestor = investors;

        //    return View(tbShares.ToList());
        //}
    }
}
