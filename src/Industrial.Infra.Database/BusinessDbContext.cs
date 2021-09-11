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
        public static readonly LoggerFactory LoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });
        public DbSet<Now> Nows { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<NowMes> NowMeses { get; set; }
        public BusinessDbContext(DbContextOptions<BusinessDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(LoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new Now_EntityTypeConfiguration().Configure(modelBuilder.Entity<Now>());
            new Location_EntityTypeConfiguration().Configure(modelBuilder.Entity<Location>());
            new NowMes_EntityTypeConfiguration().Configure(modelBuilder.Entity<NowMes>());
        }
    }
}
