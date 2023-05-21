namespace PatientService.Messages;
public record PatientEnlisted
{
    public Guid PatientId { get; init; }
}
