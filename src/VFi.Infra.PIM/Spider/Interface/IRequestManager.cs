namespace VFi.Infra.PIM.Spider.Interface
{

    public interface IRequestManager
    {
        
        Task LoadProxy(); 
        Task Downtime(string? proxyKey); 
        SpiderLeg GetSpiderLeg();
        List<SpiderLeg> GetSpiderLegs();
        MercariToken MercariToken { get;}
    }
}