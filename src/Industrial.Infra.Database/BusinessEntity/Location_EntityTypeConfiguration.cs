using Industrial.Infra.Database.BusinessEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.Database.BusinessEntity
{
    public class Location_EntityTypeConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder
                .HasKey(l => l.LocationID);
            builder
                .Property(l => l.Status)
                .HasDefaultValue(LocStatus.Normal);
            builder
                .Property(l => l.LoadStatus)
                .HasDefaultValue(LocLoadStatus.idle);
        }
    }
}
