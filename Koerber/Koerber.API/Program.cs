using Koerber.API.Contracts;
using Koerber.API.Services;
using Koerber.Configurations;
using Koerber.DB;
using Microsoft.EntityFrameworkCore;

public static class WebApplicationExtensionMethods
{
    #region Public Methods

    public static void AutomateDataLoad(this WebApplicationBuilder builder)
    {
        var dataLoaderOptions = builder.Configuration.GetSection("DataLoaderOptions").Get<DataLoaderOptions>();

        ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();

        builder.Services.AddScoped<TaxiTripsContext>();

        TaxiTripsContext? taxiTripsContext = serviceProvider.GetService<TaxiTripsContext>();

        if (taxiTripsContext != null)
        {
            try
            {
                new ZonesServices(taxiTripsContext, dataLoaderOptions).Synchronize();

                new GreenTaxiServices(taxiTripsContext, dataLoaderOptions).Synchronize();

                new YellowTaxiServices(taxiTripsContext, dataLoaderOptions).Synchronize();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to load Trips Data. Exception: {ex}");
            }
        }
    }

    #endregion Public Methods
}

internal class Program
{
    #region Private Methods

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        ConfigureServices(builder.Services);

        string? loadDataArg = Environment.GetEnvironmentVariable("LOAD_DATA");
        
        if (!String.IsNullOrWhiteSpace(loadDataArg) && loadDataArg.Equals("YES"))
        {
            builder.AutomateDataLoad();
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

        void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.Configure<DataLoaderOptions>(
                builder.Configuration.GetSection("DataLoaderOptions")
            );

            var databaseOptions = builder.Configuration.GetSection("DatabaseOptions").Get<DatabaseOptions>();

            services.AddDbContext<TaxiTripsContext>(options =>
                options.UseSqlServer(databaseOptions.ConnectionString, b => b.MigrationsAssembly("Koerber.API"))
            );

            services.AddTransient<IKoerberServices, KoerberServices>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }

    #endregion Private Methods
}