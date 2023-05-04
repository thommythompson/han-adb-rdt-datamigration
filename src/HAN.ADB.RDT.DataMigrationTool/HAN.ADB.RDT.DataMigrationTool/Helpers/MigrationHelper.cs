using HAN.ADB.RDT.DataMigrationTool.DataAccess.MongoDb;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql;
using HAN.ADB.RDT.DataMigrationTool.Helpers.Extensions;

namespace HAN.ADB.RDT.DataMigrationTool.Helpers
{
	public class MigrationHelper
	{
        private readonly MigratePostsHelper _migratePostsHelper;
        private readonly MigrateUsersHelper _migrateUsersHelper;

        public MigrationHelper(MigratePostsHelper migratePostsHelper, MigrateUsersHelper migrateUsersHelper)
		{
			_migratePostsHelper = migratePostsHelper;
			_migrateUsersHelper = migrateUsersHelper;
		}

		public async Task MigratePosts()
		{
			await _migratePostsHelper.MigratePosts();
		}

        public async Task MigrateUsers()
        {
            await _migrateUsersHelper.MigrateUsers();
        }
    }
}