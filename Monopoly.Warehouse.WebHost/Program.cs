using Microsoft.EntityFrameworkCore;
using Monopoly.Warehouse.DataAccess;
using Monopoly.Warehouse.WebHost.Extensions;
using Newtonsoft.Json;

namespace Monopoly.Warehouse.WebHost;

public class Program
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        IServiceCollection serviceCollection = builder.Services;
        
        ConfigureServices(serviceCollection, builder.Configuration);

        WebApplication app = builder.Build();

        ConfigureApp(app);

        await app.RunAsync();
    }

    private static void ConfigureApp(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.MapControllers();
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

        services.AddEndpointsApiExplorer();
        services.AddDefaultSwagger();
    }
}