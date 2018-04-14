namespace cloudscribe.Logging.Models
{
    public interface IWebRequestInfoProvider
    {
        string GetIpAddress();
        string GetRequestUrl();
        
    }
}
