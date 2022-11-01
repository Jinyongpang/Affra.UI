﻿using Affra.Core.Domain.Services;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Description
{
    public partial class DecimalDescriptionItemEdit<TItem>
        where TItem : class
    {
        [Parameter] public decimal? Decimal { get; set; }
        [Parameter] public TItem Item { get; set; }
        [Parameter] public long ItemId { get; set; }
        [Parameter] public bool IsRequired { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Parameter] public EventCallback<decimal?> DecimalChanged { get; set; }
        [Parameter] public EventCallback<decimal?> OnDecimalChanged { get; set; }

        private IGenericService<TItem> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<TItem, ICentralizedDatabaseSystemUnitOfWork>>();
        }

        private bool isEditing;

        private async Task StartEdit()
        {
            isEditing = true;
            this.StateHasChanged();
        }

        private void MouseLeave()
        {
            isEditing = false;
            this.StateHasChanged();
        }

        private async Task StopEdit()
        {
            try
            {
                await DecimalChanged.InvokeAsync(this.Decimal);
                await OnDecimalChanged.InvokeAsync(this.Decimal);

                using var scope = ServiceProvider.CreateScope();
                var service = this.GetGenericService(scope);

                if (this.Item.AsIEntity().Id > 0)
                {
                    await service.UpdateAsync(this.Item, this.Item.AsIEntity().Id);
                    AffraNotificationService.NotifyItemUpdated();
                }
                else
                {
                    await service.InsertAsync(this.Item);
                    AffraNotificationService.NotifyItemCreated();
                }

                isEditing = false;
                this.StateHasChanged();
            }
            catch (Exception ex)
            {
                this.AffraNotificationService.NotifyException(ex);
            }
        }

        private string GetStyle()
        {
            return this.Decimal is null
                ? "background-color: yellow;"
                : null;
        }
    }
}
