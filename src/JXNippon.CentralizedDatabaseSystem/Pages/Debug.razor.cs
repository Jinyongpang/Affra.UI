using System.Linq.Dynamic.Core;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Logistics;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.Charts;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.OData.Client;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class Debug
    {
        [Inject] private IViewService ViewService { get; set; }
        private GenericDataGrid<DailyLogistic> dataGrid;

        [Parameter] public IQueryable<dynamic> Queryable { get; set; }
        [Parameter] public string TType { get; set; }

        [Inject] private IServiceProvider ServiceProvider { get; set; }
        private Eval Eval { get; set; } = new Eval();
        private IEnumerable<ChartSeries> chartSeries = new List<ChartSeries>()
        {
            new ()
            {
                Title = "Today's Level",
                CategoryProperty = "DateUI",
                LineType = "Dashed",
                MarkerType = "Circle",
                Smooth = false,
                ValueProperty = "TodayLevel"
            },
        };
        private async Task LoadDataAsync(LoadDataArgs args)
        {
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await dataGrid.ReloadAsync();
        }

        private async Task OnClickAsync(object args)
        {
            TType = ViewService.GetTypeMapping()["DailyLogistic"];
            using var serviceScope = ServiceProvider.CreateScope();
            var s = (DataServiceQuery)serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<DailyLogistic, ICentralizedDatabaseSystemUnitOfWork>>().Get();
            s = (DataServiceQuery)s.Where(Eval.Expression)
                .GroupBy("")
                .Select("");
            // var a = (DataServiceQuery)s.Get().Execute<IQueryable<DailyLogistic>>(Eval.Expression);
            var response = await s
              .ToQueryOperationResponseAsync<DailyLogistic>();
            
           
            var service = this.ViewService.GetGenericService(serviceScope, TType);
            Queryable = service.Get();
            var q = (DataServiceQuery)Queryable;

            var items = (await q.ExecuteAsync());

        }
    }
}
