using HAN.ADB.RDT.DataMigrationTool.Core.Mssql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql.TypeConfig
{
	public class BageEntityTypeConfiguration : IEntityTypeConfiguration<Badge>
	{
		public BageEntityTypeConfiguration()
		{
		}

        public void Configure(EntityTypeBuilder<Badge> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.User)
                .WithMany(e => e.Badges)
                .HasForeignKey(e => e.UserId);
        }
    }
}