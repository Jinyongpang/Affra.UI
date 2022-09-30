using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using Microsoft.OData.Client;

namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SalesMY
{
    public partial class MonthlyHIPSale : IMonthly
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.SalesMY
{
    public partial class MonthlyFPSOSale : IMonthly
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellTests
{
    public partial class MonthlyWellTest : IMonthly
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.WellProductions
{
    public partial class MonthlyWellProduction : IMonthly
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Reservoir
{
    public partial class MonthlyReservoir : IMonthly
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.FieldMY
{
    public partial class MonthlyFPSOFieldMY : IMonthly
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}
namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.FieldMY
{
    public partial class MonthlyHIPFieldMY : IMonthly
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    }
}

namespace CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Deferments 
{ 
    public partial class MonthlyGasProductionDeliverySchedule : IMonthly 
    {
        [IgnoreClientProperty]
        public DateTime DateUI
        {
            get { return this.Date.ToLocalDateTime(); }
            set { this.Date = value.ToUniversalTime(); }
        }
    } 
}