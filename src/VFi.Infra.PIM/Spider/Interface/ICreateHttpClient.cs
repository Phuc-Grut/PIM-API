using VFi.Domain.PIM.Models;
namespace VFi.Infra.PIM.Spider.Interface
{
    public interface ICreateHttpClient
    {
        HttpClient Create(Proxy? proxyInfo);
    }
}