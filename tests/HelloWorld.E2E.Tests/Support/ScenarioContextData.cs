using Microsoft.Playwright;

namespace HelloWorld.E2E.Tests.Support;

public sealed class ScenarioContextData
{
    public required IBrowserContext BrowserContext { get; init; }
    public required IPage Page { get; init; }
}
