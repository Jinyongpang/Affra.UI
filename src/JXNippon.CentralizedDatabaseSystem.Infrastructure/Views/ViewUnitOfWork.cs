using Affra.Core.Infrastructure.OData.UnitOfWorks;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.Extensions.Options;
using Microsoft.OData.Extensions.Client;
using ViewODataService.Default;

namespace JXNippon.CentralizedDatabaseSystem.Infrastructure.Views
{
    public class ViewUnitOfWork : ODataUnitOfWorkBase, IViewUnitOfWork
    {
        public ViewUnitOfWork(IODataClientFactory oDataClientFactory, IOptions<ViewConfigurations> viewConfigurations)
            : base(oDataClientFactory.CreateClient<Container>(new Uri(viewConfigurations.Value.Url), nameof(ViewUnitOfWork)))
        {
        }
    }
}