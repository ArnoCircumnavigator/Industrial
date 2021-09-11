using Industrial.Domain.EntityModel;
using Industrial.Infra.Database.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.Database
{
    public class BusinessDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public BusinessDbContext(DbContextOptions<BusinessDbContext> options)
            : base(options)
        {

        }

        public static readonly LoggerFactory LoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(LoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //new Item_EntityTypeConfiguration().Configure(modelBuilder.Entity<Item>());
            modelBuilder.ApplyConfiguration(new Item_EntityTypeConfiguration());
        }
    }
}
