using HAN.ADB.RDT.DataMigrationTool.Core.Mssql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql.TypeConfig
{
	public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
		public UserEntityTypeConfiguration()
		{
		}

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(e => e.Id);
        }
    }
}

