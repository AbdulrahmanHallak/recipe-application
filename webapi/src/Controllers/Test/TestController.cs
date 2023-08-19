using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers.Test;

[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("Test")]
    public string Test() => "This is from the test controller";
}
