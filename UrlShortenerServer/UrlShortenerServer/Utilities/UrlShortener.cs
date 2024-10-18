using UrlShortenerServer.Models;

namespace UrlShortenerServer.Utilities;

public interface IUrlShortener
{
    public string ShortenUrl(string longUrl, string userFingerprint);
    public string GetOriginalUrl(string shortUrl);
    public List<UrlMapping> GetUserShortUrls(string userId);
    public bool DeleteUrl(string shortUrl);
    void IncrementVisitCount(string shortUrl);
}
public class UrlShortener : IUrlShortener
{
    private const string BaseCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int Base = 62;
    private readonly Dictionary<string, (string, string, int)> _shortToLongMap = new();
    // private readonly Dictionary<long, string> _urlMapping = new();
    private readonly Dictionary<string, List<string>> _userShortUrls = new();
    private long _idCounter = 1;

    // Encode a number into a base-62 string
    private static string Encode(long number)
    {
        var shortUrl = new Stack<char>();
        while (number > 0)
        {
            shortUrl.Push(BaseCharacters[(int)(number % Base)]);
            number /= Base;
        }

        return new string(shortUrl.ToArray());
    }

    // Decode a base-62 string into a number
    private static long Decode(string shortUrl)
    {
        long number = 0;
        foreach (var c in shortUrl) number = number * Base + BaseCharacters.IndexOf(c);
        return number;
    }

    // Shorten the given URL
    public string ShortenUrl(string longUrl, string userFingerprint)
    {
        var shortUrl = Encode(_idCounter);
        // _urlMapping[_idCounter] = longUrl;
        _shortToLongMap[shortUrl] = (longUrl, userFingerprint, 0);
        if(_userShortUrls.TryGetValue(userFingerprint, out var value))
        {
            value.Add(shortUrl);
        }
        else
        {
            _userShortUrls[userFingerprint] = [shortUrl];
        }
        _idCounter++;
        return shortUrl;
    }

    // Retrieve the original URL from the short URL
    public string GetOriginalUrl(string shortUrl)
    {
        return _shortToLongMap.ContainsKey(shortUrl) ? _shortToLongMap[shortUrl].Item1 : null;
    }

    public List<UrlMapping> GetUserShortUrls(string userId)
    {
        var urlsForUser = _userShortUrls.TryGetValue(userId, out var urlList) ? urlList : new List<string>();
        var listOfMappings = new List<UrlMapping>();
        foreach (var url in urlsForUser)
        {
            listOfMappings.Add(new UrlMapping
            {
                ShortUrl = url,
                LongUrl = _shortToLongMap.TryGetValue(url, out var tuple) ? tuple.Item1 : null,
                UserFingerprint = userId,
                VisitCount = tuple.Item3,
            });
        }

        return listOfMappings;
    }

    public bool DeleteUrl(string shortUrl)
    {
        try
        {
            _shortToLongMap.TryGetValue(shortUrl, out var tuple);
            _shortToLongMap.Remove(shortUrl);
            _userShortUrls[tuple.Item2].Remove(shortUrl);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }

    public void IncrementVisitCount(string shortUrl)
    {
        var shortToLong = _shortToLongMap[shortUrl];
        shortToLong.Item3 += 1;
        _shortToLongMap[shortUrl] = shortToLong;
    }
}