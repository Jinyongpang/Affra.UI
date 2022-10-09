﻿using System.Collections.Concurrent;
using Affra.Core.Domain.Services;
using Affra.Core.Infrastructure.OData.Extensions;
using AntDesign;
using JXNippon.CentralizedDatabaseSystem.Domain.Notifications;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.OData.Client;
using NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Notifications
{
    public partial class PersonalMessageList
    {
        private const int loadSize = 9;
        private AntList<PersonalMessage> _dataList;
        private List<PersonalMessage > personalMessages;
        private int count;
        private int currentCount;
        private bool isLoading = false;
        private bool initLoading = true;
        private IDictionary<string, User> users = new ConcurrentDictionary<string, User>(StringComparer.OrdinalIgnoreCase);

        [Parameter] public PersonalMessageStatus? PersonalMessageStatus { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        [Inject] private AffraNotificationService AffraNotificationService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private IUserService UserService { get; set; }
        public int Count => this.count;

        protected override Task OnInitializedAsync()
        {
            initLoading = false;
            return this.LoadDataAsync();
        }

        public Task ReloadAsync()
        {
            return this.LoadDataAsync();
        }

        public Task OnLoadMoreAsync()
        {
            return this.LoadDataAsync(true);
        }

        private async Task LoadDataAsync(bool isLoadMore = false)
        {
            isLoading = true;
            StateHasChanged();
            if (!isLoadMore)
            {
                currentCount = 0;
            }

            using var serviceScope = ServiceProvider.CreateScope();
            IGenericService<PersonalMessage>? personalMessageService = this.GetGenericService(serviceScope);
            var query = (DataServiceQuery<PersonalMessage>)personalMessageService.Get();

            if (this.PersonalMessageStatus is not null)
            { 
                query = (DataServiceQuery<PersonalMessage>)query.Where(x => x.Status == this.PersonalMessageStatus);
            }

            QueryOperationResponse<PersonalMessage>? response = await query
                .Expand(x => x.Message)
                .OrderByDescending(x => x.CreatedDateTime)
                .Skip(currentCount)
                .Take(loadSize)
                .ToQueryOperationResponseAsync<PersonalMessage>();

            count = (int)response.Count;
            currentCount += loadSize;
            var personalMessageList = response.ToList();

            if (isLoadMore)
            {
                personalMessages.AddRange(personalMessageList);
            }
            else
            {
                personalMessages = personalMessageList;
            }

            foreach (var email in personalMessages
                .Where(x => !string.IsNullOrEmpty(x.Message.CreatedBy))
                .Select(x => x.Message.CreatedBy))
            {
                if (!this.users.TryGetValue(email, out var user))
                {
                    using var serviceScopeUser = ServiceProvider.CreateScope();
                    var userService = this.GetUserGenericService(serviceScopeUser);
                    user = (await userService.Get()
                        .Where(x => x.Email.ToUpper() == email.ToUpper())
                        .ToQueryOperationResponseAsync<User>())
                        .FirstOrDefault();
                    this.users[email] = user;
                }
                
            }

            isLoading = false;

            if (personalMessages.DistinctBy(x => x.Id).Count() != personalMessages.Count)
            {
                AffraNotificationService.NotifyWarning("Data have changed. Kindly reload.");
            }

            StateHasChanged();
        }

        private void HandleException(Exception ex)
        {
            AffraNotificationService.NotifyException(ex);
        }
        private async Task MarkAsReadAsync(PersonalMessage personalMessage)
        {
            if (personalMessage is null
                || personalMessage.Status == NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages.PersonalMessageStatus.Read)
            {
                return;
            }
            using var serviceScope = ServiceProvider.CreateScope();
            var service = serviceScope.ServiceProvider.GetRequiredService<IPersonalMessageService>();
            await service.MarkAsReadAsync(personalMessage);

            if (this.PersonalMessageStatus == NotificationODataService.Affra.Service.Notification.Domain.PersonalMessages.PersonalMessageStatus.Unread)
            {
                count--;
                currentCount--;
                this.personalMessages.Remove(personalMessage);
                this.StateHasChanged();
            }
        }
        private IGenericService<PersonalMessage> GetGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<PersonalMessage, INotificationUnitOfWork>>();
        }

        private IGenericService<User> GetUserGenericService(IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService<IUnitGenericService<User, IUserUnitOfWork>>();
        }

        private string GetAvatarName(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return string.Empty;
            }
            if (this.users.TryGetValue(email, out var user))
            { 
                return this.UserService.GetAvatarName(user.Name);
            }

            return $"{email[0]}";
        }

        private string GetAvatarColor(string email)
        {
            if (!string.IsNullOrEmpty(email) && this.users.TryGetValue(email, out var user))
            {
                return user?.UserPersonalization?.AvatarColor ?? string.Empty;
            }
            return string.Empty;
        }

        private string GetAvatarIcon(string email)
        {
            if (!string.IsNullOrEmpty(email) && this.users.TryGetValue(email, out var user))
            {
                return user?.UserPersonalization?.AvatarId > 0
                     ? $"avatar\\{user?.UserPersonalization?.AvatarId}.png"
                     : string.Empty;
            }
            return string.Empty;
        }
    }
}
