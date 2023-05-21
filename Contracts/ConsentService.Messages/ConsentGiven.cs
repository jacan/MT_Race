namespace ConsentService.Messages
{
    public record PatientConsented
    {
        public Guid PatientId { get; init; }
        public DateTime DateOfConsent { get; init; }
    }
}