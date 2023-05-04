using HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql;
using HAN.ADB.RDT.DataMigrationTool.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.MongoDb;
using HAN.ADB.RDT.DataMigrationTool.Helpers.Extensions;

namespace HAN.ADB.RDT.DataMigrationTool.Configuration
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<ISqlRepository>(x => new SqlRepository(configuration.GetConnectionString("Sql")));

            services.AddSingleton<IMongoDbRepository>(x => new MongoDbRepository(configuration.GetConnectionString("MongoDb")));

			services.AddTransient<MigratePostsHelper>();
            services.AddTransient<MigrateUsersHelper>();
            services.AddTransient<MigrationHelper>();

            return services;
		}
	}
}

