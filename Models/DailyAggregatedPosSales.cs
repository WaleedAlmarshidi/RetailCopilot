using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RetailCopilot;

public partial class DailyAggregatedPosSales
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ulong Id { get; set; }
    public string PosId { get; set; } = null!;    

    // [NotMapped]
    [ForeignKey("PosId")]
    public virtual Pos? Pos { get; set; }

    public DateTime AggregatedFrom { get; set; }

    public double? TotalMargin { get; set; }

    public double TotalSales { get; set; }
    public uint TotalCount { get; set; }
}
