using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Industrial.Infra.Database.BusinessEntity
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
            //指定Now和Nowmes的关系
            builder
                .HasOne(n => n.NowMes)
                .WithOne(l => l.Now)
                .HasForeignKey<Now>(now => now.ContainerID)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
        }
    }
}
