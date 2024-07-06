using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RetailCopilot.Data;

namespace RetailCopilot;

public partial class PosSale
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    public DateTime? Date { get; set; }

    public string? CompanyName { get; set; }

    public string PosId { get; set; } = null!;

    [ForeignKey("PosId")]
    public virtual Pos? Pos { get; set; }
    public string? CustomerPhone { get; set; }
    [NotMapped]
    public int CustomerID { get; set; }
    public double? Margin { get; set; }

    public double? Total { get; set; }
    public string? PurchasedProducts { get; set; } = string.Empty;

    [NotMapped]
    public List<Product>? PurchasedProductsSerialised { get; set; }

    public string? CustomerName { get; set; }
    [MaxLength(255)]
    [Required]
    public required string EmployeeExternalId { get; set; } = "0";
    // Navigation property (if needed)
    [ForeignKey("EmployeeExternalId")]
    public virtual ApplicationUser? Employee { get; set; }
}
public class EmployeeSalesReport
{
    public string EmployeeName { get; set; }
    public string PosName { get; set; }
    public decimal TotalSales { get; set; }
    public List<Product> ProductsSold { get; set; }

}