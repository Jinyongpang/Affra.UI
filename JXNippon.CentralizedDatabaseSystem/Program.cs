using Affra.Core.Domain.Extensions;
using Affra.Core.Domain.UnitOfWorks;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using JXNippon.CentralizedDatabaseSystem;
using JXNippon.CentralizedDatabaseSystem.Domain.FileManagements;
using JXNippon.CentralizedDatabaseSystem.Infrastructure.FileManagements;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OData.Extensions.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

DataExtractorConfigurations dataExtractorConfigurations = new DataExtractorConfigurations();
builder.Configuration.GetSection(nameof(DataExtractorConfigurations)).Bind(dataExtractorConfigurations);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
    .AddScoped<IFileManagementService, FileManagementService>()
    .AddScoped<IDataExtractorUnitOfWork, DataExtractorUnitOfWork>()
    .AddScoped<IUnitOfWork, DataExtractorUnitOfWork>()
    .AddSingleton<IOptions<DataExtractorConfigurations>>(Options.Create(dataExtractorConfigurations))
    .AddGenericService()
    .AddODataClient(nameof(DataExtractorUnitOfWork))
    .AddHttpClient();

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
});

await builder.Build().RunAsync();
