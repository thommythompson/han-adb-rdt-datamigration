using HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql;
using HAN.ADB.RDT.DataMigrationTool.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.ADB.RDT.DataMigrationTool.Configuration
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<SqlContext>(
				options =>
					options.UseSqlServer(
						configuration.GetConnectionString("Sql")
                    )
			);

			services.AddTransient<IMigrationHelper, MigrationHelper>();

            return services;
		}
	}
}

