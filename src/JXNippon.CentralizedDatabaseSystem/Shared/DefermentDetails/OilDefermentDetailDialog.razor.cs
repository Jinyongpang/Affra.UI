using CentralizedDatabaseSystemODataService.Affra.Service.CentralizedDatabaseSystem.Domain.Deferments;
using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.DefermentDetails
{
    public partial class OilDefermentDetailDialog
    {
        [Parameter] public OilDefermentDetail Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }

        private string ddDowntimeCategory = string.Empty;
        private string ddDowntimeType = string.Empty;
        private string ddPrimaryCause = string.Empty;
        private string ddSecondaryCause = string.Empty;
        private string ddStatus = string.Empty;

        private readonly Dictionary<string, string> downtimeCategorydict = new Dictionary<string, string>();
        private readonly Dictionary<string, string> downtimeTypedict = new Dictionary<string, string>();
        private readonly Dictionary<string, string> primaryCausedict = new Dictionary<string, string>();
        private readonly Dictionary<string, string> secondaryCausedict = new Dictionary<string, string>();
        private readonly Dictionary<string, string> statusdict = new Dictionary<string, string>();

        private bool isViewing { get => MenuAction == 3; }
        protected Task SubmitAsync(OilDefermentDetail arg)
        {
            arg.DowntimeCategory = Enum.Parse<DefermentDetailDowntimeCategory>(downtimeCategorydict[ddDowntimeCategory]);
            arg.DowntimeType = Enum.Parse<DefermentDetailDowntimeType>(downtimeTypedict[ddDowntimeType]);
            arg.PrimaryCause = Enum.Parse<DefermentDetailPrimaryCause>(primaryCausedict[ddPrimaryCause]);
            arg.SecondaryCause = Enum.Parse<DefermentDetailSecondaryCause>(secondaryCausedict[ddSecondaryCause]);
            arg.Status = Enum.Parse<DefermentDetailStatus>(statusdict[ddStatus]);

            DialogService.Close(true);
            return Task.CompletedTask;
        }
        protected override Task OnInitializedAsync()
        {
            if (ddDowntimeCategory != null)
            {
                ddDowntimeCategory = stringLocalizer[Item.DowntimeCategory.ToString()];
            }

            if (ddDowntimeType != null)
            {
                ddDowntimeType = stringLocalizer[Item.DowntimeType.ToString()];
            }

            if (ddPrimaryCause != null)
            {
                ddPrimaryCause = stringLocalizer[Item.PrimaryCause.ToString()];
            }

            if (ddSecondaryCause != null)
            {
                ddSecondaryCause = stringLocalizer[Item.SecondaryCause.ToString()];
            }

            if (ddStatus != null)
            {
                ddStatus = stringLocalizer[Item.Status.ToString()];
            }

            return LoadAllDefermentEnum();
        }
        private void Cancel()
        {
            DialogService.Close(false);
        }

        private Task LoadAllDefermentEnum()
        {
            foreach (var val in Enum.GetValues(typeof(DefermentDetailDowntimeCategory)).Cast<DefermentDetailDowntimeCategory>())
            {
                downtimeCategorydict.Add(stringLocalizer[val.ToString()], val.ToString());
            }

            foreach (var val in Enum.GetValues(typeof(DefermentDetailDowntimeType)).Cast<DefermentDetailDowntimeType>())
            {
                downtimeTypedict.Add(stringLocalizer[val.ToString()], val.ToString());
            }

            foreach (var val in Enum.GetValues(typeof(DefermentDetailPrimaryCause)).Cast<DefermentDetailPrimaryCause>())
            {
                primaryCausedict.Add(stringLocalizer[val.ToString()], val.ToString());
            }

            foreach (var val in Enum.GetValues(typeof(DefermentDetailSecondaryCause)).Cast<DefermentDetailSecondaryCause>())
            {
                secondaryCausedict.Add(stringLocalizer[val.ToString()], val.ToString());
            }

            foreach (var val in Enum.GetValues(typeof(DefermentDetailStatus)).Cast<DefermentDetailStatus>())
            {
                statusdict.Add(stringLocalizer[val.ToString()], val.ToString());
            }

            return Task.CompletedTask;
        }
    }
}
