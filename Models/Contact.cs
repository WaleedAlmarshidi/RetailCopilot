using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace RetailCopilot;

public partial class Contact
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string? Company { get; set; }

    public string? CreatedBy { get; set; }

    public short SaleOrderCount { get; set; } = 0;

    public short PosOrderCount { get; set; } = 0;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public int? ZidId { get; set; }

    public string? Name { get; set; }

    public DateTime? LastVisitDate { get; set; }

    public decimal? AverageTicketAmount { get; set; } = 0;

    public decimal? TotalSpentAmount { get; set; } = 0;

    public long? LastPosSaleId { get; set; }
    [ForeignKey("LastPosSaleId")]
    public virtual PosSale? LastPosSale { get; set; }

    public int? AverageSessionTimeInMinutes { get; set; }
    // public virtual Inquiry? LastInquiry { get; set; }
    public DateTime? LastInquiryAt { get; set; }
    public short GetTotalSalesCount(){
        return (short)(this.PosOrderCount + this.SaleOrderCount);
    }
    public ushort GetTimeElapsedSinceLastVisit()
    {
        return (ushort)DateTime.UtcNow.Subtract(this.LastVisitDate.GetValueOrDefault()).TotalDays;
    }
}
public class SalesCopilotContact : Contact 
{
    public List<Product> purchasedProducts { get; set; } = new List<Product>();

}
public class Product
{
    [JsonPropertyName("full_product_name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("qty")]
    public double Quantity { get; set; }
}
