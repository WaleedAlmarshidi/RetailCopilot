using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetailCopilot;

public partial class ShopVisitCount
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public int Count { get; set; }
    public string PosId { get; set; } = null!;
    [ForeignKey("PosId")]
    public virtual Pos? Pos { get; set; }
}
