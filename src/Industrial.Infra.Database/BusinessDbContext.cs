using Industrial.Infra.Database.BusinessEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.Database
{
    public class BusinessDbContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Now> Nows { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<NowMes> NowMeses { get; set; }
        public BusinessDbContext(DbContextOptions<BusinessDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new Job_EntityTypeConfiguration().Configure(modelBuilder.Entity<Job>());
            new Now_EntityTypeConfiguration().Configure(modelBuilder.Entity<Now>());
            new Location_EntityTypeConfiguration().Configure(modelBuilder.Entity<Location>());
            new NowMes_EntityTypeConfiguration().Configure(modelBuilder.Entity<NowMes>());
        }
    }
}
