using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetailCopilot;

public partial class ViolationRecord
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ViolationId { get; set; }
    [ForeignKey("ViolationId")]
    public Violation? Violation { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ViolatedAt { get; set; }
    public string PosId { get; set; } = null!;
    [ForeignKey("PosId")]
    public Pos? Pos { get; set; }
    public string? AttachmentUrl { get; set; }
    public string? TicketUrl { get; set; }
}
