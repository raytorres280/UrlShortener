using Microsoft.AspNetCore.Mvc;
using UrlShortenerServer.Models;
using UrlShortenerServer.Services;

namespace UrlShortenerServer.Controllers;

[ApiController]
[Route("url")]
public class UrlController(IUrlService urlService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetUserUrls([FromQuery] GetUserUrlRequest request)
    {
        var urls = urlService.LoadUserUrls(request.UserFingerprint);
        return Ok(urls);
    }

    [HttpGet("{shortUrl}")]
    public IActionResult GetLongUrl([FromRoute] string shortUrl)
    {
        var longUrl = urlService.GetRealUrl(shortUrl);
        return Ok(longUrl);
    }

    [HttpPut("{shortUrl}")]
    public IActionResult IncrementVisitCount([FromRoute] string shortUrl)
    {
        urlService.IncrementVisitCount(shortUrl);
        return Ok();
    }
    
    [HttpPost]
    public IActionResult CreateShortenedUrl([FromBody] ShortenedUrlRequest request)
    {
        var shortenedUrl = urlService.CreateShortenedUrl(request.Url, request.userFingerprint);
        return Ok(shortenedUrl);
    }

    [HttpDelete("{shortUrl}")]
    public IActionResult DeleteShortenedUrl([FromRoute] string shortUrl)
    {
        var success = urlService.DeleteShortenedUrl(shortUrl);
        return Ok(success);
    }
}