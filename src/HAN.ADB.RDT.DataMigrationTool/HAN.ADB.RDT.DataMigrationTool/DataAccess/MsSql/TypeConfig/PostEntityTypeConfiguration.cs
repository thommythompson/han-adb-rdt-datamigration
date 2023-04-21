using HAN.ADB.RDT.DataMigrationTool.Core.Mssql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql.TypeConfig
{
	public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
	{
		public PostEntityTypeConfiguration()
		{
		}

        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");

            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.AcceptedAnswer)
                .WithOne(e => e.AcceptedAnswerFor)
                .HasForeignKey<Post>(e => e.AcceptedAnswerId);

            builder.HasOne(e => e.LastEditorUser)
                .WithMany(e => e.LastEditorForPosts)
                .HasForeignKey(e => e.LastEditorUserId);

            builder.HasOne(e => e.OwnerUser)
                .WithMany(e => e.OwnerForPosts)
                .HasForeignKey(e => e.OwnerUserId);

            builder.HasOne(e => e.Parent)
                .WithMany(e => e.Answers)
                .HasForeignKey(e => e.ParentId);

            builder.HasOne(e => e.PostType)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.PostTypeId);
        }
    }
}