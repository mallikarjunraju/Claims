using Claims.Application;
using Claims.Application.UseCases.Audit;
using Claims.Infrastructure.DbContexts;
using Claims.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Claims.Api.Extensions;

public static class ServiceCollectionX
{
    public static void AddClaimApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuditContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddDbContext<InsuranceContext>(
            options =>
            {
                var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
                var database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
                options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
            }
        );

        services.AddScoped<IClaimsRepository, ClaimsRepository>();
        services.AddScoped<ICoversRepository, CoversRepository>();
        services.AddScoped<IAuditer, Auditer>();
        services.AddMediatR(typeof(UseCasesAssemblyMarker));
    }
}
