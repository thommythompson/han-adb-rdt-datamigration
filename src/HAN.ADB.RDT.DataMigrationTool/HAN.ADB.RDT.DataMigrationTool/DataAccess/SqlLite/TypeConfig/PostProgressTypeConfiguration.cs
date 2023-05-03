using HAN.ADB.RDT.DataMigrationTool.Core.SqlLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.SqlLite.TypeConfig
{
	public class PostProgressTypeConfiguration : IEntityTypeConfiguration<PostProgress>
    {
		public PostProgressTypeConfiguration()
		{
		}

        public void Configure(EntityTypeBuilder<PostProgress> builder)
        {
            builder.HasNoKey();
        }
    }
}

