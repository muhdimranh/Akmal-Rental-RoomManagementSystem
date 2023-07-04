using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbSale
{
    public int SId { get; set; }

    public DateTime SDate { get; set; }

    public int? SPaymentid { get; set; }

    public string SCategory { get; set; } = null!;

    public string? STransactionType { get; set; }

    public string SDetail { get; set; } = null!;

    public double? SAmountIn { get; set; }

    public double? SAmountOut { get; set; }

    public string? SReceipt { get; set; }

    public virtual TbPayment? SPayment { get; set; }
}
