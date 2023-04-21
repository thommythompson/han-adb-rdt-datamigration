using System;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MongoDb
{
	public class MongoDbRepository
	{
		private readonly string _connectionString;

		public MongoDbRepository(string connectionString)
		{
			_connectionString = connectionString;
		}
	}
}

