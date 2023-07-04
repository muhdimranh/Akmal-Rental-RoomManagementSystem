using System;
using System.Collections.Generic;

namespace Viho.web.DataDB;

public partial class TbAttendancedate
{
    public int Id { get; set; }

    public int Attid { get; set; }

    public DateTime? Attdate { get; set; }

    public virtual TbAttendanceCleaner? Att { get; set; } = null!;
}
