using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
namespace RetailCopilot;

public enum LoyaltyBadge
{
    Metal,
    Bronz,
    Silver,
    Gold,
    Daimond
}
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

    public LoyaltyBadge loyaltyBadge { get; set; }
    public short GetTotalSalesCount(){
        return (short)(this.PosOrderCount + this.SaleOrderCount);
    }
    public ushort GetTimeElapsedSinceLastVisit()
    {
        return (ushort)DateTime.UtcNow.Subtract(this.LastVisitDate.GetValueOrDefault()).TotalDays;
    }
    private static double NormalCDF(double z)
    {
        // Constants
        double p = 0.3275911;
        double[] a = {0.254829592, -0.284496736, 1.421413741, -1.453152027, 1.061405429};

        // Save the sign of z
        int sign = z < 0 ? -1 : 1;
        z = Math.Abs(z) / Math.Sqrt(2.0);

        // A&S formula 7.1.26
        double t = 1.0 / (1.0 + p * z);
        double y = 1.0 - (((((a[4] * t + a[3]) * t) + a[2]) * t + a[1]) * t + a[0]) * t * Math.Exp(-z * z);

        return 0.5 * (1.0 + sign * y);
    }
    public LoyaltyBadge GetLoyaltyBadge ()
    {
        if (AverageTicketAmount == 0 && (SaleOrderCount + PosOrderCount) == 0)
            return LoyaltyBadge.Metal;

        double AverageTicketZ = (double)((AverageTicketAmount is null ? 360 : AverageTicketAmount  - 360) / 375);

        var AverageTicketCdf = NormalCDF(AverageTicketZ);
        var AverageTicketP = 1 - AverageTicketCdf;

        double SalesCountZ = (double)((this.PosOrderCount - 1) / 3.5);

        var SalesCountCdf = NormalCDF(SalesCountZ);
        var SalesCountP = 1 - SalesCountCdf;
        if (AverageTicketP < 0.1 && SalesCountP < 0.3)
            return LoyaltyBadge.Daimond;
        else if (AverageTicketP < 0.1)
            return LoyaltyBadge.Gold;
        else if (AverageTicketP > 0.7)
            return LoyaltyBadge.Metal;
        else if (AverageTicketP > 0.5)
            return LoyaltyBadge.Bronz;
        return LoyaltyBadge.Silver;
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
