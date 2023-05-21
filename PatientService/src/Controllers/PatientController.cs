using Microsoft.AspNetCore.Mvc;

namespace PatientService.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{
    private readonly ILogger<PatientController> _logger;

    public PatientController(ILogger<PatientController> logger)
    {
        _logger = logger;
    }
}
