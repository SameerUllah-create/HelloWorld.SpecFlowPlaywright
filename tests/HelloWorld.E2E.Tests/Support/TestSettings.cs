namespace HelloWorld.E2E.Tests.Support;

public sealed class TestSettings
{
    public string BaseUrl { get; init; } = "http://localhost:5070";
    public bool Headless { get; init; } = true;
}
