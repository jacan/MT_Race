using ConsentService.Messages;
using ManagementService.Messages;
using MassTransit;

namespace PatientService.Consumers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PatientConsentedConsumer :
        IConsumer<PatientConsented>
    {
        public async Task Consume(ConsumeContext<PatientConsented> context)
        {
            //await Task.Delay(1000);

            await context.Publish<AnalyticsInitiatedForConsentedPatient>(new AnalyticsInitiatedForConsentedPatient()
            {
                PatientId = context.Message.PatientId,
                Experiments = new[] { "WAS", "CAS", "WOA", "LPA" },
            });
        }
    }
}