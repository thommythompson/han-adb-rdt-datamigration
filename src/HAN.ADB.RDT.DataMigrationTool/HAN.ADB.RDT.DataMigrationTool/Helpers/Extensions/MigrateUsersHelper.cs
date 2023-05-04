using System;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.MongoDb;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql;
using Microsoft.Extensions.Logging;

namespace HAN.ADB.RDT.DataMigrationTool.Helpers.Extensions
{
	public class MigrateUsersHelper
	{
        private readonly ISqlRepository _sqlRepository;
        private readonly IMongoDbRepository _mongoDbRepository;
        private readonly ILogger<MigratePostsHelper> _logger;

        private int MaxUserIdInSql { get; set; }
        private int MaxUserIdInMongoDb { get; set; }

        public MigrateUsersHelper(ISqlRepository sqlRepository, IMongoDbRepository mongoDbRepository, ILogger<MigratePostsHelper> logger)
        {
            _sqlRepository = sqlRepository;
            _mongoDbRepository = mongoDbRepository;
            _logger = logger;
        }

        public async Task MigrateUsers()
        {
            await UpdateMaxIds();

            while (MaxUserIdInSql != MaxUserIdInMongoDb)
            {
                string json;
                try
                {
                    json = await _sqlRepository.GetUsers(MaxUserIdInMongoDb);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Failed to fetch users. Exception: {ex}");
                    throw;
                }

                try
                {
                    await _mongoDbRepository.InsertUsers(json);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Failed to insert users. Exception: {ex}");
                    throw;
                }

                await UpdateMaxIds();
            }
        }

        private async Task UpdateMaxIds()
        {
            try
            {
                MaxUserIdInMongoDb = await _mongoDbRepository.GetMaxUserId();
                MaxUserIdInSql = await _sqlRepository.GetMaxUserId();

                _logger.LogInformation($"Maximum MongoDb user id: {MaxUserIdInMongoDb}, maximum SQL Server user id {MaxUserIdInSql}.");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to update max user id's. Exception: {ex}");
                throw;
            }
        }
    }
}

