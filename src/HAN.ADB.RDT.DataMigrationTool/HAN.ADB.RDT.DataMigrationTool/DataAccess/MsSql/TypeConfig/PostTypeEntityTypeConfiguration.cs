using HAN.ADB.RDT.DataMigrationTool.Core.Mssql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql.TypeConfig
{
	public class PostTypeEntityTypeConfiguration : IEntityTypeConfiguration<PostType>
	{
		public PostTypeEntityTypeConfiguration()
		{
		}

        public void Configure(EntityTypeBuilder<PostType> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}