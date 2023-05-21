using System.Security.Cryptography.X509Certificates;
using ManagementService.Messages;
using MassTransit;
using MassTransit.Transports;
using PatientService.Messages;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ManagementService.Consumers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AnalyticsInitiatedForConsentedPatientConsumer :
        IConsumer<AnalyticsInitiatedForConsentedPatient>
    {
        private readonly ILogger _logger;

        public AnalyticsInitiatedForConsentedPatientConsumer(ILogger logger)
        {
            _logger = logger;
        }
        
        private static int _forceExceptionCount = 1;
        public async Task Consume(ConsumeContext<AnalyticsInitiatedForConsentedPatient> context)
        {
            _logger.Information("Retry attempt: {RetryAttempt} - Redelivery attempt {RedeliveryCount}", context.GetRetryAttempt(), context.GetRedeliveryCount());
            
            if (_forceExceptionCount > 0)
            {
                _forceExceptionCount--;
                _logger.Information("Throwing an exception");
                throw new NullReferenceException("Something went wrong, consumer faulted!");
            }

            //await Task.Delay(500);

            var correlationId= context.Headers.GetCorrelationId();
            
            await context.Publish(new PatientEnlisted
            {
                PatientId = context.Message.PatientId,
            });
        }
    }
}