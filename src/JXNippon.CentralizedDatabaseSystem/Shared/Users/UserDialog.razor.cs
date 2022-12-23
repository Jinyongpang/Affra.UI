using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using Microsoft.AspNetCore.Components;
using Radzen;
using UserODataService.Affra.Service.User.Domain.Users;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Users
{
    public partial class UserDialog
    {
        private UserPersonalization userPersonalization = new UserPersonalization();
        [Parameter] public User Item { get; set; }
        [Parameter] public int MenuAction { get; set; }
        [Parameter] public bool IsUserEdit { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private IUserService UserService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }
        private bool isViewing { get => MenuAction == 3; }
        private int[] availableAvatar = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        private ICollection<string> roles = new List<string>()
        {
            "User",
            "SuperUser",
            "Administrator",
        };

        protected override async Task OnInitializedAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            roles = (await userService.GetRolesAsync())
                .Select(x => x.Name)
                .ToList();
            this.userPersonalization = this.Item.UserPersonalization;
        }

        protected Task SubmitAsync(User arg)
        {
            if (string.IsNullOrEmpty(Item.Username))
            {
                Item.Username = Item.Email;
            }

            if (Item.Id == Guid.Empty)
            {
                Item.CreatedDateTime = DateTime.UtcNow;
            }

            DialogService.Close(true);
            return Task.CompletedTask;
        }

        private void OnSelect(int id)
        {
            this.userPersonalization.AvatarId = id;
        }

        private void Cancel()
        {
            DialogService.Close(false);
        }
    }
}
