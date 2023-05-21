using Microsoft.AspNetCore.Mvc;

namespace ManagementService.Controllers;

[ApiController]
[Route("[controller]")]
public class ManagementController : ControllerBase
{
    private readonly ILogger<ManagementController> _logger;

    public ManagementController(ILogger<ManagementController> logger)
    {
        _logger = logger;
    }
}
