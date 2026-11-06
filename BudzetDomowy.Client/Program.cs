using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BudzetDomowy.Client;

// Formatowanie kwot i dat w polskim stylu (przecinek jako separator dziesiętny).
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pl-PL");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pl-PL");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
