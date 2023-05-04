using System;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql
{
	public interface ISqlRepository
	{
        public Task<string> GetPosts(int startId);
        public Task<int> GetMaxPostId();
        public Task<string> GetUsers(int startId);
        public Task<int> GetMaxUserId();
    }
}

