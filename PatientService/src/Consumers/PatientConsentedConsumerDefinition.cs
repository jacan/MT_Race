using MassTransit;

namespace PatientService.Consumers
{
    public class PatientConsentedConsumerDefinition :
        ConsumerDefinition<PatientConsentedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<PatientConsentedConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}