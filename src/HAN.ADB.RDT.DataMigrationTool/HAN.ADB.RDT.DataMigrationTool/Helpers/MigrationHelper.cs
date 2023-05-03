using HAN.ADB.RDT.DataMigrationTool.DataAccess.MongoDb;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.SqlLite;
using HAN.ADB.RDT.DataMigrationTool.Helpers.Extensions;

namespace HAN.ADB.RDT.DataMigrationTool.Helpers
{
	public class MigrationHelper
	{
		private readonly SqlContext _sqlContext;
		private readonly MongoDbRepository _mongoDbRepository;
		private readonly ProgressContext _progressContext;

		public MigrationHelper(SqlContext sqlContext, MongoDbRepository mongoDbRepository, ProgressContext progressContext)
		{
			_sqlContext = sqlContext;
			_mongoDbRepository = mongoDbRepository;
			_progressContext = progressContext;
		}

		public async Task MigratePosts()
		{
			int lastInsertedId = await GetLastInsertedPostId();
			int maxId = await GetMaxPostId();

			while (lastInsertedId != maxId)
			{
				Console.WriteLine($"Migrating post {lastInsertedId} from {maxId}");
                await new MigratePostsHelper(_sqlContext, _mongoDbRepository, _progressContext).MigratePosts(lastInsertedId);

				lastInsertedId = await GetLastInsertedPostId();
                maxId = await GetMaxPostId();
            }
		}

		private async Task<int> GetLastInsertedPostId()
		{
            int lastInsertedId = _progressContext.PostProgresses.Select(e => e.LastInsertedId).DefaultIfEmpty().Max();
			return lastInsertedId != null ? lastInsertedId : 0;
        }

		private async Task<int> GetMaxPostId()
		{
            return _sqlContext.Posts.Where(e => e.ParentId == null).Max(e => e.Id);
        }
    }
}