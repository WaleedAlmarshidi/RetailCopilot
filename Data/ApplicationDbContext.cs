using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace RetailCopilot.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public virtual DbSet<Tenant> Tenants { get; set; }
    // public virtual DbSet<Partner> Partners { get; set; }
    public virtual DbSet<Contact> Contacts { get; set; }
    public virtual DbSet<DailyAggregatedPosSales> DailyAggregatedPosSales { get; set; }
    public virtual DbSet<PosOrder> PosOrders { get; set; }
    public virtual DbSet<PosSale> PosSales { get; set; }
    public virtual DbSet<Inquiry> Inquiries { get; set; }
    public virtual DbSet<ShopVisitCount> ShopVisitsCount { get; set; }
    public virtual DbSet<Violation> Violations { get; set; }
    public virtual DbSet<ViolationRecord> ViolationRecords { get; set; }
    public virtual DbSet<Pos> Pos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:businessvar.database.windows.net,1433;Initial Catalog=bv;Persist Security Info=False;User ID=CloudSA3b4f7089;Password=DbPass2000%;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.TenantId);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Domain)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.ContactEmail)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("active");

            entity.Property(e => e.Plan)
                .HasMaxLength(50)
                .HasDefaultValue("basic");

            entity.Property(e => e.Locale)
                .HasMaxLength(50)
                .HasDefaultValue("en");

            entity.Property(e => e.Theme)
                .HasMaxLength(255);
        });
        modelBuilder.Entity<Violation>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Name)
                .IsUnique();
        });
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Contact (res.partner)");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.AverageTicketAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Company).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.TotalSpentAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ZidId).HasColumnName("ZID_ID");
            entity.HasIndex(e => e.Phone);
        });

        modelBuilder.Entity<ShopVisitCount>()
            .HasKey(svc => new { svc.Date, svc.PosId });

        modelBuilder.Entity<PosOrder>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(50)
                .HasColumnName("Customer_Phone");
            entity.Property(e => e.Pos)
                .HasMaxLength(50)
                .HasColumnName("POS");
        });

        modelBuilder.Entity<PosSale>(entity =>
        {
            entity.HasIndex(e => e.Id, "dbo_possales_id_unique")
                .IsUnique()
                .HasFilter("([ID] IS NOT NULL)");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("ID");
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.EmployeeExternalId)
                .HasMaxLength(255)
                .IsRequired()
                .HasDefaultValue("0")   // Ensure the length matches the PrincipalKey
                .IsUnicode(false);

            entity.HasOne(d => d.Employee)
                .WithMany(p => p.PosSales)
                .HasPrincipalKey(p => p.ExternalId)
                .HasForeignKey(ps => ps.EmployeeExternalId)
                .HasConstraintName("FK_PosSales_Users_EmployeeExternalId")
                .OnDelete(DeleteBehavior.Restrict);  // Ensure cascade delete behavior
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.UserName)
                .IsUnicode(true);

            entity.Property(e => e.Id)
                .HasValueGenerator<GuidValueGenerator>();

            entity.HasMany(u => u.AuthorizedPointOfSales)
                .WithMany(p => p.Users)
                .UsingEntity(j => j.ToTable("UserAuthorizedPointOfSales"));

            entity.Property(e => e.ExternalId)
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(e => e.ExternalId)
                .IsUnique();
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(true)
                .IsRequired();
            // Any other configuration for ApplicationUser
        });
        
        modelBuilder.Entity<ApplicationUser>()
               .Property(e => e.UserName)
               .UseCollation("Arabic_CI_AS");
        modelBuilder.Entity<ApplicationUser>()
               .Property(e => e.UserName)
               .IsUnicode(true);
        // modelBuilder.Entity<Partner>(entity =>
        // {
        //     entity.HasKey(e => e.Id);

        //     entity.Property(e => e.FirstName)
        //         .IsRequired()
        //         .HasMaxLength(255);

        //     entity.Property(e => e.LastName)
        //         .IsRequired()
        //         .HasMaxLength(255);

        //     entity.Property(e => e.Email)
        //         .IsRequired()
        //         .HasMaxLength(255);

        //     entity.Property(e => e.CreatedAt)
        //         .HasDefaultValueSql("GETUTCDATE()");

        //     entity.Property(e => e.UpdatedAt)
        //         .HasDefaultValueSql("GETUTCDATE()");

        //     entity.Property(e => e.ExternalId)
        //         .IsRequired()
        //         .HasConversion(v => v, v => v)
        //         .HasMaxLength(255);

        //     entity.HasIndex(e => e.ExternalId)
        //         .IsUnique();
        //     // entity.HasOne(e => e.Tenant)
        //     //     .WithMany(t => t.Employees)
        //     //     .HasForeignKey(e => e.TenantId)
        //     //     .OnDelete(DeleteBehavior.Cascade);
        // });
    }
}
