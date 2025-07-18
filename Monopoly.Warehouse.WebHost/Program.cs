using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;
using Monopoly.Warehouse.DataAccess;
using Monopoly.Warehouse.WebHost.Extensions;
using Monopoly.Warehouse.WebHost.Models.Box;
using Monopoly.Warehouse.WebHost.Models.Pallet;
using Monopoly.Warehouse.WebHost.Validation;
using Newtonsoft.Json;

namespace Monopoly.Warehouse.WebHost;

public class Program
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        IServiceCollection serviceCollection = builder.Services;
        
        ConfigureServices(serviceCollection, builder.Configuration);

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
                .AddNewtonsoftJson(op =>
                 {
                     // Ignore one-to-many serializer cycles
                     op.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 });

        services.AddAutoMapper(typeof(Program));

        services.AddDbContext<DataContext>(op =>
        {
            op.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            op.LogTo(message => Console.WriteLine(message, LogLevel.Debug));
        });
        services.AddRepositories(op =>
        {
            op.UseInMemoryDatabase = configuration.GetValue<bool>("UseInMemoryDatabase");
        });

        // services.AddMemoryCache();
        // services.AddLazyCache();
        
        services.AddEndpointsApiExplorer();
        services.AddDefaultSwagger();

        services.AddScoped<IValidator<BoxCreateOrUpdate>, BoxCreateOrUpdateValidator>();
        services.AddScoped<IValidator<PalletCreateOrUpdate>, PalletCreateOrUpdateValidator>();
    }
}