using Localizy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Localizy.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Translation> Translations { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Validation> Validations { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<HomeSlide> HomeSlides { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Avatar).HasMaxLength(500);
            entity.Property(e => e.Role).HasConversion<string>();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure Project
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.DefaultLanguage).HasMaxLength(10);

            entity.HasOne(p => p.User)
                  .WithMany(u => u.Projects)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Translation
        modelBuilder.Entity<Translation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Key).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Language).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Value).IsRequired();

            entity.HasOne(t => t.Project)
                  .WithMany(p => p.Translations)
                  .HasForeignKey(t => t.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(t => new { t.ProjectId, t.Key, t.Language }).IsUnique();
        });

        // Configure Setting
        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Key).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Value).IsRequired();
            entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasIndex(e => e.Key).IsUnique();
            entity.HasIndex(e => e.Category);
        });

        // Configure City
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Country);
        });

        // Configure Address - CHỈ ĐỊNH NGHĨA MỘT LẦN
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.FullAddress).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Website).HasMaxLength(200);
            entity.Property(e => e.OpeningHours).HasMaxLength(500);
            entity.Property(e => e.VerificationNotes).HasMaxLength(1000);
            entity.Property(e => e.RejectionReason).HasMaxLength(1000);

            // City relationship
            entity.HasOne(a => a.City)
                  .WithMany(c => c.Addresses)
                  .HasForeignKey(a => a.CityId)
                  .OnDelete(DeleteBehavior.SetNull);

            // User relationships
            entity.HasOne(a => a.SubmittedByUser)
                  .WithMany()
                  .HasForeignKey(a => a.SubmittedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.VerifiedByUser)
                  .WithMany()
                  .HasForeignKey(a => a.VerifiedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.CityId);
            entity.HasIndex(e => new { e.Latitude, e.Longitude });
        });

        // Configure Validation
        modelBuilder.Entity<Validation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequestId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Priority).HasConversion<string>();
            entity.Property(e => e.RequestType).HasConversion<string>();
            entity.Property(e => e.Notes).HasMaxLength(2000);
            entity.Property(e => e.OldData).HasMaxLength(4000);
            entity.Property(e => e.NewData).HasMaxLength(4000);
            entity.Property(e => e.ProcessingNotes).HasMaxLength(2000);
            entity.Property(e => e.RejectionReason).HasMaxLength(2000);
            entity.Property(e => e.IdType).HasMaxLength(50);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.AppointmentTimeSlot).HasMaxLength(50);

            // Relationships
            entity.HasOne(v => v.Address)
                  .WithMany()
                  .HasForeignKey(v => v.AddressId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(v => v.SubmittedByUser)
                  .WithMany()
                  .HasForeignKey(v => v.SubmittedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(v => v.ProcessedByUser)
                  .WithMany()
                  .HasForeignKey(v => v.ProcessedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.RequestId).IsUnique();
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Priority);
            entity.HasIndex(e => e.SubmittedDate);
        });


        // Configure HomeSlide
        modelBuilder.Entity<HomeSlide>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ImageFileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ImagePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
            entity.HasIndex(e => e.Order);
            entity.HasIndex(e => e.IsActive);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}