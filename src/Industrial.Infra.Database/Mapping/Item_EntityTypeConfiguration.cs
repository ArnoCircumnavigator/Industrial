using Industrial.Domain.EntityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.Database.Mapping
{
    public class Item_EntityTypeConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("KG_ITEM");
            
            builder.HasKey(x => x.ItemID);


            builder.Property(x => x.ItemID)
                .HasMaxLength(8).IsRequired();
            builder.Property(x => x.Name)
                .HasMaxLength(20).IsRequired();
            
        }
    }
}
