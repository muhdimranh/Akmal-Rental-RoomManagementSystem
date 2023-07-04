using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Viho.web.DataDB;


namespace Viho.web.Controllers
{
    public class TbSales1Controller : Controller
    {
        private readonly DbRentalContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TbSales1Controller(DbRentalContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: TbSales1
        public async Task<IActionResult> Index()
        {
            var sales = await _context.TbSales.ToListAsync();
            return View(sales);
        }

        // GET: TbSales1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbSales == null)
            {
                return NotFound();
            }

            var tbSale = await _context.TbSales
                .FirstOrDefaultAsync(m => m.SId == id);
            if (tbSale == null)
            {
                return NotFound();
            }

            return View(tbSale);
        }

        // GET: TbSales1/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryList = await GetCategoryListAsync();
            ViewBag.TransactionTypeList = await GetTransactionTypeListAsync();

            return View();
        }

        // POST: TbSales1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SId,SDate,SCategory,STransactionType,SDetail,SAmountIn,SAmountOut,SReceipt,ReceiptFile")] TbSale tbSale, IFormFile? SReceipt)
        {
            if (ModelState.IsValid)
            {
                if (SReceipt != null && SReceipt.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Picture/Receipt/");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + SReceipt.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await SReceipt.CopyToAsync(fileStream);
                    }

                    tbSale.SReceipt = uniqueFileName;
                }

                _context.Add(tbSale);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryList = await GetCategoryListAsync();
            ViewBag.TransactionTypeList = await GetTransactionTypeListAsync();
            return View(tbSale);
        }

        // GET: TbSales1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbSale = await _context.TbSales.FindAsync(id);
            if (tbSale == null)
            {
                return NotFound();
            }

            ViewBag.CategoryList = await GetCategoryListAsync();
            ViewBag.TransactionTypeList = await GetTransactionTypeListAsync();
            ViewBag.ExistingReceiptFileName = tbSale.SReceipt; // Set the existing receipt file name in ViewBag

