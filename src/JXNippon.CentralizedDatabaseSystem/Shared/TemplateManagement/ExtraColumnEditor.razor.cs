using System.Text.Json;
using JXNippon.CentralizedDatabaseSystem.Domain.TemplateManagements;
using Microsoft.AspNetCore.Components;
using Radzen;
using ViewODataService.Affra.Service.View.Domain.Templates;

namespace JXNippon.CentralizedDatabaseSystem.Shared.TemplateManagement
{
    public partial class ExtraColumnEditor
    {
        private IDictionary<string, object> extraObject;
        [Parameter] public IEnumerable<CustomColumn> CustomColumns { get; set; }
        [Parameter] public bool IsViewing { get; set; }

        [Parameter] public IDictionary<string, object>? ExtraDictionary { get; set; }

        [Inject] private IExtraColumnService ExtraColumnService { get; set; }

        protected override Task OnInitializedAsync()
        {
            extraObject = this.ExtraColumnService.GetExtraObject(this.CustomColumns, this.ExtraDictionary);

            return Task.CompletedTask;
        }

        public string GetExtras()
        { 
            return JsonSerializer.Serialize(extraObject);
        }

        public void UpdateValue(string property, object value)
        {
            this.extraObject[property] = value;
        }

        public void UpdateDateTimeValue(string property, object value)
        {
            var dateTime = value as DateTime?;
            var newDateTime = DateTime.FromBinary(dateTime.Value.ToBinary());
            this.extraObject[property] = newDateTime.ToUniversalTime();
        }

    }
}
