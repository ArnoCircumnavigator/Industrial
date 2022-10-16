using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Industrial.Infra.Database.Models
{
    public partial class EfCoreContext : DbContext
    {
        public EfCoreContext()
        {
        }

        public EfCoreContext(DbContextOptions<EfCoreContext> options)
            : base(options)
        {
        }
        public virtual DbSet<KgItem> KgItems { get; set; }
        public virtual DbSet<KgJob> KgJobs { get; set; }
        public virtual DbSet<KgLocation> KgLocations { get; set; }
        public virtual DbSet<KgNow> KgNows { get; set; }
        public virtual DbSet<KgNowmes> KgNowmes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=mysql-test1.mysql.database.chinacloudapi.cn;user id=DBA;password=Noproblem001;database=TestEFCore", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.39-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("latin1_swedish_ci")
                .HasCharSet("latin1");

            modelBuilder.Entity<KgItem>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("PRIMARY");

                entity.ToTable("kg_item");

                entity.Property(e => e.ItemId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ItemID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("text");
            });

            modelBuilder.Entity<KgJob>(entity =>
            {
                entity.HasKey(e => e.JobId)
                    .HasName("PRIMARY");

                entity.ToTable("kg_job");

                entity.Property(e => e.JobId).HasColumnType("int(11)");

                entity.Property(e => e.JobUniqueId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("JobUniqueID");
            });

            modelBuilder.Entity<KgLocation>(entity =>
            {
                entity.HasKey(e => e.LocationId)
                    .HasName("PRIMARY");

                entity.ToTable("kg_location");

                entity.Property(e => e.LocationId)
                    .HasColumnType("int(11)")
                    .HasColumnName("LocationID");

                var converterStatus = new ValueConverter<LocStatus, string>(
                    p => p.ToString(), //第一个表达式描述  写入存储时时转化表达式 （将enum转化成sring）
                    p => (LocStatus)Enum.Parse(typeof(LocStatus), p)); //第二个表达式描述  读取时转化表达式（将string转化成enum）

                entity.Property(e => e.Status)
                    .HasConversion(converterStatus)
                    .HasColumnType("varchar(7)");

                // 参考
                // https://learn.microsoft.com/zh-cn/dotnet/api/microsoft.entityframeworkcore.storage.valueconversion.valueconverter-2.-ctor?f1url=%3FappId%3DDev16IDEF1%26l%3DZH-CN%26k%3Dk(Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter%25602.%2523ctor)%3Bk(DevLang-csharp)%26rd%3Dtrue&view=efcore-6.0
                var converterLoadStatus = new ValueConverter<LoadStatus, string>(
                    p => p.ToString(), //第一个表达式描述  写入存储时时转化表达式 （将enum转化成sring）
                    p => (LoadStatus)Enum.Parse(typeof(LoadStatus), p)); //第二个表达式描述  读取时转化表达式（将string转化成enum）

                entity.Property(e => e.LoadStatus)
                    .HasConversion(converterLoadStatus)
                    .HasColumnType("varchar(4)");


            });

            modelBuilder.Entity<KgNow>(entity =>
            {
                entity.HasKey(e => e.ContainerId)
                    .HasName("PRIMARY");

                entity.ToTable("kg_now");

                entity.HasIndex(e => e.LocationId, "IX_KG_Now_LocationID");

                entity.Property(e => e.ContainerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ContainerID");

                entity.Property(e => e.EnterTime).HasColumnType("datetime");

                entity.Property(e => e.LocationId)
                    .HasColumnType("int(11)")
                    .HasColumnName("LocationID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.KgNows)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_KG_Now_KG_Location_LocationID");
            });

            modelBuilder.Entity<KgNowmes>(entity =>
            {
                entity.HasKey(e => e.ContainerId)
                    .HasName("PRIMARY");

                entity.ToTable("kg_nowmes");

                entity.HasIndex(e => e.ItemId, "IX_KG_NowMes_ItemID");

                entity.Property(e => e.ContainerId)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("ContainerID");

                entity.Property(e => e.ItemId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ItemID");

                entity.Property(e => e.Qty).HasColumnType("int(11)");

                entity.HasOne(d => d.Container)
                    .WithOne(p => p.KgNowmes)
                    .HasForeignKey<KgNowmes>(d => d.ContainerId)
                    .HasConstraintName("FK_KG_NowMes_KG_Now_ContainerID");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.KgNowmes)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_KG_NowMes_KG_Item_ItemID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
