using Microsoft.EntityFrameworkCore;
using Namatara.API.Services;

namespace Namatara.API.Models
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserService userService) : DbContext(options)
    {
        #region DbSet

        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TicketBooking> TicketBookings { get; set; }
        public DbSet<TourismAttraction> TourismAttractions { get; set; }
        public DbSet<TourismAttractionRating> TourismAttractionRatings { get; set; }

        #endregion DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Seed the Admin User
            //var adminUserId = Guid.NewGuid();  // Admin's GUID (replace it with a fixed GUID if needed)

            //modelBuilder.Entity<User>().HasData(
            //    new User
            //    {
            //        Id = adminUserId,
            //        Username = "admin",
            //        Password = "adminadmin",
            //        FullName = "Admin",
            //        Role = UserRole.Admin,
            //        CreatedAt = DateTime.UtcNow,
            //        UpdatedAt = DateTime.UtcNow
            //    }
            //);

            //// Seed Categories with the Admin User's ID as the Creator
            //modelBuilder.Entity<Category>().HasData(
            //    new Category
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Electronics",
            //        Description = "All electronic devices and gadgets",
            //        CreatedBy = adminUserId,  // Assigning the Admin user as creator
            //        CreatedAt = DateTime.UtcNow
            //    },
            //    new Category
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Furniture",
            //        Description = "Office and home furniture",
            //        CreatedBy = adminUserId,  // Assigning the Admin user as creator
            //        CreatedAt = DateTime.UtcNow
            //    },
            //    new Category
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Clothing",
            //        Description = "Apparel and accessories",
            //        CreatedBy = adminUserId,  // Assigning the Admin user as creator
            //        CreatedAt = DateTime.UtcNow
            //    }
            //);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is _AuditEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            var currentUserId = userService.GetCurrentUserId();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    // Mengisi properti audit saat entitas ditambahkan
                    ((_AuditEntity)entry.Entity).CreatedBy = currentUserId;
                    ((_AuditEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    // Mengisi properti audit saat entitas diperbarui
                    ((_AuditEntity)entry.Entity).UpdatedBy = currentUserId;
                    ((_AuditEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is _AuditEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            var currentUserId = userService.GetCurrentUserId(); // Mendapatkan ID pengguna saat ini

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    // Mengisi properti audit saat entitas ditambahkan
                    ((_AuditEntity)entry.Entity).CreatedBy = currentUserId;
                    ((_AuditEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    // Mengisi properti audit saat entitas diperbarui
                    ((_AuditEntity)entry.Entity).UpdatedBy = currentUserId;
                    ((_AuditEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}