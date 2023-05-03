using System;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using HAN.ADB.RDT.DataMigrationTool.Core.SqlLite;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.SqlLite.TypeConfig;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.SqlLite
{
    public class ProgressContext : DbContext
    {
        public DbSet<PostProgress> PostProgresses { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }

        public string DbPath { get; }

        public ProgressContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "progress.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PostProgressTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserProgressTypeConfiguration());
        }
    }
}