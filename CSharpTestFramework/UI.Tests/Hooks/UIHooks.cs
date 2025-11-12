using Allure.Net.Commons;
using Core;
using Microsoft.Playwright;
using Serilog;
using Serilog.Context;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace UI.Tests.Hooks
{
    [Binding]
    public class UIHooks (ScenarioContext _scenarioContext)
    {
        public IPage Page { get; private set; } = null!;
        private IDisposable? _context;

        [BeforeTestRun]
        public static void UITestsSetup()
        {
            Logging.ConfigureLogging();

            Log.Information("Starting UI tests");
        }

        [AfterTestRun]
        public static void UITestsTeardown()
        {
            Log.Information("Ending UI tests");

            Log.CloseAndFlush();

            string sourceFolder = "allure-results";
            string destinationFolder = "../../../../allure-results";
            Core.File.MoveDirectoryFiles(sourceFolder, destinationFolder);
        }

        [BeforeScenario]
        public async Task SetupTestAsync()
        {
            IPlaywright playwright = await Playwright.CreateAsync();
            IBrowser browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
            IBrowserContext context = await browser.NewContextAsync();

            Page = await context.NewPageAsync();
        }

        [AfterScenario]
        public async Task TakeScreenshotAsync()
        {
            string name = Regex.Replace(_scenarioContext.ScenarioInfo.Title, @"\s+", "");
            string path = $"./screenshots/{name}.png";
            await Page.ScreenshotAsync(new() { Path = path });

            AllureApi.AddAttachment(
                name: "Screenshot",
                type: "image/png",
                path: path
            );
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _context = LogContext.PushProperty("Scenario", scenarioContext.ScenarioInfo.Title);
            Log.Information("Starting scenario: {Scenario}", scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            Log.Information("Finished scenario: {Scenario}", scenarioContext.ScenarioInfo.Title);
            _context?.Dispose();
        }
    }

}
