using ManagementService.Configuration;
using ManagementService.Consumers;
using MassTransit;
using Serilog;

namespace ManagementService;

public class Startup
{
    public IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void RegisterServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var azureServiceBusConfiguration = Configuration.GetSection(nameof(AzureServiceBusConfiguration)).Get<AzureServiceBusConfiguration>();
        services.AddMassTransit(mtConfig =>
        {
            mtConfig.SetKebabCaseEndpointNameFormatter();
            mtConfig.AddHealthChecks();
            mtConfig.AddSerilog(x =>
            {
                x.WriteTo.Console();
                x.MinimumLevel.Debug();
            });
            
            mtConfig.AddConsumer<AnalyticsInitiatedForConsentedPatientConsumer, AnalyticsInitiatedForConsentedPatientConsumerDefinition>();
            mtConfig.UsingAzureServiceBus((registrationContext, asbBus) =>
            {
                asbBus.Host(azureServiceBusConfiguration.ConnectionString);
                asbBus.AutoStart = true;
                asbBus.UseServiceBusMessageScheduler();
                asbBus.SetNamespaceSeparatorTo("_");
                asbBus.ConfigureEndpoints(registrationContext);
            });
            
            // mtConfig.UsingRabbitMq( (registrationContext, rabbitBus) =>
            // {
            //     rabbitBus.Host("localhost", "/", host =>
            //     {
            //         host.Username("guest");
            //         host.Password("guest");
            //     } );
            //     
            //     rabbitBus.ConfigureEndpoints(registrationContext);
            // });
        });
    }   

    public void SetupMiddleware(WebApplication app, IWebHostEnvironment env)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
        app.UseHttpsRedirection();
        app.UseAuthorization();
    }
}