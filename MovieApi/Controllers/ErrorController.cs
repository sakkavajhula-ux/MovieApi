using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]//This is done for testing purpose and will be hidden in swagger.
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("/error")]
        public IActionResult Error()
        {
            return Problem(
                title: "An unexpected error occurred!",
                statusCode: StatusCodes.Status500InternalServerError
                );
        }
    }
}
