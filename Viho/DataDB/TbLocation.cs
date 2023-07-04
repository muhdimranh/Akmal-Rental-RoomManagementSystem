using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbLocation
{
    public int LId { get; set; }

    public string LAddress { get; set; } = null!;

    public string? LCode { get; set; }

    public string? LWifi { get; set; }

    public string? LModemIp { get; set; }

    public string? LCctv { get; set; }

    public string? LImglayout1 { get; set; }

    public int? LReminderDate { get; set; }

    public bool IsPaymentMade { get; set; }

    public virtual ICollection<TbAttendanceCleaner> TbAttendanceCleaners { get; set; } = new List<TbAttendanceCleaner>();

    public virtual ICollection<TbInventory> TbInventories { get; set; } = new List<TbInventory>();

    public virtual ICollection<TbRoom> TbRooms { get; set; } = new List<TbRoom>();
}
