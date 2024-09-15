using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Monopoly.API.Data.Entities;
using Monopoly.API.Services.Boxes;
using Monopoly.API.Services.Interfaces;
using Monopoly.API.Services.Pallets;
using Newtonsoft.Json;

namespace Monopoly.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
#if DEBUG
            await context.Database.EnsureDeletedAsync();
#endif
            bool created = await context.Database.EnsureCreatedAsync();

            if (created)
                await FillDb(context, scope);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }

    private static async Task FillDb(MainDbContext context, IServiceScope scope)
    {
        if (context.Pallets is null)
            throw new NullReferenceException(nameof(context.Pallets));

        if (context.Boxes is null)
            throw new NullReferenceException(nameof(context.Boxes));

        //Seed database
        var bGen = scope.ServiceProvider.GetRequiredService<IDataGenerator<Box>>();
        var pGen = scope.ServiceProvider.GetRequiredService<IDataGenerator<Pallet>>();

        var pallets = pGen.GenerateObjects(100);
        await context.Pallets.AddRangeAsync(await pallets);
        await context.SaveChangesAsync();

        var boxes = await bGen.GenerateObjects(200);
        await context.Boxes.AddRangeAsync(boxes);
        await context.SaveChangesAsync();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
               .AddNewtonsoftJson(op =>
                {
                    // Ignore one-to-many serializer cycles
                    op.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(op =>
        {
            op.SwaggerDoc("v1", new OpenApiInfo
            {
                Version        = "v1",
                Title          = "Монополия API",
                Description    = "API для управления складом - паллетами и коробками",
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

        builder.Services.AddDbContext<MainDbContext>(op =>
        {
            var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");
            if (useInMemory)
                op.UseInMemoryDatabase("MonopolyDb");
            else
                op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            op.LogTo(s =>
            {
                Debug.WriteLine(s);
                //try
                //{
                //    using var writer = File.AppendText("app_log.txt");
                //    writer.WriteLine(s);
                //}
                //catch (Exception ex)
                //{
                //    Debug.Write(ex.Message);
                //}
            });
        });

        builder.Services.AddScoped<IDataGenerator<Box>, BoxesGeneratorService>();
        builder.Services.AddScoped<IDataGenerator<Pallet>, PalletsGeneratorService>();
        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<IDataService<Box>, BoxesService>();
        builder.Services.AddScoped<IDataService<Pallet>, PalletsService>();
    }
}