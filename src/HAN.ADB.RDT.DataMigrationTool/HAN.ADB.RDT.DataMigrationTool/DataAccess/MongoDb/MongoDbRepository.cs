using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MongoDb
{
	public class MongoDbRepository
	{
		private readonly string _connectionString;
		private readonly string _databaseName;

		public MongoDbRepository(string connectionString)
		{
			_connectionString = connectionString;
            _databaseName = connectionString.Split("//")[1].Split("/")[1];

            RegisterSerializer();
            RegisterBsonClasses();
        }

        public async Task InsertPosts(IEnumerable<Core.MongoDb.Post> posts)
		{
            await AddToCollection("posts", posts);
        }

        public async Task InsertUsers(IEnumerable<Core.MongoDb.Post> users)
        {
            await AddToCollection("users", users);
        }

        private async Task AddToCollection(string collectionName, IEnumerable<object> objects)
		{
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);

            var collection = database.GetCollection<BsonDocument>(collectionName);

            IEnumerable<BsonDocument> bsonDocs = new List<BsonDocument>();
            bsonDocs = objects.Select(e => e.ToBsonDocument());

            await collection.InsertManyAsync(bsonDocs);
        }

        private void RegisterSerializer()
        {
            var objectSerializer = new ObjectSerializer(type => ObjectSerializer.DefaultAllowedTypes(type) || type.FullName.StartsWith("HAN.ADB.RDT.DataMigrationTool"));
            BsonSerializer.RegisterSerializer(objectSerializer);
        }

        private void RegisterBsonClasses()
        {
            BsonClassMap.RegisterClassMap<Core.MongoDb.Post>(cm => {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            });

            BsonClassMap.RegisterClassMap<Core.MongoDb.PostRef>(cm => {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            });

            BsonClassMap.RegisterClassMap<Core.MongoDb.User>(cm => {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            });

            BsonClassMap.RegisterClassMap<Core.MongoDb.UserRef>(cm => {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            });

            BsonClassMap.RegisterClassMap<Core.MongoDb.Comment>(cm => {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            });

            BsonClassMap.RegisterClassMap<Core.MongoDb.Vote>(cm => {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            });

            BsonClassMap.RegisterClassMap<Core.MongoDb.Badge>(cm => {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
            });
        }
	}
}

