using HAN.ADB.RDT.DataMigrationTool.DataAccess.MongoDb;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql;
using HAN.ADB.RDT.DataMigrationTool.Helpers.Extensions;

namespace HAN.ADB.RDT.DataMigrationTool.Helpers
{
	public class MigrationHelper
	{
		private readonly SqlContext _sqlContext;
		private readonly MongoDbRepository _mongoDbRepository;

		public MigrationHelper(SqlContext sqlContext, MongoDbRepository mongoDbRepository)
		{
			_sqlContext = sqlContext;
			_mongoDbRepository = mongoDbRepository;
		}

		public async Task MigratePosts()
		{
			await new MigratePostsHelper(_sqlContext, _mongoDbRepository).MigratePosts();
		}
	}
}