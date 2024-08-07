using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RetailCopilot.Data;

namespace RetailCopilot;

public partial class Pos
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; } = null!;
    public DateTime SubscriptionDueDate { get; set; } = DateTime.UtcNow.AddYears(1);
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? CompanyName { get; set; }
    public virtual Contact? Manager { get; set; }
    // public ICollection<Employee>? Employees { get; set; }
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public ICollection<ApplicationUser>? Users { get; set; }
}
