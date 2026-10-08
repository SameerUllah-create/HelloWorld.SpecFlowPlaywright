using HelloWorld.E2E.Tests.Support;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace HelloWorld.E2E.Tests.Hooks;

[Binding]
public sealed class BrowserHooks
{
    private static readonly object LockObject = new();
    private static IPlaywright? _playwright;
    private static IBrowser? _browser;
    private static TestSettings _settings = new();

    private readonly ScenarioContext _scenarioContext;

    public BrowserHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    public static TestSettings Settings => _settings;

    [BeforeTestRun(Order = 0)]
    public static void LoadSettings()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("testsettings.json", optional: false)
            .Build();

        _settings = configuration.Get<TestSettings>() ?? new TestSettings();
    }

    [BeforeTestRun(Order = 1)]
    public static async Task StartBrowserAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = _settings.Headless
        });
    }

    [BeforeScenario]
    public async Task CreateBrowserContextAsync()
    {
        if (_browser is null)
        {
            throw new InvalidOperationException("Browser is not initialized.");
        }

        var browserContext = await _browser.NewContextAsync();
        var page = await browserContext.NewPageAsync();

        _scenarioContext.Set(new ScenarioContextData
        {
            BrowserContext = browserContext,
            Page = page
        }, nameof(ScenarioContextData));
    }

    [AfterScenario]
    public async Task DisposeBrowserContextAsync()
    {
        if (!_scenarioContext.TryGetValue(nameof(ScenarioContextData), out ScenarioContextData? data) || data is null)
        {
            return;
        }

        await data.Page.CloseAsync();
        await data.BrowserContext.CloseAsync();
    }

    [AfterTestRun]
    public static async Task StopBrowserAsync()
    {
        lock (LockObject)
        {
            if (_browser is null && _playwright is null)
            {
                return;
            }
        }

        if (_browser is not null)
        {
            await _browser.CloseAsync();
            _browser = null;
        }

        _playwright?.Dispose();
        _playwright = null;
    }
}
