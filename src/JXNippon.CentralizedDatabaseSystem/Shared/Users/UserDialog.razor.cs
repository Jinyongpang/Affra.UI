using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using Microsoft.AspNetCore.Components;
using Radzen;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Users
{
    public partial class UserDialog
    {
        private UserPersonalization userPersonalization;
        [Parameter] public User Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Parameter] public bool IsUserEdit { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IUserService UserService { get; set; }
        private bool isViewing { get => MenuAction == 3; }

        private ICollection<string> roles = new List<string>()
        {
            "User",
            "SuperUser",
            "Administrator",
        };

        protected override Task OnInitializedAsync()
        {
            this.userPersonalization = this.Item.UserPersonalization;
            return Task.CompletedTask;
        }

        protected Task SubmitAsync(User arg)
        {
            arg.UserPersonalization.AvatarColor = this.userPersonalization.AvatarColor;
            arg.UserPersonalization = this.userPersonalization;
            DialogService.Close(true);
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }
    }
}
