using Industrial.Domain.EntityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Industrial.Infra.Database.Mapping
{
    public class Now_EntityTypeConfiguration : IEntityTypeConfiguration<Now>
    {
        public void Configure(EntityTypeBuilder<Now> builder)
        {
            //指定Now主键为ContainerID
            builder.HasKey(n => n.ContainerID);

            //指定Now用LocationID外键到Location,
            builder
                .HasOne(n => n.Location)
                .WithMany()
                .HasForeignKey(n => n.LocationID);
        }
    }
}
