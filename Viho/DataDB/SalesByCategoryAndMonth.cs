namespace Viho.web.DataDB
{
    public class SalesByCategoryAndMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public string? Category { get; set; }

        public double? TotalSales { get; set; }

        public string? ProfitLoss { get; set; }
    }
}
