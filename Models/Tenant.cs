namespace RetailCopilot;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RetailCopilot.Data;

public partial class Tenant
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid TenantId { get; set; }

    public string? ExternalCompanyId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    [Required]
    [MaxLength(255)]
    public string Domain { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string ContactEmail { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string Status { get; set; } = "active";
    [MaxLength(50)]
    public string Plan { get; set; } = "basic";

    [MaxLength(255)]
    public string Theme { get; set; }

    [MaxLength(50)]
    public string Locale { get; set; } = "en";

    public Guid? AdminUserId { get; set; }

    public ICollection<Pos> Poss { get; set; } = new List<Pos>();
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}
