using MassTransit;
using ILogger = Serilog.ILogger;

namespace ManagementService.Consumers
{
    public class AnalyticsInitiatedForConsentedPatientConsumerDefinition :
        ConsumerDefinition<AnalyticsInitiatedForConsentedPatientConsumer>
    {
        private readonly ILogger _logger;

        public AnalyticsInitiatedForConsentedPatientConsumerDefinition(ILogger logger)
        {
            _logger = logger;
        }
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<AnalyticsInitiatedForConsentedPatientConsumer> consumerConfigurator)
        {
            _logger.Information("Configuring Delayed Redelivery");
            endpointConfigurator.UseDelayedRedelivery(redeliveryConfig =>
            {
                redeliveryConfig.Interval(2, TimeSpan.FromSeconds(1));
            });
            
            //In-proc retries (This is dangerous!)
            _logger.Information("Configuring Message Retry");
            endpointConfigurator.UseMessageRetry(r =>
            {
                r.Interval(retryCount: 10, interval: TimeSpan.FromSeconds(2));
            });
        }
    }
}