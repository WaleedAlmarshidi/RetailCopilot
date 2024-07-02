namespace RetailCopilot;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RetailCopilot.Data;

public class Partner
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(255)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }

    // [Required]
    // [ForeignKey("Tenant")]
    // public Guid TenantId { get; set; }

    // public Tenant Tenant { get; set; }
    [Required]
    [MaxLength(255)]
    public required string ExternalId { get; set; }  // Ensure it's required and unique
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    // public Pos Pos { get; set; }
    public ICollection<PosSale> PosSales { get; set; }
    // Additional properties as needed
}