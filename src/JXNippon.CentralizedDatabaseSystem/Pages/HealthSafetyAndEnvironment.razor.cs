using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Shared;
using JXNippon.CentralizedDatabaseSystem.Shared.HealthSafetyAndEnvironment;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class HealthSafetyAndEnvironment
    {
        private HealthSafetyAndEnvironmentDataGrid healthSafetyAndEnvironmentDataGrid;
        private LossOfPrimaryContainmentIncidentDataGrid lossOfPrimaryContainmentIncidentDataGrid;
        private LifeBoatsDataGrid lifeBoatsDataGrid;
        private LongTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid;
        private OperatingChangeDataGrid operatingChangeDataGrid;

        private DateFilterPanel filterPanel;

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            healthSafetyAndEnvironmentDataGrid.CommonFilter = filterPanel.CommonFilter;
            lossOfPrimaryContainmentIncidentDataGrid.CommonFilter = filterPanel.CommonFilter;
            lifeBoatsDataGrid.CommonFilter = filterPanel.CommonFilter;
            longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid.CommonFilter = filterPanel.CommonFilter;
            operatingChangeDataGrid.CommonFilter = filterPanel.CommonFilter;
        }

        private async Task OnChangeAsync(CommonFilter commonFilter)
        {
            await Task.WhenAll(healthSafetyAndEnvironmentDataGrid.ReloadAsync(),
                lossOfPrimaryContainmentIncidentDataGrid.ReloadAsync(),
                lifeBoatsDataGrid.ReloadAsync(),
                longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid.ReloadAsync(),
                operatingChangeDataGrid.ReloadAsync());
        }
        private async Task ReloadAsync()
        {
            await Task.WhenAll(healthSafetyAndEnvironmentDataGrid.ReloadAsync(),
                lossOfPrimaryContainmentIncidentDataGrid.ReloadAsync(),
                lifeBoatsDataGrid.ReloadAsync(),
                longTermOverridesAndInhibitsOnAlarmAndOrTripDataGrid.ReloadAsync(),
                operatingChangeDataGrid.ReloadAsync());
        }
    }
}
