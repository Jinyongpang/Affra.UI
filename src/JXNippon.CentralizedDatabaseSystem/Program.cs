using Affra.Core.Domain.Extensions;
using JXNippon.CentralizedDatabaseSystem;
using JXNippon.CentralizedDatabaseSystem.Domain.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using JXNippon.CentralizedDatabaseSystem.Handlers;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.CentralizedDatabaseSystemServices;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.FileManagements;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Microsoft.OData.Extensions.Client;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

DataExtractorConfigurations dataExtractorConfigurations = new DataExtractorConfigurations();
builder.Configuration.GetSection(nameof(DataExtractorConfigurations)).Bind(dataExtractorConfigurations);

CentralizedDatabaseSystemConfigurations centralizedDatabaseSystemConfigurations = new CentralizedDatabaseSystemConfigurations();
builder.Configuration.GetSection(nameof(CentralizedDatabaseSystemConfigurations)).Bind(centralizedDatabaseSystemConfigurations);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
    .AddScoped<IDataExtractorUnitOfWork, DataExtractorUnitOfWork>()
    .AddSingleton<IOptions<DataExtractorConfigurations>>(Options.Create(dataExtractorConfigurations))
    .AddScoped<ICentralizedDatabaseSystemUnitOfWork, CentralizedDatabaseSystemUnitOfWork>()
    .AddSingleton<IOptions<CentralizedDatabaseSystemConfigurations>>(Options.Create(centralizedDatabaseSystemConfigurations))
    .AddUnitGenericService()
    .AddODataClient(nameof(DataExtractorUnitOfWork))
    .AddHttpClient()
    .AddHttpMessageHandler<CreateActivityHandler>()
    .Services
    .AddTransient<CreateActivityHandler>()
    .AddScoped<NotificationService>()
    .AddSingleton<IJSInProcessRuntime>(services =>
        (IJSInProcessRuntime)services.GetRequiredService<IJSRuntime>());

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
});

await builder.Build().RunAsync();
