using Microsoft.EntityFrameworkCore;
using RetailCopilot;
using RetailCopilot.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
public interface ISalesService
{
    Task<List<EmployeeSalesReport>> GetSalesReportGroupedByEmployeesAsync(ClaimsPrincipal claimsPrincipal, bool getAllPos = false, DateTime? startDate = null, DateTime? endDate = null);
}
public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }
    public static DateTime StartOfDay(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
    }

    public static DateTime EndOfDay(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
    }
}

public class SalesService : ISalesService
{
    private readonly ApplicationDbContext _context;
    public SalesService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<EmployeeSalesReport>> GetSalesReportGroupedByEmployeesAsync(ClaimsPrincipal claimsPrincipal, bool getAllPos = false, DateTime? startDate = null, DateTime? endDate = null)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _context.Users
            .Include(u => u.AuthorizedPointOfSales)
            .Include(u => u.Tenant)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new List<EmployeeSalesReport>();
        
        var Raw = _context.PosSales
            .Where(ps => ps.Pos != null 
                && ps.Pos.TenantId.Equals(user.TenantId)
                && !ps.EmployeeExternalId.Equals("0"));

        if (startDate.HasValue)
            Raw = Raw.Where(ps => ps.Date >= startDate);
        if (endDate.HasValue)  
            Raw = Raw.Where(ps => ps.Date <= endDate);

        if (getAllPos)
        {
            var rawSales = await Raw
                .Select(ps => new
                {
                    ps.Employee,
                    ps.Pos.Name,
                    ps.Total,
                    ps.PurchasedProducts
                })
                .ToListAsync();

            var groupedSales = rawSales
                .GroupBy(ps => new { ps.Employee, ps.Name })
                .Select(g => new
                {
                    PosName = g.Key.Name,
                    EmployeeName = g.Key.Employee.FullName,
                    TotalSales = g.Sum(ps => ps.Total),
                    Products = g.SelectMany(ps => 
                        string.IsNullOrEmpty(ps.PurchasedProducts) 
                        ? new List<Product>() 
                        : JsonSerializer.Deserialize<List<Product>>(ps.PurchasedProducts) ?? new List<Product>())
                })
                .ToList();

            // Aggregate product quantities
            var salesReports = groupedSales.Select(s => new EmployeeSalesReport
            {
                PosName = s.PosName,
                EmployeeName = s.EmployeeName,
                TotalSales = (decimal)s.TotalSales,
                ProductsSold = s.Products
                    .GroupBy(p => p.Name)
                    .Select(g => new Product
                    {
                        Name = g.Key,
                        Quantity = g.Sum(p => p.Quantity)
                    })
                    .ToList()
            }).ToList();

            return salesReports;
        }
        else
        {
            var authorizedPosIds = user.AuthorizedPointOfSales.Select(p => p.Id).ToList();

            if (claimsPrincipal.IsInRole("Seller"))
            {
                var rawSales = await Raw
                    .Where(ps => ps.EmployeeExternalId.Equals(user.ExternalId))
                    .Select(ps => new
                    {
                        ps.Employee,
                        ps.Pos.Name,
                        ps.Total,
                        ps.PurchasedProducts
                    })
                    .ToListAsync();

                var groupedSales = rawSales
                    .GroupBy(ps => new { ps.Employee, ps.Name })
                    .Select(g => new
                    {
                        PosName = g.Key.Name,
                        EmployeeName = g.Key.Employee.FullName,
                        TotalSales = g.Sum(ps => ps.Total),
                        Products = g.SelectMany(ps => 
                            string.IsNullOrEmpty(ps.PurchasedProducts) 
                            ? new List<Product>() 
                            : JsonSerializer.Deserialize<List<Product>>(ps.PurchasedProducts) ?? new List<Product>())                    })
                    .ToList();

                // Aggregate product quantities
                var salesReports = groupedSales.Select(s => new EmployeeSalesReport
                {
                    PosName = s.PosName,
                    EmployeeName = s.EmployeeName,
                    TotalSales = (decimal)s.TotalSales,
                    ProductsSold = s.Products
                        .GroupBy(p => p.Name)
                        .Select(g => new Product
                        {
                            Name = g.Key,
                            Quantity = g.Sum(p => p.Quantity)
                        })
                        .ToList()
                }).ToList();

                return salesReports;
            }
            if (claimsPrincipal.IsInRole("Branch-Manager"))
            {
                var rawSales = await Raw
                    .Where(ps => authorizedPosIds.Contains(ps.PosId))
                    .Select(ps => new
                    {
                        ps.Employee,
                        ps.Pos.Name,
                        ps.Total,
                        ps.PurchasedProducts
                    })
                    .ToListAsync();

                var groupedSales = rawSales
                    .GroupBy(ps => new { ps.Employee, ps.Name })
                    .Select(g => new
                    {
                        PosName = g.Key.Name,
                        EmployeeName = g.Key.Employee.FullName,
                        TotalSales = g.Sum(ps => ps.Total),
                        Products = g.SelectMany(ps => 
                            string.IsNullOrEmpty(ps.PurchasedProducts) 
                            ? new List<Product>() 
                            : JsonSerializer.Deserialize<List<Product>>(ps.PurchasedProducts) ?? new List<Product>())
                    })
                    .ToList();

                // Aggregate product quantities
                var salesReports = groupedSales.Select(s => new EmployeeSalesReport
                {
                    PosName = s.PosName,
                    EmployeeName = s.EmployeeName,
                    TotalSales = (decimal)s.TotalSales,
                    ProductsSold = s.Products
                        .GroupBy(p => p.Name)
                        .Select(g => new Product
                        {
                            Name = g.Key,
                            Quantity = g.Sum(p => p.Quantity)
                        })
                        .ToList()
                }).ToList();

                return salesReports;
            }

            return new List<EmployeeSalesReport>();
        }
    }
}