﻿using System.Dynamic;
using JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates;
using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem.Domain.Grids;
using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using JXNippon.CentralizedDatabaseSystem.Models;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.OData.Client;
using Radzen;
using Radzen.Blazor;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Views
{
    public partial class DataGridComponent : IAsyncDisposable
    {
        private RadzenDataGrid<IDictionary<string, object>> grid;
        private IEnumerable<IDaily> items;
        private IEnumerable<IDictionary<string, object>> itemsDictionary;
        private bool isLoading = false;
        private IHubSubscription subscription;
        private bool isDisposed = false;

        [Parameter] public EventCallback<IQueryable<dynamic>> LoadData { get; set; }
        [Parameter] public IQueryable<dynamic> Queryable { get; set; }
        [Parameter] public string TType { get; set; }
        [Parameter] public string Subscription { get; set; }
        [Parameter] public DateTimeOffset? StartDate { get; set; }
        [Parameter] public DateTimeOffset? EndDate { get; set; }
        [Parameter] public IEnumerable<GridColumn> GridColumns { get; set; }
        [Parameter] public Column Column { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private IViewService ViewService { get; set; }
        [Inject] private IContentUpdateNotificationService ContentUpdateNotificationService { get; set; }
        
        public CommonFilter CommonFilter { get; set; }
        public int Count { get; set; }

        private IEnumerable<int> pageSizeOptions = new int[] { 5, 10, 15 };

        private Type Type
        {
            get
            {
                var type = Type.GetType(TType);
                return type;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(this.Subscription))
            {
                subscription = ContentUpdateNotificationService.Subscribe<object>(Subscription, OnContentUpdateAsync);
                await subscription.StartAsync();
            }           
            await ReloadAsync(StartDate, EndDate);
        }

        private Task OnContentUpdateAsync(object obj)
        {
            StateHasChanged();
            return this.ReloadAsync();
        }
        public Task ReloadAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            StartDate = startDate ?? StartDate;
            EndDate = endDate ?? EndDate;
            return grid?.FirstPage(true) ?? Task.CompletedTask;
        }

        private async Task LoadDataAsync(LoadDataArgs args)
        {
            isLoading = true;
            this.items = await this.GetDailyItemsAsync(this.TType, this.StartDate, this.EndDate, args);
            itemsDictionary = await this.MergeOverridenTypeAsync(this.items) ?? new List<ExpandoObject>();
            isLoading = false;
        }

        private async Task<IEnumerable<IDictionary<string, object>>?> MergeOverridenTypeAsync(IEnumerable<IDaily> dailyItems)
        {
            var types = this.GridColumns
                .Where(x => !string.IsNullOrEmpty(x.Type))
                .Select(x => x.Type)
                .Distinct();
            List<IDictionary<string, object>>? result = new List<IDictionary<string, object>>();
            List<IDaily> list = new List<IDaily>();
            if (dailyItems.Any())
            {
                
                foreach (var type in types)
                {
                    var actualType = ViewHelper.GetActualType(type);
                    if (actualType is not null)
                    {
                        list.AddRange(await this.GetDailyItemsAsync(actualType.AssemblyQualifiedName, dailyItems.First().Date, dailyItems.Last().Date));
                    }
                }
            }
            foreach (var item in dailyItems)
            {
                var dictionaryObject = item.ToDictionaryObject();
                foreach (var matched in list.Where(x => x.Date == item.Date))
                {
                    foreach (var dict in matched.ToDictionaryObject(matched.GetType().Name))
                    {
                        dictionaryObject.Add(dict);
                    }
                }
                    
                result.Add(dictionaryObject);
            }

            return result;
            
        }

        private string GetColumnPropertyExpression(string name, Type type)
        {
            var expression = $@"it[""{name}""].ToString()";
            return type == typeof(int) ? $"int.Parse({expression})" : expression;
        }

        private async Task<IEnumerable<IDaily>> GetDailyItemsAsync(string type, DateTimeOffset? start, DateTimeOffset? end, LoadDataArgs args = null)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            var service = this.ViewService.GetGenericService(serviceScope, type);
            Queryable = service.Get();
            if (start != null && end != null)
            {
                Queryable = Queryable
                    .Cast<IDaily>()
                    .Where(item => item.Date >= start.Value.ToUniversalTime())
                    .Where(item => item.Date <= end.Value.ToUniversalTime());
            }

            Queryable = (IQueryable<dynamic>)Queryable
                .Cast<IDaily>()
                .OrderBy(x => x.Date)
                .AppendQuery(args?.Filter, args?.Skip, args?.Top, args?.OrderBy);

            if (args is not null)
            {
                await LoadData.InvokeAsync(Queryable);
            }
            
            var q = (DataServiceQuery)Queryable;
            var response = (await q
                .ExecuteAsync()) as QueryOperationResponse;

            if (args is not null)
            {
                this.Count = (int)response.Count;
            }

            return response
                .Cast<IDaily>()
                .ToList();
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }

        private System.Reflection.PropertyInfo[]? GetProperties()
        {
            System.Reflection.PropertyInfo[]? properties = Type.GetProperties();
            return properties;
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (!isDisposed)
                {
                    grid.Dispose();
                    if (subscription is not null)
                    {
                        await subscription.DisposeAsync();
                    }
                    isDisposed = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
