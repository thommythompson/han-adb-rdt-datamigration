using System;
namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MongoDb
{
	public interface IMongoDbRepository
	{
        public Task InsertPosts(string json);
        public Task<int> GetMaxPostId();
        public Task InsertUsers(string json);
        public Task<int> GetMaxUserId();
    }
}

