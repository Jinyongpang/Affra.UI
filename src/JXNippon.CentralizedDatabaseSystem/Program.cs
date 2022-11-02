global using JXNippon.CentralizedDatabaseSystem.Domain.Extensions;
global using JXNippon.CentralizedDatabaseSystem.Domain.Roles;
global using System.Collections.ObjectModel;
global using UserODataService.Affra.Service.User.Domain.Roles;
using System.Globalization;
using Affra.Core.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem;
using JXNippon.CentralizedDatabaseSystem.Configurations;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.CommonHelpers;
using JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates;
using JXNippon.CentralizedDatabaseSystem.Domain.DataSources;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using JXNippon.CentralizedDatabaseSystem.Domain.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Domain.Notifications;
using JXNippon.CentralizedDatabaseSystem.Domain.TemplateManagements;
using JXNippon.CentralizedDatabaseSystem.Domain.Users;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using JXNippon.CentralizedDatabaseSystem.Extensions;
using JXNippon.CentralizedDatabaseSystem.Handlers;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.FileManagements;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.Hubs;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.ManagementOfChanges;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.Notifications;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.Users;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.Views;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using OpenAPI.UserService;
using Radzen;
using JXNippon.CentralizedDatabaseSystem.Domain.Workspaces;
using JXNippon.CentralizedDatabaseSystem.Domain.WorkspaceAPI;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.Workspaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

CultureConfigurations cultureConfigurations = new CultureConfigurations();
builder.Configuration.GetSection(nameof(CultureConfigurations)).Bind(cultureConfigurations);

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(cultureConfigurations.DefaultCulture);
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(cultureConfigurations.DefaultCulture);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

DataExtractorConfigurations dataExtractorConfigurations = new();
builder.Configuration.GetSection(nameof(DataExtractorConfigurations)).Bind(dataExtractorConfigurations);

CentralizedDatabaseSystemConfigurations centralizedDatabaseSystemConfigurations = new();
builder.Configuration.GetSection(nameof(CentralizedDatabaseSystemConfigurations)).Bind(centralizedDatabaseSystemConfigurations);

ViewConfigurations viewConfigurations = new();
builder.Configuration.GetSection(nameof(ViewConfigurations)).Bind(viewConfigurations);

ContentUpdateNotificationServiceConfigurations contentUpdateNotificationServiceConfigurations = new ContentUpdateNotificationServiceConfigurations();
builder.Configuration.GetSection(nameof(ContentUpdateNotificationServiceConfigurations)).Bind(contentUpdateNotificationServiceConfigurations);

AzureAdConfigurations azureAdConfigurations = new();
builder.Configuration.GetSection(nameof(AzureAdConfigurations)).Bind(azureAdConfigurations);

NotificationConfigurations notificationConfigurations = new();
builder.Configuration.GetSection(nameof(NotificationConfigurations)).Bind(notificationConfigurations);

PersonalMessageNotificationServiceConfigurations personalMessageServiceConfigurations = new();
builder.Configuration.GetSection(nameof(PersonalMessageNotificationServiceConfigurations)).Bind(personalMessageServiceConfigurations);

ManagementOfChangeConfigurations managementOfChangeConfigurations = new();
builder.Configuration.GetSection(nameof(ManagementOfChangeConfigurations)).Bind(managementOfChangeConfigurations);

UserConfigurations userConfigurations = new();
builder.Configuration.GetSection(nameof(UserConfigurations)).Bind(userConfigurations);

RoleAuthorizationConfigurations roleAuthorizationConfigurations = new();
builder.Configuration.GetSection(nameof(RoleAuthorizationConfigurations)).Bind(roleAuthorizationConfigurations);

