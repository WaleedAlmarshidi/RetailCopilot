// using System;
// using System.Collections.Generic;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;

// namespace RetailCopilot;

// partial class AppDbContext : IdentityDbContext<User>
// {
//     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
//     {
//     }
//     public virtual DbSet<Contact> Contacts { get; set; }
//     // public virtual DbSet<DailyAggregatedResult> DailyAggregatedResults { get; set; }
//     public virtual DbSet<DailyAggregatedPosSales> DailyAggregatedPosSales { get; set; }
//     public virtual DbSet<PosOrder> PosOrders { get; set; }
//     public virtual DbSet<PosSale> PosSales { get; set; }
//     public virtual DbSet<Inquiry> Inquiries { get; set; }
//     public virtual DbSet<ShopVisitCount> ShopVisitsCount { get; set; }
//     public virtual DbSet<Violation> Violations { get; set; }
//     public virtual DbSet<ViolationRecord> ViolationRecords { get; set; }
//     public virtual DbSet<Pos> Pos { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseSqlServer("Server=tcp:businessvar.database.windows.net,1433;Initial Catalog=bv;Persist Security Info=False;User ID=CloudSA3b4f7089;Password=DbPass2000%;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder.Entity<Contact>(entity =>
//         {
//             entity.HasKey(e => e.Id).HasName("PK_Contact (res.partner)");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("ID");
//             entity.Property(e => e.AverageTicketAmount).HasColumnType("decimal(10, 2)");
//             entity.Property(e => e.City).HasMaxLength(50);
//             entity.Property(e => e.Company).HasMaxLength(50);
//             entity.Property(e => e.Country).HasMaxLength(50);
//             entity.Property(e => e.CreatedBy).HasMaxLength(50);
//             entity.Property(e => e.Email).HasMaxLength(50);
//             entity.Property(e => e.Name).HasMaxLength(150);
//             entity.Property(e => e.Phone).HasMaxLength(50);
//             entity.Property(e => e.TotalSpentAmount).HasColumnType("decimal(10, 2)");
//             entity.Property(e => e.ZidId).HasColumnName("ZID_ID");
//             entity.HasIndex(e => e.Phone);
//         });
//         modelBuilder.Entity<ShopVisitCount>()
//             .HasKey(svc => new { svc.Date, svc.PosId });
//         modelBuilder.Entity<PosOrder>(entity =>
//         {
//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("ID");
//             entity.Property(e => e.CompanyName).HasMaxLength(50);
//             entity.Property(e => e.CustomerPhone)
//                 .HasMaxLength(50)
//                 .HasColumnName("Customer_Phone");
//             entity.Property(e => e.Employee).HasMaxLength(50);
//             entity.Property(e => e.Pos)
//                 .HasMaxLength(50)
//                 .HasColumnName("POS");
//         });

//         modelBuilder.Entity<PosSale>(entity =>
//         {
//             entity.HasIndex(e => e.Id, "dbo_possales_id_unique")
//                 .IsUnique()
//                 .HasFilter("([ID] IS NOT NULL)");

//             entity.Property(e => e.Id)
//                 .HasMaxLength(255)
//                 .HasColumnName("ID");
//             entity.Property(e => e.CompanyName).HasMaxLength(50);
//             entity.Property(e => e.CustomerName).HasMaxLength(255);
//             entity.Property(e => e.CustomerPhone)
//                 .HasMaxLength(50)
//                 .IsUnicode(false);
//         });

//         OnModelCreatingPartial(modelBuilder);
//     }

//     partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
// }
