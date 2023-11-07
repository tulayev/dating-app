using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected ActionResult HandleResponse<T>(ApiResponse<T> response)
        {
            if (response == null || (response.Success && response.Data == null))
                return NotFound();

            if (response.Success && response.Data != null)
                return Ok(response.Data);

            return BadRequest(response.ErrorMessage);
        }
    }
}
