using HAN.ADB.RDT.DataMigrationTool.Core.SqlLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.SqlLite.TypeConfig
{
	public class UserProgressTypeConfiguration : IEntityTypeConfiguration<UserProgress>
    {
		public UserProgressTypeConfiguration()
		{
		}

        public void Configure(EntityTypeBuilder<UserProgress> builder)
        {
            builder.HasNoKey();
        }
    }
}

