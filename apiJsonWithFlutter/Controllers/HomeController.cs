using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apiJsonWithFlutter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
