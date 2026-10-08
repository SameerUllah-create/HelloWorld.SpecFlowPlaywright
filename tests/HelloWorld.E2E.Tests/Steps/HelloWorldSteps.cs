using HelloWorld.E2E.Tests.Hooks;
using HelloWorld.E2E.Tests.Support;
using NUnit.Framework;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace HelloWorld.E2E.Tests.Steps;

[Binding]
public sealed class HelloWorldSteps
{
    private readonly ScenarioContext _scenarioContext;

    public HelloWorldSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private ScenarioContextData Data => _scenarioContext.Get<ScenarioContextData>(nameof(ScenarioContextData));

    [Given("the Hello World app is running")]
    public async Task GivenTheHelloWorldAppIsRunning()
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(BrowserHooks.Settings.BaseUrl);
        Assert.That(response.IsSuccessStatusCode, Is.True,
            $"Expected app to be reachable at {BrowserHooks.Settings.BaseUrl}. Start the app before running tests.");
    }

    [When("I open the home page")]
    public async Task WhenIOpenTheHomePage()
    {
        await Data.Page.GotoAsync(BrowserHooks.Settings.BaseUrl);
    }

    [When("I click the \"(.*)\" button")]
    public async Task WhenIClickTheButton(string buttonText)
    {
        await Data.Page.GetByRole(AriaRole.Button, new() { Name = buttonText }).ClickAsync();
    }

    [Then("the page should load successfully")]
    public void ThenThePageShouldLoadSuccessfully()
    {
        Assert.That(Data.Page.Url, Is.EqualTo(BrowserHooks.Settings.BaseUrl + "/").Or.EqualTo(BrowserHooks.Settings.BaseUrl));
    }

    [Then("I should see the \"(.*)\" message")]
    public async Task ThenIShouldSeeTheMessage(string expectedMessage)
    {
        var messageText = await Data.Page.Locator("#hello-message").InnerTextAsync();
        Assert.That(messageText.Trim(), Is.EqualTo(expectedMessage));
    }

    [Then("I should be redirected to \"(.*)\"")]
    public async Task ThenIShouldBeRedirectedTo(string expectedPath)
    {
        var expectedUrl = $"{BrowserHooks.Settings.BaseUrl.TrimEnd('/')}{expectedPath}";
        await Data.Page.WaitForURLAsync(expectedUrl);
        Assert.That(Data.Page.Url, Is.EqualTo(expectedUrl));
    }

    [Then("the page title should be \"(.*)\"")]
    public async Task ThenThePageTitleShouldBe(string expectedTitle)
    {
        var pageTitle = await Data.Page.TitleAsync();
        Assert.That(pageTitle, Is.EqualTo(expectedTitle));
    }
}