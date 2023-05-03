using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HAN.ADB.RDT.DataMigrationTool.Configuration;
using Microsoft.Extensions.Configuration;
using HAN.ADB.RDT.DataMigrationTool.Helpers;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.SqlLite;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(app =>
    {
        app.AddJsonFile("appsettings.json");
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddCustomServices(hostContext.Configuration);
        services.AddLogging();
    })
    .Build();


using IServiceScope serviceScope = host.Services.CreateScope();
IServiceProvider provider = serviceScope.ServiceProvider;

var progressContext = provider.GetRequiredService<ProgressContext>();

await progressContext.Database.EnsureCreatedAsync();

var migrationHelper = provider.GetRequiredService<MigrationHelper>();

await migrationHelper.MigratePosts();

await host.RunAsync();