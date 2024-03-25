using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetailCopilot;

public partial class Inquiry
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string PosId { get; set; } = null!;
    [ForeignKey("PosId")]
    public virtual Pos Pos { get; set; } = null!;
    public virtual PosSale? AssociatedPosSale { get; set; }
    public virtual Contact ContactInquired { get; set; } = null!;

}
