using Allure.Net.Commons;
using Core;
using Reqnroll;
using Reqnroll.Assist;
using RestSharp;
using Serilog.Context;
using System.Net;
using System.Text.RegularExpressions;

namespace API.Tests.Hooks
{
    [Binding]
    public class APIHooks
    {
        private IDisposable? _context;

        [BeforeTestRun]
        public static void ApiTestsSetup()
        {
            Logging.ConfigureLogging();

            Log.Information("Starting API tests");
        }

        [AfterTestRun]
        public static void ApiTestsTeardown()
        {
            Log.Information("Ending API tests");

            Log.CloseAndFlush();

            string sourceFolder = "allure-results";
            string destinationFolder = "../../../../allure-results";
            Core.File.MoveDirectoryFiles(sourceFolder, destinationFolder);
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


        [BeforeScenario("@requiresCsrf", Order = 0)]
        public async Task GetCsrfToken(ScenarioContext scenarioContext)
        {
            var homepage = TestConfig.BaseUrl;
            var cookieContainer = new CookieContainer();
            var options = new RestClientOptions(homepage)
            {
                CookieContainer = cookieContainer,
                ThrowOnAnyError = false
            };

            var client = new RestClient(options);

            var service = new RestSharpService(client);

            Log.Information("Sending GET request to get CSRF token");
            var response = await service.GetContent();
            var html = response.Content;

            var match = Regex.Match(response.Content, "name=\"csrf_token\"[^>]*value=\"([^\"]+)\"");

            string csrfToken = match.Groups[1].Value;
            scenarioContext.Add("csrfToken", csrfToken);

            // Same session for token to work
            scenarioContext["siteClient"] = client;

        }

        [BeforeScenario("@requiresLogin", Order = 10)]
        public async Task LogIn(ScenarioContext scenarioContext)
        {
            var homepage = TestConfig.BaseUrl;

            var client = scenarioContext.Get<RestClient>("siteClient");
            var service = new RestSharpService(client);

            Log.Information("Sending POST request to log in");
            var response = await service.PostLogin(csrfToken: scenarioContext.Get<string>("csrfToken"));

            var cookies = client.Options.CookieContainer?.GetCookies(new Uri(homepage));
            Log.Debug("Cookies: " + string.Join(", ", cookies?.Cast<Cookie>() ?? Enumerable.Empty<Cookie>()));

            var html = response.Content;

        }
    }
}
