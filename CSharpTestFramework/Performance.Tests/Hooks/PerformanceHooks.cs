using Core;
using Serilog;
using Serilog.Context;
using System.Diagnostics;

namespace Performance.Tests.Hooks
{
    [Binding]
    public class PerformanceHooks
    {
        private IDisposable? _context;

        [BeforeTestRun]
        public static void PerformanceTestsSetup()
        {
            Logging.ConfigureLogging();

            Log.Information("Starting Performance tests");
        }

        [AfterTestRun]
        public static void PerformanceTestsTeardown()
        {
            Log.Information("Ending Performance tests");

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
    }
}
