using Microsoft.AspNetCore.Mvc;

namespace UrlShortenerServer.Controllers;

[ApiController]
[Route("")]
public class IndexController : ControllerBase
{
    [HttpGet]
    public object GetRealUrl(string shortenedUrl)
    {
        return null!;
    }
}