using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using LarpVault.Data;   // Change this if your namespace is different

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()   // Required because you have Http.AspNetCore package
    .ConfigureServices(services =>
    {
        // Production monitoring - Application Insights (very important for the job)
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // DbContext for Azure SQL
        services.AddDbContext<LarpVaultDbContext>(options =>
        {
            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            if (!string.IsNullOrEmpty(connectionString))
            {
                options.UseSqlServer(connectionString);
            }
        });
    })
    .Build();

host.Run();