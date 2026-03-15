using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers;

public sealed class StatusController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get() =>
        Ok(new
        {
            service = "ChatApp.Api",
            status = "ready"
        });
}
