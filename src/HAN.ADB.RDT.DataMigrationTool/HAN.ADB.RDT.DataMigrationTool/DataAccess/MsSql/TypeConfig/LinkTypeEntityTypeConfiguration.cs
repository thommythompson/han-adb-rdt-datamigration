using HAN.ADB.RDT.DataMigrationTool.Core.Mssql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql.TypeConfig
{
	public class LinkTypeEntityTypeConfiguration : IEntityTypeConfiguration<LinkType>
	{
		public LinkTypeEntityTypeConfiguration()
		{
		}

        public void Configure(EntityTypeBuilder<LinkType> builder)
        {
            builder.ToTable("LinkTypes");

            builder.HasKey(e => e.Id);
        }
    }
}