using Allure.Net.Commons;
using Core;
using DotNetEnv;
using Google.Apis.Gmail.v1;
using Microsoft.Playwright;
using Serilog;
using Serilog.Context;
using System.IO;
using System.Text.RegularExpressions;

namespace UI.Tests.Hooks
{
    [Binding]
    public class UIHooks(ScenarioContext scenarioContext)
    {
        private readonly ScenarioContext _scenarioContext = scenarioContext;
        public IPage Page => _scenarioContext.Get<IPage>("Page");
        private IDisposable? _context;
        private IBrowser? _browser;
        private IBrowserContext? _browserContext;

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

        [BeforeScenario(Order = 1)]
        public async Task SetupTestAsync()
        {
            IPlaywright playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
            _browserContext = await _browser.NewContextAsync();
            IPage page = await _browserContext.NewPageAsync();

            _scenarioContext.Set(page, "Page");
        }

        [AfterScenario]
        public async Task TakeScreenshotAsync()
        {
            if (_scenarioContext.ContainsKey("Page"))
            {
                IPage page = _scenarioContext.Get<IPage>("Page");
                string name = Regex.Replace(_scenarioContext.ScenarioInfo.Title, @"\s+", "");
                string path = $"./screenshots/{name}.png";

                Directory.CreateDirectory("./screenshots");

                await page.ScreenshotAsync(new() { Path = path });
                AllureApi.AddAttachment(
                    name: "Screenshot",
                    type: "image/png",
                    path: path
                );
            }
        }

        [AfterScenario]
        public async Task CleanupAsync()
        {
            if (_browserContext != null)
            {
                await _browserContext.CloseAsync();
            }
            if (_browser != null)
            {
                await _browser.CloseAsync();
            }
        }

        [BeforeScenario(Order = 2)]
        public void BeforeScenario()
        {
            _context = LogContext.PushProperty("Scenario", _scenarioContext.ScenarioInfo.Title);
            Log.Information("Starting scenario: {Scenario}", _scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Log.Information("Finished scenario: {Scenario}", _scenarioContext.ScenarioInfo.Title);
            _context?.Dispose();
        }
    }
}