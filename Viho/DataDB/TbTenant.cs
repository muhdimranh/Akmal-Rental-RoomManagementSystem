using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbTenant
{
    public int TId { get; set; }

    public string TName { get; set; } = null!;

    public string TIc { get; set; } = null!;

    public string TPhone { get; set; } = null!;

    public string TAddress { get; set; } = null!;

    public string? TUploadIc { get; set; }

    public string TEmerContact { get; set; } = null!;

    public int? TRoomid { get; set; }

    public DateTime TEntrydate { get; set; }

    public DateTime? TExitdate { get; set; }

    public string TCardCode { get; set; } = null!;

    public string TAddOnDetail { get; set; } = null!;

    public string TPaymentStatus { get; set; } = null!;

    public string? TAgreement { get; set; }

    public DateTime LastReminderDate { get; set; }

    public bool IsPaymentCollected { get; set; }

    public bool IsCheckedOut { get; set; }

    public virtual TbRoom? TRoom { get; set; }

    public virtual ICollection<TbPayment> TbPayments { get; set; } = new List<TbPayment>();

    public virtual ICollection<TbRoom> TbRooms { get; set; } = new List<TbRoom>();
}
