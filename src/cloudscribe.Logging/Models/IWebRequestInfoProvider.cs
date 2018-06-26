namespace cloudscribe.Logging.Models
{
    public interface IWebRequestInfoProvider
    {
        string GetIpAddress(bool tryUseXForwardHeader = true);
        string GetRequestUrl();
        
    }
}
