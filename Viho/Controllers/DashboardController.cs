using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Viho.web.DataDB;
using Quartz;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quartz.Impl;
using System.Threading.Tasks;
using Viho.Models;
using Viho.web.Models;
using System.Security.Claims;
using Newtonsoft.Json;

namespace Viho.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly DbRentalContext _context;

        public class DashboardViewModel
        {
            public List<TbTenant> Tenants { get; set; }
            public List<TbLocation> PendingLocations { get; set; }
        }

        public DashboardController(DbRentalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.FullName = HttpContext.Session.GetString("FullName");

            if (User.Identity.IsAuthenticated)
            {
                // User is authenticated, check the role and redirect accordingly
                var userRole = User.Claims.FirstOrDefault(c => c.Type == "URoleid")?.Value;
                if (userRole == "3") // Admin
                {
                    return RedirectToAction("AdminDashboard");
                }
                else if (userRole == "4") // Investor
                {
                    return RedirectToAction("InvestorDashboard");
                }
                // If the role is not 3 (Admin) or 4 (Investor), you can redirect to an appropriate page
                return RedirectToAction("Unauthorized");
            }

            // User is not authenticated, redirect to login page
            return RedirectToAction("Login", "Account");

           
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






        [HttpPost]
        public IActionResult MarkPaymentMade(int landlordId)
        {
            // Find the landlord by ID
            var landlord = _context.TbLocations.FirstOrDefault(l => l.LId == landlordId);

            if (landlord != null)
            {
                // Update the payment status
                landlord.IsPaymentMade = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [Authorize]
        [TypeFilter(typeof(RestrictAccessFilter))]
        public IActionResult AdminDashboard()
        {

            ViewBag.FullName = HttpContext.Session.GetString("FullName");
            // Retrieve data for the dashboard
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

            // Get the tenants with current and past reminder dates
            var currentDate = DateTime.Now;
            var tenants = _context.TbTenants
                .Include(t => t.TRoom)
                
                .Where(t => t.LastReminderDate <= currentDate && !t.IsCheckedOut && t.TPaymentStatus == "Unpaid")
                .ToList();

           

            // Get the pending landlords for the current month

            var pendingLocation = _context.TbLocations
                .Where(l => l.IsPaymentMade == false)
                .ToList();

            var viewModel = new DashboardViewModel
            {
                Tenants = tenants,
                PendingLocations = pendingLocation
            };

            return View(viewModel);
        }


        public IActionResult InvestorDashboard()
        {
            ViewBag.FullName = HttpContext.Session.GetString("FullName");
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


            // Get the tenants with current and past reminder dates
            var currentDate = DateTime.Now;
            var tenants = _context.TbTenants
                .Include(t => t.TRoom)
                .Where(t => t.LastReminderDate <= currentDate)
                .ToList();



            // Get the pending landlords for the current month

            var pendingLocation = _context.TbLocations
                .Where(l => l.IsPaymentMade == false)
                .ToList();

            var viewModel = new DashboardViewModel
            {
                Tenants = tenants,
                PendingLocations = pendingLocation
            };

            return View(viewModel);
        }


        public IActionResult Ecommerce()
        {
            return View();
        }
    }
}
