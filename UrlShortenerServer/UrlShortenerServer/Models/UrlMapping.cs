namespace UrlShortenerServer.Models;

public class UrlMapping
{
    public string ShortUrl { get; set; }
    public string LongUrl { get; set; }
    public string UserFingerprint { get; set; }
    public int VisitCount { get; set; }
}