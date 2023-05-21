namespace ManagementService.Messages;

public record AnalyticsInitiatedForConsentedPatient
{
    public Guid PatientId { get; init; }
    public string[] Experiments { get; init; }
}
