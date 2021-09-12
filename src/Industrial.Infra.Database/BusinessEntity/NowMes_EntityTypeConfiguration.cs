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
    public class NowMes_EntityTypeConfiguration : IEntityTypeConfiguration<NowMes>
    {
        public void Configure(EntityTypeBuilder<NowMes> builder)
        {
            
            builder.HasKey(n => n.ContainerID);

            builder
                .HasOne(nowmes => nowmes.Item)
                .WithMany()
                .HasForeignKey(nowmes => nowmes.ItemID);


            builder
                .HasOne(n => n.Now)
                .WithOne(l => l.NowMes)
                .HasForeignKey<NowMes>(nowmes => nowmes.ContainerID)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
        }
    }
}
