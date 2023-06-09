﻿using HAN.ADB.RDT.DataMigrationTool.DataAccess.MongoDb;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql;
using Microsoft.Extensions.Logging;

namespace HAN.ADB.RDT.DataMigrationTool.Helpers.Extensions
{
    public class MigratePostsHelper
    {
        private readonly ISqlRepository _sqlRepository;
        private readonly IMongoDbRepository _mongoDbRepository;
        private readonly ILogger<MigratePostsHelper> _logger;

        private int MaxPostIdInSql { get; set; }
        private int MaxPostIdInMongoDb { get; set; }

        public MigratePostsHelper(ISqlRepository sqlRepository, IMongoDbRepository mongoDbRepository, ILogger<MigratePostsHelper> logger)
        {
            _sqlRepository = sqlRepository;
            _mongoDbRepository = mongoDbRepository;
            _logger = logger;
        }

        public async Task MigratePosts()
        {
            await UpdateMaxIds();

            while (MaxPostIdInSql != MaxPostIdInMongoDb)
            {
                string json;
                try
                {
                    json = await _sqlRepository.GetPosts(MaxPostIdInMongoDb);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Failed to fetch posts. Exception: {ex}");
                    throw;
                }

                try
                {
                    await _mongoDbRepository.InsertPosts(json);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Failed to insert posts. Exception: {ex}");
                    throw;
                }

                await UpdateMaxIds();
            }
        }

        private async Task UpdateMaxIds()
        {
            try
            {
                MaxPostIdInMongoDb = await _mongoDbRepository.GetMaxPostId();
                MaxPostIdInSql = await _sqlRepository.GetMaxPostId();

                _logger.LogInformation($"Maximum MongoDb post id: {MaxPostIdInMongoDb}, maximum SQL Server post id {MaxPostIdInSql}.");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to update max post id's. Exception: {ex}");
                throw;
            }
        }
    }
}