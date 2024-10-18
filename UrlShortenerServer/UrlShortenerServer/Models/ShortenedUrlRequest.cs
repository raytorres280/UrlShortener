namespace UrlShortenerServer.Models;

public class ShortenedUrlRequest
{
    public string Url { get; set; }
    public string userFingerprint { get; set; }
}