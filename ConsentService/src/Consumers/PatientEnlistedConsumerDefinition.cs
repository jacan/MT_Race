using MassTransit;

namespace ConsentService.Consumers
{
    public class PatientEnlistedConsumerDefinition :
        ConsumerDefinition<PatientEnlistedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<PatientEnlistedConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}