﻿using System.Collections.ObjectModel;
using System.Text.Json;
using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.AspNetCore.Components;
using Radzen;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class GridDialog
    {
        [Parameter] public View View { get; set; }
        [Parameter] public Column Column { get; set; }
        public Grid Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private IViewService ViewService { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private IEnumerable<string> types;
        private bool isViewing { get => MenuAction == 3; }

        protected override Task OnInitializedAsync()
        {
            Column.ViewName = View.Name;
            Column.View = View;
            types = ViewService.GetTypeMapping()
                .Select(x => x.Key)
                .ToHashSet();

            Item = new Grid()
            {
                GridColumns = new Collection<GridColumn>(),
            };
            if (!string.IsNullOrEmpty(Column.ColumnComponent))
            {
                Item = JsonSerializer.Deserialize<Grid>(Column.ColumnComponent) ?? Item;
            }
            
            return Task.CompletedTask;
        }

        protected Task SubmitAsync(Grid arg)
        {
            Column.ColumnComponent = JsonSerializer.Serialize(Item);
            DialogService.Close(true);
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }

        private void OnChange(object value)
        {
            var row = value as Row;
            Column.RowId = row.Id;
            Column.Row = row;
            Column.Sequence = row.Columns.Count;
        }
    }
}