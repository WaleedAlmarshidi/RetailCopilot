using Microsoft.EntityFrameworkCore;
using RetailCopilot;
using RetailCopilot.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
public interface ISalesService
{
    Task<List<EmployeeSalesReport>> GetSalesReportGroupedByEmployeesAsync(ClaimsPrincipal claimsPrincipal, bool getAllPos = false);
}

public class SalesService : ISalesService
{
    private readonly ApplicationDbContext _context;
    public SalesService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<EmployeeSalesReport>> GetSalesReportGroupedByEmployeesAsync(ClaimsPrincipal claimsPrincipal, bool getAllPos = false)
    {
        if (true)
        {
            // Fetch sales for all POS
            var Raw = _context.PosSales
                .OrderByDescending(ps => ps.Date) // Order by Date descending
                .Where(ps => ps.Pos != null); // Filter by POS ID

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
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users
                .Include(u => u.AuthorizedPointOfSales)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new List<EmployeeSalesReport>();
            }

            var authorizedPosIds = user.AuthorizedPointOfSales.Select(p => p.Id).ToList();

            if (claimsPrincipal.IsInRole("Seller"))
            {
                // Fetch sales linked to this employee only
                var salesReports = await _context.PosSales
                    .Where(ps => ps.EmployeeExternalId.Equals(user.ExternalId))
                    .OrderByDescending(ps => ps.Date)
                    .Take(10)
                    .GroupBy(ps => ps.Employee)
                    .Select(g => new EmployeeSalesReport
                    {
                        EmployeeName = g.Key.UserName,
                        TotalSales = (decimal)g.Sum(ps => ps.Total),
                    })
                    .ToListAsync();

                return salesReports;
            }

            if (claimsPrincipal.IsInRole("Branch-Manager"))
            {
                // Fetch sales for authorized POS
                var salesReports = await _context.PosSales
                    .Where(ps => authorizedPosIds.Contains(ps.PosId)) // Filter by authorized POS IDs
                    .OrderByDescending(ps => ps.Date) // Order by Date descending
                    .Take(10) // Take only the last 10 records
                    .GroupBy(ps => ps.Employee)
                    .Select(g => new EmployeeSalesReport
                    {
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