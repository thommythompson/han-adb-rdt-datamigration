using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using HAN.ADB.RDT.DataMigrationTool.Core;
using System.Collections;
using ThirdParty.Json.LitJson;

namespace HAN.ADB.RDT.DataMigrationTool.DataAccess.MongoDb
{
    public class MongoDbRepository : IMongoDbRepository
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

        public async Task<int> GetMaxPostId()
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);

            var collection = database.GetCollection<Post>("posts");

            var options = new FindOptions<Post, Post>
            {
                Limit = 1,
                Sort = Builders<Post>.Sort.Descending(p => p.Id)
            };

            var result = (await collection.FindAsync(FilterDefinition<Post>.Empty, options)).FirstOrDefault();

            if (null != result)
                return result.Id;

            return default(int);
        }

        public async Task<int> GetMaxUserId()
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);

            var collection = database.GetCollection<User>("users");

            var options = new FindOptions<User, User>
            {
                Limit = 1,
                Sort = Builders<User>.Sort.Descending(p => p.Id)
            };

            var result = (await collection.FindAsync(FilterDefinition<User>.Empty, options)).FirstOrDefault();

            if (null != result)
                return result.Id;

            return default(int);
        }

        public async Task InsertPosts(string json) => await InsertJson(json, "posts");

        public async Task InsertUsers(string json) => await InsertJson(json, "users");

        private async Task InsertJson(string json, string collectionName)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);

            var collection = database.GetCollection<BsonDocument>(collectionName);

            List<BsonDocument> documents = BsonSerializer.Deserialize<BsonArray>(json).Select(p => p.AsBsonDocument).ToList<BsonDocument>();

            await collection.InsertManyAsync(documents);
        }

        private void RegisterSerializer()
        {
            var objectSerializer = new ObjectSerializer(type => ObjectSerializer.DefaultAllowedTypes(type) || type.FullName.StartsWith("HAN.ADB.RDT.DataMigrationTool"));
            BsonSerializer.RegisterSerializer(objectSerializer);
        }

        private void RegisterBsonClasses()
        {
            BsonClassMap.RegisterClassMap<Post>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._id);
            });

            BsonClassMap.RegisterClassMap<PostRef>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._id);
            });

            BsonClassMap.RegisterClassMap<PostComment>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._id);
            });

            BsonClassMap.RegisterClassMap<PostVote>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._id);
            });

            BsonClassMap.RegisterClassMap<UserComment>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._id);
            });

            BsonClassMap.RegisterClassMap<UserVote>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._id);
            });

            BsonClassMap.RegisterClassMap<Answer>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._id);
            });

            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._id);
            });

            BsonClassMap.RegisterClassMap<UserRef>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._id);
            });

            BsonClassMap.RegisterClassMap<Badge>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._id);
            });

            BsonClassMap.RegisterClassMap<QuestionRef>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._id);
            });

            BsonClassMap.RegisterClassMap<AnswerRef>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c._id);
            });
        }
    }
}
