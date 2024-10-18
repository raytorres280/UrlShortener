using UrlShortenerServer.Models;
using UrlShortenerServer.Utilities;

namespace UrlShortenerServer.Services;

public interface IUrlService
{
    public UrlMapping CreateShortenedUrl(string url, string userFingerprint);
    public bool DeleteShortenedUrl(string shortenedUrl);
    public string GetRealUrl(string shortenedUrl);
    public int GetShortenedUrlVisitCount(string shortenedUrl);
    public List<UrlMapping> LoadUserUrls(string userId);
    void IncrementVisitCount(string shortUrl);
}

public class UrlService(IUrlShortener urlShortener) : IUrlService
{
    public UrlMapping CreateShortenedUrl(string url, string userFingerprint)
    {
        var shortenedUrl = urlShortener.ShortenUrl(url, userFingerprint);
        var mapping = new UrlMapping
        {
            ShortUrl = shortenedUrl,
            LongUrl = url,
            UserFingerprint = userFingerprint
        };
        return mapping;
    }

    public bool DeleteShortenedUrl(string shortenedUrl)
    {
        return urlShortener.DeleteUrl(shortenedUrl);
    }

    public string GetRealUrl(string shortenedUrl)
    {
        var originalUrl = urlShortener.GetOriginalUrl(shortenedUrl);
        return originalUrl;
    }

    public int GetShortenedUrlVisitCount(string shortenedUrl)
    {
        throw new NotImplementedException();
    }

    public List<UrlMapping> LoadUserUrls(string userId)
    {
        return urlShortener.GetUserShortUrls(userId);
    }

    public void IncrementVisitCount(string shortUrl)
    {
        urlShortener.IncrementVisitCount(shortUrl);
    }
}