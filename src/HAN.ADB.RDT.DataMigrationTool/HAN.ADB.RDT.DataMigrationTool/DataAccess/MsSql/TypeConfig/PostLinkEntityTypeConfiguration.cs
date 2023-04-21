using HAN.ADB.RDT.DataMigrationTool.Core.Mssql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql.TypeConfig
{
	public class PostLinkEntityTypeConfiguration : IEntityTypeConfiguration<PostLink>
    {
		public PostLinkEntityTypeConfiguration()
		{
		}

        public void Configure(EntityTypeBuilder<PostLink> builder)
        {
            builder.ToTable("PostLinks");

            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Post)
                .WithMany(e => e.PostLinks)
                .HasForeignKey(e => e.PostId);

            builder.HasOne(e => e.RelatedPost)
                .WithMany(e => e.RelatedPostFor)
                .HasForeignKey(e => e.RelatedPostId);

            builder.HasOne(e => e.LinkType)
                .WithMany()
                .HasForeignKey(e => e.LinkTypeId);
        }
    }
}

