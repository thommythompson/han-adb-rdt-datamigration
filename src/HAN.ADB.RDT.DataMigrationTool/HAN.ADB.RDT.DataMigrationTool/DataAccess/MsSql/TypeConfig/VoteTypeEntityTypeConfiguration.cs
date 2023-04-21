using HAN.ADB.RDT.DataMigrationTool.Core.Mssql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql.TypeConfig
{
	public class VoteTypeEntityTypeConfiguration : IEntityTypeConfiguration<VoteType>
	{
		public VoteTypeEntityTypeConfiguration()
		{
		}

        public void Configure(EntityTypeBuilder<VoteType> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}