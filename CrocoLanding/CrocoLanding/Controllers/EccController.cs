using Croco.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrocoLanding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EccController : ControllerBase
    {
        [HttpPost("SendCallBackRequest")]
        public BaseApiResponse SendCallBackRequest([FromForm]CreateCallBackModel model)
        {
            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            return new BaseApiResponse(true, "Ok");
        }
    }
}