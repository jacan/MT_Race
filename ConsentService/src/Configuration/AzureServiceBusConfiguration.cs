namespace ConsentService.Configuration;

public record AzureServiceBusConfiguration
{
    public string? ConnectionString { get; init; }
}