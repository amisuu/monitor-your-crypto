using CryptoChecker.Application.Interfaces;
using CryptoChecker.Application.Services;
using CryptoChecker.Domain.Interfaces;
using CryptoChecker.Infrastructure;
using CryptoChecker.Infrastructure.Repositories.Repositories;
using ElectronNET.API;
using ElectronNET.API.Entities;
using MatBlazor;
using Microsoft.Data.Sqlite;
using SqlKata.Compilers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddElectron();
builder.WebHost.UseElectron(args);

// Add services to the container.
builder.Services.AddScoped<HttpClient>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMatBlazor();

builder.Services.AddMatToaster(config =>
{
    config.Position = MatToastPosition.BottomCenter;
    config.PreventDuplicates = true;
    config.NewestOnTop = true;
    config.ShowCloseButton = true;
    config.MaximumOpacity = 95;
    config.VisibleStateDuration = 3000;
});

builder.Services.AddSingleton<ICryptoStatsSource, CoingeckoService>();

var dbConnection = new SqliteConnection("Data Source=data.db");
var db = new SqlKataDatabase(dbConnection, new SqliteCompiler());
builder.Services.AddSingleton(ctx => db);

builder.Services.AddSingleton<ICryptocurrencyResolver, CryptocurrencyResolverService>();
builder.Services.AddSingleton<ISummaryService, SummaryService>();

builder.Services.AddSingleton<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddSingleton<IMarketOrderRepository, MarketOrderRepository>();
builder.Services.AddSingleton<IPortfolioEntryRepository, PortfolioEntryRepository>();

builder.Services.AddSingleton<IPortfolioService, PortfolioService>();
builder.Services.AddSingleton<IMarketOrderService, MarketOrderService>();
builder.Services.AddSingleton<IPortfolioEntryService, PortfolioEntryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

if (HybridSupport.IsElectronActive)
{
    ElectronBootstrap();
}

app.Run();


async void ElectronBootstrap()
{
    var url = new BrowserWindowOptions();
    url.Show = false;
    url.Height = 940;
    url.Width = 1152;
    var browserWindow = await Electron.WindowManager.CreateWindowAsync(url);
    await browserWindow.WebContents.Session.ClearCacheAsync();
    browserWindow.RemoveMenu();
    browserWindow.OnReadyToShow += () => browserWindow.Show();
    browserWindow.SetTitle("Crypto Checker");
}
