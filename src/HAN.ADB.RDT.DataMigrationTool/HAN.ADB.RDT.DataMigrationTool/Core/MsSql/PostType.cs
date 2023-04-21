using System;
namespace HAN.ADB.RDT.DataMigrationTool.Core.Mssql
{
	public class PostType
	{
		public int Id { get; set; }
		public string Type { get; set; }

        // Foreign references
		public ICollection<Post> Posts { get; set; }
    }
}