            return View(tbSale);
        }

            // POST: TbSales1/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SId,SDate,SCategory,STransactionType,SDetail,SAmountIn,SAmountOut,SReceipt,ReceiptFile")] TbSale tbSale, IFormFile SReceipt)
        {
            if (id != tbSale.SId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (SReceipt != null && SReceipt.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Picture/Receipt/");

                        if (!string.IsNullOrEmpty(tbSale.SReceipt))
                        {
                            string previousFilePath = Path.Combine(uploadsFolder, tbSale.SReceipt);
                            if (System.IO.File.Exists(previousFilePath))
                            {
                                System.IO.File.Delete(previousFilePath);
                            }
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + SReceipt.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await SReceipt.CopyToAsync(fileStream);
                        }

                        // Update the image property
                        tbSale.SReceipt = uniqueFileName;
                    }

                    _context.Update(tbSale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbSaleExists(id))
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

            ViewBag.CategoryList = await GetCategoryListAsync();
            ViewBag.TransactionTypeList = await GetTransactionTypeListAsync();
            ViewBag.ExistingReceiptFileName = tbSale.SReceipt;// Set the existing receipt file name in ViewBag

            return View(tbSale);
        }


        // GET: TbSales1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbSale = await _context.TbSales.FindAsync(id);
            if (tbSale == null)
            {
                return NotFound();
            }

            return View(tbSale);
        }


        // POST: TbSales1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbSale = await _context.TbSales.FindAsync(id);
            if (tbSale == null)
            {
                return NotFound();
            }

            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(tbSale.SReceipt))
            {
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Picture/Receipt", tbSale.SReceipt);

                // Check if the image file exists before deleting
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.TbSales.Remove(tbSale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbSaleExists(int id)
        {
          return (_context.TbSales?.Any(e => e.SId == id)).GetValueOrDefault();
        }

        private static Task<List<SelectListItem>> GetCategoryListAsync()
        {
            var categoryList = new List<SelectListItem>
            {
                new SelectListItem { Value = "General", Text = "General" },
                new SelectListItem { Value = "GE1", Text = "GE1" },
                new SelectListItem { Value = "GE2", Text = "GE2" },
                new SelectListItem { Value = "GE3", Text = "GE3" },
                new SelectListItem { Value = "TN1", Text = "TN1" },
                new SelectListItem { Value = "TN2", Text = "TN2" },
                new SelectListItem { Value = "TN3", Text = "TN3" },
                new SelectListItem { Value = "TN4", Text = "TN4" }
            };

            return Task.FromResult(categoryList);
        }

        private static Task<List<SelectListItem>> GetTransactionTypeListAsync()
        {
            var transactionTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Cash", Text = "Cash" },
                new SelectListItem { Value = "Credit Card", Text = "Credit Card" },
                new SelectListItem { Value = "Bank Transfer", Text = "Bank Transfer" },
            };

            return Task.FromResult(transactionTypeList);
        }

        // GET: TbSales1/DownloadReceipt/5
        public async Task<IActionResult> DownloadReceipt(int id)
        {
            var sale = await _context.TbSales.FindAsync(id);
            if (sale == null || string.IsNullOrEmpty(sale.SReceipt))
            {
                return NotFound();
            }

            // Get the file path
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Picture/Receipt", sale.SReceipt);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // Set the appropriate content type based on the file extension
            var contentType = GetContentType(filePath);

            // Return the file with the original file name and content type
            return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }


        public async Task<ActionResult> SalesByCategoryAndMonth()
        {
            var salesByCategoryAndMonth = _context.TbSales
                .GroupBy(s => new { s.SCategory, s.SDate.Year, s.SDate.Month })
                .Select(g => new SalesByCategoryAndMonth
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Category = g.Key.SCategory,
                    TotalSales = g.Sum(s => (s.SAmountIn ?? 0) - (s.SAmountOut ?? 0)),
                    ProfitLoss = "",
                })
                .ToList();

            var salesByMonth = _context.TbSales
                .GroupBy(s => new { s.SDate.Year, s.SDate.Month })
                .Select(g => new TbProfit
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalSales = g.Sum(s => (s.SAmountIn ?? 0) - (s.SAmountOut ?? 0)),
                })
                .ToList();

            foreach (var item in salesByMonth)
            {
                var profit = _context.TbProfits.FirstOrDefault(p => p.Year == item.Year && p.Month == item.Month);

                if (profit != null)
                {
                    // Update the existing record in TbProfit
                    profit.TotalSales = (double)item.TotalSales;
                }
                else
                {
                    // Add a new record to TbProfit
                    profit = new TbProfit
                    {
                        Year = item.Year,
                        Month = item.Month,
                        TotalSales = (double)item.TotalSales
                    };
                    _context.TbProfits.Add(profit);
                }
            }

            // Calculate the profit/loss and update the ProfitLoss column for each item
            foreach (var item in salesByCategoryAndMonth)
            {
                if (item.TotalSales < 0)
                {
                    // If TotalSales is negative, it represents a loss
                    item.ProfitLoss = "Loss";
                }
                else if (item.TotalSales > 0)
                {
                    // If TotalSales is positive, it represents a profit
                    item.ProfitLoss = "Profit";
                }
                else
                {
                    // If TotalSales is zero, it can be considered as neither profit nor loss
                    item.ProfitLoss = "No profit/loss";
                }
            }

            await _context.SaveChangesAsync();

            return View(salesByCategoryAndMonth);
        }







        public ActionResult SalesDashboard()
        {
            // Retrieve data for the dashboard
            List<double> TotalSalesByYear = GetTotalSalesByYear();
            List<double> TotalExpensesByYear = GetTotalExpensesByYear();
            List<double> TotalSalesByCategory = GetTotalSalesByCategory();
            List<(DateTime Date, double TotalSales)> salesTimeline = GetSalesTimeline();
            List<string> Categories = GetCategoriesFromDatabase();
            // Calculate profit for each month
            List<double> TotalProfitByMonth = new List<double>();
            List<double> TotalExpensesByMonth = GetTotalExpensesByMonth();
            List<(string Month, double Profit, double Expenses)> profitAndExpensesByMonth = GetProfitAndExpensesByMonth();
            // Ensure both data lists have the same size
            int count = Math.Min(salesTimeline.Count, TotalExpensesByYear.Count);
            for (int i = 0; i < count; i++)
            {
                double profit = salesTimeline[i].TotalSales - TotalExpensesByYear[i];
                TotalProfitByMonth.Add(profit);
            }


            double grossProfit = CalculateGrossProfit();
            double netProfitMargin = CalculateNetProfitMargin();
            double salesGrowthRate = CalculateSalesGrowthRate();
            double expenseToSalesRatio = CalculateExpenseToSalesRatio();
            double profitForCurrentMonth = CalculateProfitForCurrentMonth();

            Dictionary<string, double> profitByCategory = GetProfitByCategory();
            Dictionary<string, int> LossesByCategory = GetLossesByCategory();


            // Prepare data for charts
            var months = profitAndExpensesByMonth.Select(p => p.Month).ToList();
            var profitData = profitAndExpensesByMonth.Select(p => p.Profit).ToList();
            var expensesData = profitAndExpensesByMonth.Select(p => p.Expenses).ToList();

            // Pass data to the view
            ViewBag.Months = JsonConvert.SerializeObject(months);
            ViewBag.ProfitData = JsonConvert.SerializeObject(profitData);
            ViewBag.ExpensesData = JsonConvert.SerializeObject(expensesData);

            // Prepare data for charts
            ViewBag.SalesByYear = JsonConvert.SerializeObject(salesTimeline.Select(s => s.TotalSales));
            ViewBag.SalesByCategory = JsonConvert.SerializeObject(TotalSalesByCategory);
            ViewBag.ExpensesByYear = JsonConvert.SerializeObject(TotalExpensesByYear);
            ViewBag.ProfitByCategory = JsonConvert.SerializeObject(profitByCategory);
            ViewBag.GrossProfit = grossProfit;
            ViewBag.NetProfitMargin = netProfitMargin;
            ViewBag.SalesGrowthRate = salesGrowthRate;
            ViewBag.ExpenseToSalesRatio = expenseToSalesRatio;
            ViewBag.ProfitByMonth = JsonConvert.SerializeObject(TotalProfitByMonth);
            ViewBag.SalesTimeline = JsonConvert.SerializeObject(salesTimeline);
            ViewBag.Categories = Categories;
            ViewBag.LossesByCategory = JsonConvert.SerializeObject(LossesByCategory);
            ViewBag.ExpensesByMonth = JsonConvert.SerializeObject(TotalExpensesByMonth);
            // Prepare data for sales timeline chart
            var timelineLabels = salesTimeline.Select(t => t.Date.ToString("MMM yyyy")).ToList();
            var timelineData = salesTimeline.Select(t => t.TotalSales).ToList();

            // Pass data to the view
            ViewBag.TimelineLabels = JsonConvert.SerializeObject(timelineLabels);
            ViewBag.TimelineData = JsonConvert.SerializeObject(timelineData);
            ViewBag.ProfitForCurrentMonth = profitForCurrentMonth;

            return View();
        }


        private List<(string Month, double Profit, double Expenses)> GetProfitAndExpensesByMonth()
        {
            var profitAndExpensesByMonth = _context.TbSales
                .GroupBy(s => new { s.SDate.Year, s.SDate.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                    Profit = g.Sum(s => (s.SAmountIn ?? 0) - (s.SAmountOut ?? 0)),
                    Expenses = g.Sum(s => s.SAmountOut ?? 0)
                })
                .ToList();

            return profitAndExpensesByMonth.Select(p => (p.Month, p.Profit, p.Expenses)).ToList();
        }

        public List<double> GetTotalExpensesByMonth()
        {
            var totalExpensesByMonth = _context.TbSales
                .GroupBy(s => new { s.SDate.Year, s.SDate.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => g.Sum(s => s.SAmountOut ?? 0))
                .ToList();

            return totalExpensesByMonth;
        }


        private List<(DateTime Date, double TotalSales)> GetSalesTimeline()
        {
            var salesTimeline = _context.TbSales
                .GroupBy(s => new { s.SDate.Year, s.SDate.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    TotalSales = g.Sum(s => s.SAmountIn ?? 0)
                })
                .ToList();

            return salesTimeline.Select(s => (s.Date, s.TotalSales)).ToList();
        }



        public List<double> GetTotalSalesByYear()
        {
            var totalSalesByYear = _context.TbSales
                .GroupBy(s => s.SDate.Year)
                .OrderBy(g => g.Key)
                .Select(g => g.Sum(s => (s.SAmountIn ?? 0) - (s.SAmountOut ?? 0)))
                .ToList();

            return totalSalesByYear;
        }


        public List<double> GetTotalSalesByCategory()
        {
            var totalSalesByCategory = _context.TbSales
                .GroupBy(s => s.SCategory)
                .OrderBy(g => g.Key)
                .Select(g => g.Sum(s => s.SAmountIn ?? 0))
                .ToList();

            return totalSalesByCategory;
        }



        public List<double> GetTotalExpensesByYear()
        {
            var totalExpensesByYear = _context.TbSales
                .GroupBy(s => s.SDate.Year)
                .OrderBy(g => g.Key)
                .Select(g => g.Sum(s => s.SAmountOut ?? 0))
                .ToList();

            return totalExpensesByYear;
        }

        public double CalculateGrossProfit()
        {
            var totalSales = GetTotalSalesByYear().Sum();
            var totalExpenses = GetTotalExpensesByYear().Sum();
            var grossProfit = totalSales - totalExpenses;
            return grossProfit;
        }

        public double CalculateNetProfitMargin()
        {
            var totalSales = GetTotalSalesByYear().Sum();
            var totalExpenses = GetTotalExpensesByYear().Sum();
            var netProfitMargin = (totalSales - totalExpenses) / totalSales * 100;
            return Math.Round(netProfitMargin, 2);

        }

        public double CalculateExpenseToSalesRatio()
        {
            var totalSales = GetTotalSalesByYear().Sum();
            var totalExpenses = GetTotalExpensesByYear().Sum();
            var expenseToSalesRatio = (totalExpenses / totalSales) * 100;
            return Math.Round(expenseToSalesRatio, 2); // Round the ratio to two decimal places
        }

        public Dictionary<string, double> GetProfitByCategory()
        {
            var profitByCategory = _context.TbSales
                .GroupBy(s => s.SCategory)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Category = g.Key,
                    Profit = g.Sum(s => (s.SAmountIn ?? 0) - (s.SAmountOut ?? 0))
                })
                .ToDictionary(x => x.Category, x => x.Profit);

            return profitByCategory;
        }

        private double CalculateSalesGrowthRate()
        {
            var salesTimeline = GetSalesTimeline();

            if (salesTimeline.Count <= 1)
            {
                return 0.0; // Return 0 if there are no or only one data point
            }

            var initialSales = salesTimeline.First().TotalSales;
            var latestSales = salesTimeline.Last().TotalSales;

            var salesGrowthRate = (latestSales - initialSales) / initialSales * 100;

            return Math.Round(salesGrowthRate, 2);
        }

        private List<(DateTime Date, double Profit)> GetProfitByMonth()
        {
            var profitByMonth = _context.TbSales
                .GroupBy(s => new { s.SDate.Year, s.SDate.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Profit = g.Sum(s => (s.SAmountIn ?? 0) - (s.SAmountOut ?? 0))
                })
                .ToList();

            return profitByMonth.Select(s => (s.Date, s.Profit)).ToList();
        }

        private double CalculateProfitForCurrentMonth()
        {
            var currentDate = DateTime.Now;
            var profitByMonth = GetProfitByMonth();
            var currentMonthProfit = profitByMonth.FirstOrDefault(p => p.Date.Month == currentDate.Month && p.Date.Year == currentDate.Year);

            // If there is no profit data available for the current month, return 0.0
            if (currentMonthProfit.Equals(default((DateTime, double))))
            {
                return 0.0;
            }

            return currentMonthProfit.Profit;
        }


        // Method to retrieve the categories from the database
        public List<string> GetCategoriesFromDatabase()
        {
            var categories = _context.TbSales
                .Select(s => s.SCategory)
                .Distinct()
                .ToList();

            return categories;
        }


        public Dictionary<string, int> GetLossesByCategory()
        {
            var lossesByCategory = _context.TbSales
                .GroupBy(s => s.SCategory)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Category = g.Key,
                    Losses = g.Count(s => (s.SAmountIn ?? 0) - (s.SAmountOut ?? 0) < 0)
                })
                .ToDictionary(x => x.Category, x => x.Losses);

            return lossesByCategory;
        }

        public Dictionary<string, double> GetExpenseBreakdownByCategory()
        {
            var expenseBreakdownByCategory = _context.TbSales
                .Where(s => s.SAmountOut.HasValue)
                .GroupBy(s => s.SCategory)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.SAmountOut.Value));

            return expenseBreakdownByCategory;
        }


    }
}
