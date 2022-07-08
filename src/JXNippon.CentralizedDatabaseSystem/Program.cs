using System.Globalization;
using Affra.Core.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem;
using JXNippon.CentralizedDatabaseSystem.Configurations;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using JXNippon.CentralizedDatabaseSystem.Domain.Hubs;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using JXNippon.CentralizedDatabaseSystem.Handlers;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.FileManagements;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.Hubs;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.Views;
using JXNippon.CentralizedDatabaseSystem.Notifications;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Microsoft.OData.Extensions.Client;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

CultureConfigurations cultureConfigurations = new CultureConfigurations();
builder.Configuration.GetSection(nameof(CultureConfigurations)).Bind(cultureConfigurations);

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(cultureConfigurations.DefaultCulture);
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(cultureConfigurations.DefaultCulture);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

DataExtractorConfigurations dataExtractorConfigurations = new DataExtractorConfigurations();
builder.Configuration.GetSection(nameof(DataExtractorConfigurations)).Bind(dataExtractorConfigurations);

CentralizedDatabaseSystemConfigurations centralizedDatabaseSystemConfigurations = new CentralizedDatabaseSystemConfigurations();
builder.Configuration.GetSection(nameof(CentralizedDatabaseSystemConfigurations)).Bind(centralizedDatabaseSystemConfigurations);

ViewConfigurations viewConfigurations = new ViewConfigurations();
builder.Configuration.GetSection(nameof(ViewConfigurations)).Bind(viewConfigurations);

ContentUpdateNotificationServiceConfigurations contentUpdateNotificationServiceConfigurations = new ContentUpdateNotificationServiceConfigurations();
builder.Configuration.GetSection(nameof(ContentUpdateNotificationServiceConfigurations)).Bind(contentUpdateNotificationServiceConfigurations);

AzureAdConfigurations azureAdConfigurations = new AzureAdConfigurations();
builder.Configuration.GetSection(nameof(AzureAdConfigurations)).Bind(azureAdConfigurations);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
    .AddScoped<IDataExtractorUnitOfWork, DataExtractorUnitOfWork>()
    .AddSingleton<IOptions<DataExtractorConfigurations>>(Options.Create(dataExtractorConfigurations))
    .AddScoped<ICentralizedDatabaseSystemUnitOfWork, CentralizedDatabaseSystemUnitOfWork>()
    .AddSingleton<IOptions<CentralizedDatabaseSystemConfigurations>>(Options.Create(centralizedDatabaseSystemConfigurations))
    .AddScoped<IViewUnitOfWork, ViewUnitOfWork>()
    .AddSingleton<IOptions<ViewConfigurations>>(Options.Create(viewConfigurations))
    .AddUnitGenericService()
    .AddODataClient(nameof(DataExtractorUnitOfWork))
    .AddHttpClient()
    .AddHttpMessageHandler<CreateActivityHandler>()
    .AddHttpMessageHandler<JXNippon.CentralizedDatabaseSystem.Handlers.AuthorizationMessageHandler>()
    .Services
    .AddODataClient(nameof(CentralizedDatabaseSystemUnitOfWork))
    .AddHttpClient()
    .AddHttpMessageHandler<CreateActivityHandler>()
    .AddHttpMessageHandler<JXNippon.CentralizedDatabaseSystem.Handlers.AuthorizationMessageHandler>()
    .Services
    .AddODataClient(nameof(ViewUnitOfWork))
    .AddHttpClient()
    .AddHttpMessageHandler<CreateActivityHandler>()
    .AddHttpMessageHandler<JXNippon.CentralizedDatabaseSystem.Handlers.AuthorizationMessageHandler>()
    .Services
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
    .AddSingleton<IOptions<AzureAdConfigurations>>(Options.Create(azureAdConfigurations))
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
