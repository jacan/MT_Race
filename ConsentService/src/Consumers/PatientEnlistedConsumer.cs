using ConsentService.Messages;
using MassTransit;
using PatientService.Messages;

namespace ConsentService.Consumers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PatientEnlistedConsumer :
        IConsumer<PatientEnlisted>
    {
        public async Task Consume(ConsumeContext<PatientEnlisted> context)
        {
            await context.Publish<PatientConsented>(new()
            {
                PatientId = context.Message.PatientId,
                DateOfConsent = DateTime.UtcNow,
            });
        }
    }
}