WorkspaceAPIConfigurations workspaceAPIConfigurations = new();
builder.Configuration.GetSection(nameof(WorkspaceAPIConfigurations)).Bind(workspaceAPIConfigurations);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
    .AddScoped<IDataExtractorUnitOfWork, DataExtractorUnitOfWork>()
    .AddSingleton<IOptions<DataExtractorConfigurations>>(Options.Create(dataExtractorConfigurations))
    .AddScoped<ICentralizedDatabaseSystemUnitOfWork, CentralizedDatabaseSystemUnitOfWork>()
    .AddSingleton<IOptions<CentralizedDatabaseSystemConfigurations>>(Options.Create(centralizedDatabaseSystemConfigurations))
    .AddScoped<IViewUnitOfWork, ViewUnitOfWork>()
    .AddSingleton<IOptions<ViewConfigurations>>(Options.Create(viewConfigurations))
    .AddScoped<INotificationUnitOfWork, NotificationUnitOfWork>()
    .AddSingleton<IOptions<NotificationConfigurations>>(Options.Create(notificationConfigurations))
    .AddScoped<IManagementOfChangeUnitOfWork, ManagementOfChangeUnitOfWork>()
    .AddSingleton<IOptions<ManagementOfChangeConfigurations>>(Options.Create(managementOfChangeConfigurations))
    .AddScoped<IUserUnitOfWork, UserUnitOfWork>()
    .AddSingleton<IOptions<UserConfigurations>>(Options.Create(userConfigurations))
    .AddUnitGenericService()
    .AddODataHttpClient(nameof(DataExtractorUnitOfWork))
    .AddODataHttpClient(nameof(CentralizedDatabaseSystemUnitOfWork))
    .AddODataHttpClient(nameof(ViewUnitOfWork))
    .AddODataHttpClient(nameof(NotificationUnitOfWork))
    .AddODataHttpClient(nameof(ManagementOfChangeUnitOfWork))
    .AddODataHttpClient(nameof(UserUnitOfWork))
    .AddScoped<CreateActivityHandler>()
    .AddScoped<JXNippon.CentralizedDatabaseSystem.Handlers.AuthorizationMessageHandler>()
    .AddScoped<NotificationService>()
    .AddScoped<TooltipService>()
    .AddSingleton<IJSInProcessRuntime>(services =>
        (IJSInProcessRuntime)services.GetRequiredService<IJSRuntime>())
    .AddScoped<ContextMenuService>()
    .AddScoped<DialogService>()
    .AddScoped<AffraNotificationService>()
    .AddScoped<IViewService, ViewService>()
    .AddScoped(typeof(IHubClient<>), typeof(SignalRHubClient<>))
    .AddScoped<IContentUpdateNotificationService, ContentUpdateNotificationService>()
    .AddSingleton<IOptions<ContentUpdateNotificationServiceConfigurations>>(Options.Create(contentUpdateNotificationServiceConfigurations))
    .AddScoped<IPersonalMessageNotificationService, PersonalMessageNotificationService>()
    .AddSingleton<IOptions<PersonalMessageNotificationServiceConfigurations>>(Options.Create(personalMessageServiceConfigurations))
    .AddSingleton<IOptions<AzureAdConfigurations>>(Options.Create(azureAdConfigurations))
    .AddScoped<IPersonalMessageService, PersonalMessageService>()
    .AddSingleton<IGlobalDataSource, GlobalDataSource>()
    .AddScoped<IExtraColumnService, ExtraColumnService>()
    .AddScoped<ICommonHelper, CommonHelper>()
    .AddSingleton<IOptions<RoleAuthorizationConfigurations>>(Options.Create(roleAuthorizationConfigurations))
    .AddScoped<IUserService, UserService>()
    .AddSingleton<IAuthorizationHandler, UserRoleAuthorizeHandler>()
    .AddSingleton<IOptions<WorkspaceAPIConfigurations>>(Options.Create(workspaceAPIConfigurations))
    .AddScoped<IWorkspaceService, WorkspaceService>()
    .AddHttpClient<IWorkspaceAPIClient, JXNippon.CentralizedDatabaseSystem.Infrastructure.Workspaces.WorkspaceAPIClient>()
    .AddHttpMessageHandler<CreateActivityHandler>()
    .AddHttpMessageHandler<AuthorizationMessageHandler>()
    .Services
    .AddHttpClient<IUserServiceClient, JXNippon.CentralizedDatabaseSystem.Infrastructure.Users.UserServiceClient>()
    .AddHttpMessageHandler<CreateActivityHandler>()
    .AddHttpMessageHandler<AuthorizationMessageHandler>()
    .Services
    .AddMemoryCache()
    .AddAuthorizationCore(options =>
    {
        foreach (FeaturePermission featurePermission in Enum.GetValues(typeof(FeaturePermission)))
        {
            options.AddPolicy($"{featurePermission}.Read", policy =>
                policy.Requirements.Add(new UserRoleAuthorizePermission(new Permission() { Name = featurePermission.ToString(), HasReadPermissoin = true })));
            options.AddPolicy($"{featurePermission}.ReadWrite", policy =>
                policy.Requirements.Add(new UserRoleAuthorizePermission(new Permission() { Name = featurePermission.ToString(), HasReadPermissoin = true, HasWritePermission = true, })));
        }

    })
    .AddAntDesign()
    .AddLocalization();

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    foreach (var scope in azureAdConfigurations.Scopes)
    {
        options.ProviderOptions.DefaultAccessTokenScopes.Add(scope);
    }
    options.ProviderOptions.LoginMode = "redirect";
});

await builder.Build().RunAsync();
