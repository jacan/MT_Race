using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PatientService.Messages;

namespace ConsentService.Controllers;

[ApiController]
[Route("[controller]")]
public class ConsentController : ControllerBase
{
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<ConsentController> _logger;

    public ConsentController(IPublishEndpoint publishEndpoint, ILogger<ConsentController> logger)
    {
        _publisher = publishEndpoint;
        _logger = logger;
    }

    [HttpPost]
    public async Task StartResearch()
    {
        await _publisher.Publish<PatientEnlisted>(new PatientEnlisted
        {
            PatientId = Guid.NewGuid(),
        });
    }
}
