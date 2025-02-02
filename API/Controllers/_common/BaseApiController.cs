using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers._common
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }
}
