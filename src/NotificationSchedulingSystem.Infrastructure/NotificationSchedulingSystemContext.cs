using Microsoft.EntityFrameworkCore;
using NotificationSchedulingSystem.Infrastructure.Models;

namespace NotificationSchedulingSystem.Infrastructure;

public class NotificationSchedulingSystemContext : DbContext
{
    public NotificationSchedulingSystemContext()
    {
    }

    public NotificationSchedulingSystemContext(DbContextOptions<NotificationSchedulingSystemContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            throw new ArgumentException("ConnectionString is not configured properly", nameof(optionsBuilder));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasIndex(x => x.Name, "UC_Company_Name").IsUnique();
            entity.HasIndex(x => x.Number, "UC_Company_Number").IsUnique();
            entity.Property(x => x.Id).HasDefaultValueSql("(newid())");
            entity.Property(x => x.Name).HasMaxLength(30);
            entity.Property(x => x.CompanyType).HasColumnType("nvarchar(50)");
            entity.Property(x => x.Market).HasColumnType("nvarchar(50)");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(x => x.SendingDate).HasColumnType("Date");
            entity.HasOne(x => x.Company)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.CompanyId)
                .HasConstraintName("FK_Notifications_With_Companies");
        });
    }
}