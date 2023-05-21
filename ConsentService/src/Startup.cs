using ConsentService.Configuration;
using ConsentService.Consumers;
using ConsentService.Messages;
using MassTransit;
using Microsoft.Extensions.Azure;
using Serilog;

namespace ConsentService;

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
            
            mtConfig.AddConsumersFromNamespaceContaining<PatientEnlistedConsumer>();
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
            app.UseSwaggerUI(swaggerUi =>
            {
                swaggerUi.SwaggerEndpoint("/swagger/v1/swagger.json", "Consent Service");
                swaggerUi.RoutePrefix = string.Empty;
            });
        }

        app.UseRouting();
        app.MapControllers();
        app.UseAuthorization();
    }
}