using Core;
using Serilog.Context;

namespace API.Tests.Hooks
{
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
