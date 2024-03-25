using System;
using System.Collections.Generic;

namespace RetailCopilot;

public partial class PosOrder
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public string? Employee { get; set; }

    public string? CompanyName { get; set; }

    public string? Pos { get; set; }

    public string? CustomerPhone { get; set; }

    public double? Margin { get; set; }

    public double? Total { get; set; }
}
