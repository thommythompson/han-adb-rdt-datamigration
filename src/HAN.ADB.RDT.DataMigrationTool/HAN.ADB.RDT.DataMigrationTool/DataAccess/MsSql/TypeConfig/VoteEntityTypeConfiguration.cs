using HAN.ADB.RDT.DataMigrationTool.Core.Mssql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql.TypeConfig
{
	public class VoteEntityTypeConfiguration : IEntityTypeConfiguration<Vote>
    {
		public VoteEntityTypeConfiguration()
		{
		}

        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Post)
                .WithMany(e => e.Votes)
                .HasForeignKey(e => e.PostId);

            builder.HasOne(e => e.User)
                .WithMany(e => e.Votes)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.VoteType)
                .WithMany()
                .HasForeignKey(e => e.VoteTypeId);
        }
    }
}

