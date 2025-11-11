using Core;
using Serilog;
using Serilog.Context;

namespace Data.Tests.Hooks
{
    [Binding]
    public class DataHooks
    {
        private IDisposable? _context;

        [BeforeTestRun]
        public static void DataTestsSetup()
        {
            Logging.ConfigureLogging();

            Log.Information("Starting Data tests");
        }

        [AfterTestRun]
        public static void DataTestsTeardown()
        {
            Log.Information("Ending Data tests");

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
