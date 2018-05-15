using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Jober.Models;
using Jober.Models.HomeViewModels;

namespace Jober.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<CategoryWorker> CategoryWorkers { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //builder.Entity<>().Property(u => u.).HasDefaultValue(false);

            builder.Entity<CategoryWorker>().HasKey(c => new { c.CategoryId, c.WorkerId });
            builder.Entity<CategoryWorker>().HasOne(c => c.Category).WithMany(c => c.CategoryWorkers).HasForeignKey(c => c.CategoryId);
            builder.Entity<CategoryWorker>().HasOne(c => c.Worker).WithMany(w => w.CategoryWorkers).HasForeignKey(w => w.WorkerId);

            builder.Entity<Location>().Property(l => l.IsResidence).HasDefaultValue(false);

            builder.Entity<Worker>().Property(w => w.IsActive).HasDefaultValue(true);

            builder.Entity<ApplicationUser>().Property(u => u.CityId).HasDefaultValue(1);
            builder.Entity<ApplicationUser>().Property(u => u.Balance).HasDefaultValue(0);

            builder.Entity<Worker>().Property(w => w.EvaluationGood).HasDefaultValue(0);
            builder.Entity<Worker>().Property(w => w.EvaluationBad).HasDefaultValue(0);

        }
    }
}
