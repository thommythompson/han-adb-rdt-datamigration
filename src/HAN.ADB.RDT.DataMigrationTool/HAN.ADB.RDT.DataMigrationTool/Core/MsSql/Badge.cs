using System;
namespace HAN.ADB.RDT.DataMigrationTool.Core.Mssql
{
	public class Badge
	{
        public int Id { get; set; }
		public string Name { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public DateTime Date { get; set; }
	}
}

