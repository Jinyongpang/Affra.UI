using Microsoft.Extensions.Options;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.Reports
{
    public class ReportAPIClient : Domain.ReportAPI.ReportAPIClient
    {
        public ReportAPIClient(HttpClient httpClient, IOptions<ReportAPIClientConfigurations> aPIConfigurations) : base(httpClient)
        {
            BaseUrl = aPIConfigurations.Value.Url;
        }
    }
}
