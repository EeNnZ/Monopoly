using System.Reflection;
using Microsoft.OpenApi.Models;
using Monopoly.Warehouse.Core.Abstractions.Repositories;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;
using Monopoly.Warehouse.DataAccess.Data;
using Monopoly.Warehouse.DataAccess.Repositories;
using Monopoly.Warehouse.WebHost.Options;
using Monopoly.Warehouse.WebHost.Properties;

namespace Monopoly.Warehouse.WebHost.Extensions;



public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureOptions"></param>
    public static IServiceCollection AddRepositories(this IServiceCollection services, Action<RepoOptions> configureOptions)
    {
        var options = new RepoOptions();
        configureOptions(options);
        
        if (options.UseInMemoryDatabase)
        {
            var data = Task.Run(async () => await FakeDataFactory.GetBoxesAndPalletsAsync());
            services.AddScoped<IRepository<Box>, InMemoryRepository<Box>>(_ => new InMemoryRepository<Box>(data.Result.boxes));
            services.AddScoped<IRepository<Pallet>, InMemoryRepository<Pallet>>(_ => new InMemoryRepository<Pallet>(data.Result.pallets));
        }
        else
        {
            services.AddScoped<IBoxesRepository, BoxesEfRepository>();
            services.AddScoped<IPalletsRepository, PalletsEfRepository>();   
        }
        
        return services;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public static void AddDefaultSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(op =>
        {
            op.SwaggerDoc("v1", new OpenApiInfo
            {
                Version        = "v1",
                Title          = $"{Resources.MonopolyName} API",
                Description    = Resources.ApiDescription,
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url  = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url  = new Uri("https://example.com/license")
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            op.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }
}