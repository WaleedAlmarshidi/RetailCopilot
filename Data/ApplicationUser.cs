using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace RetailCopilot.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
        // Foreign key for Employee
    // public Guid EmployeeId { get; set; } = Guid.Empty;

    // // Navigation property
    // public virtual Employee Employee { get; set; }

    [Required]
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public override required string UserName { get; set; }
    [Required]
    [MaxLength(255)]
    [Column(TypeName = "varchar(255)")]
    public required string ExternalId { get; set; } = string.Empty;  // Ensure it's required and unique
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Pos>? AuthorizedPointOfSales { get; set; }

    public ICollection<PosSale> PosSales { get; set; }
    [Required]
    [MaxLength(255)]
    public required string FullName { get; set; }
    // public int AnyProperty2 { get; set; }
}
public class ApplicationRole : IdentityRole
{
    public string Description { get; set; } = "any description";
    // Add other custom properties as needed
}
public class ApplicationUserRole : IdentityUserRole<string>
{
    public string Description { get; set; } = "any description";
    // Add other custom properties as needed
}