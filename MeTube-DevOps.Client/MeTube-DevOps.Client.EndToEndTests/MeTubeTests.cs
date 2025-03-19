using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace MeTube_DevOps.Client.EndToEndTests
{
    public class MeTubeTests : IClassFixture<PlaywrightFixture>
    {
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private readonly IPlaywright _playwright;
        private readonly IBrowser _chromiumBrowser;
        private readonly IBrowser _firefoxBrowserBrowser;
        private readonly IBrowser _webKitBrowserBrowser;
        private readonly ITestOutputHelper _output;

        public MeTubeTests(PlaywrightFixture fixture, ITestOutputHelper output)
        {
            _output = output;
            _configuration = fixture.Configuration;
            _baseUrl = fixture.BaseUrl;
            _playwright = fixture.Playwright;
            _chromiumBrowser = fixture.ChromiumBrowser;
            _firefoxBrowserBrowser = fixture.FirefoxBrowser;
            _webKitBrowserBrowser = fixture.WebkitBrowser;
        }

        [Fact]
        public async Task NavigateToLoginPage_ShouldDisplayLoginForm()
        {
            var context = await _chromiumBrowser.NewContextAsync();
            var page = await context.NewPageAsync();

            await page.GotoAsync(_baseUrl, new PageGotoOptions { WaitUntil = WaitUntilState.Load });

            var homeTitle = await page.TitleAsync();
            _output.WriteLine($"Hemsidans titel: {homeTitle}");
            homeTitle.Should().Contain("MeTube");

            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await Task.Delay(500);

            var loginUrl = page.Url;
            loginUrl.Should().Contain("/login");

            await page.ScreenshotAsync(new PageScreenshotOptions { Path = "screenshots/login_page.png" });

            await context.CloseAsync();
        }
    }
}