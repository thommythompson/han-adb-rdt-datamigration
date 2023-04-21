using Microsoft.EntityFrameworkCore;
using HAN.ADB.RDT.DataMigrationTool.Core.Mssql;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql.TypeConfig;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql
{
	public class SqlContext : DbContext
	{
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<LinkType> LinkTypes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLink> PostLinks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteType> VoteTypes { get; set; }

        public SqlContext(DbContextOptions<SqlContext> options)
			: base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LinkTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PostEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PostLinkEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PostTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VoteEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VoteTypeEntityTypeConfiguration());
        }
    }
}

