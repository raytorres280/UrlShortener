namespace UrlShortenerServer.Models;

public class DeleteUrlRequest
{
    public string ShortenedUrl { get; set; }
    public string UserFingerprint { get; set; }
}