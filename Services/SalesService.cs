using Microsoft.EntityFrameworkCore;
using RetailCopilot;
using RetailCopilot.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            var GroupedSalesReports = Raw.GroupBy(ps => new { ps.Employee.UserName, ps.Pos.Name });

            var salesReports = await GroupedSalesReports.Select(g => new EmployeeSalesReport
                {
                    EmployeeName = g.Key.UserName,
                    PosName = g.Key.Name,
                    TotalSales = (decimal)g.Sum(ps => ps.Total),
                })
                .ToListAsync();

            return salesReports;
        }
        else
        {
            var authorizedPosIds = user.AuthorizedPointOfSales.Select(p => p.Id).ToList();

            if (claimsPrincipal.IsInRole("Seller"))
            {
                var salesReports = await Raw
                    .Where(ps => ps.EmployeeExternalId.Equals(user.ExternalId))
                    .GroupBy(ps => new { ps.Employee.UserName, ps.Pos.Name })
                    .Select(g => new EmployeeSalesReport
                    {
                        PosName = g.Key.Name,
                        EmployeeName = user.UserName,
                        TotalSales = (decimal)g.Sum(ps => ps.Total)
                    })
                    .ToListAsync();

                return salesReports;
            }

            if (claimsPrincipal.IsInRole("Branch-Manager"))
            {
                // Fetch sales for authorized POS
                var salesReports = await Raw
                    .Where(ps => authorizedPosIds.Contains(ps.PosId)) // Filter by authorized POS IDs
                    .GroupBy(ps => new { ps.Employee.UserName, ps.Pos.Name })
                    .Select(g => new EmployeeSalesReport
                    {
                        PosName = g.Key.Name,
                        EmployeeName = g.Key.UserName,
                        TotalSales = (decimal)g.Sum(ps => ps.Total),
                    })
                    .ToListAsync();

                return salesReports;
            }

            return new List<EmployeeSalesReport>();
        }
    }
}