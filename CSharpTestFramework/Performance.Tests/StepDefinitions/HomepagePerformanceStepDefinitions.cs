using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Core;
using NUnit.Framework.Legacy;

namespace Performance.Tests.StepDefinitions
{
    [Binding]
    internal class HomepagePerformanceStepDefinitions (ScenarioContext _scenarioContext)
    {
        [Given("the performance test script (.*)")]
        public void GivenThePerformanceTestScript(string scriptName)
        {
            var executingDirectory = Directory.GetCurrentDirectory();
            var scriptPath = Path.GetFullPath(
                Path.Combine(executingDirectory, "..", "..", "..", "Scripts", scriptName)
            );

            Log.Information($"Executing K6 script {scriptName} with full path {scriptPath}");

            _scenarioContext.Add("scriptPath", scriptPath);
        }

        [When("the load test is executed")] //k6 run --out json=output.json performance.js
        public void WhenTheLoadTestIsExecuted()
        {
            var scriptPath = _scenarioContext.Get<string>("scriptPath");
            var baseUrl = TestConfig.BaseUrl;

            Log.Information($"Executing script for load test");

            try
            {
                var p = new Process
                {
                    StartInfo =
                {
                    FileName = "k6",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments = $"run {scriptPath}"
                }
                };
                p.StartInfo.EnvironmentVariables["BASE_URL"] = baseUrl;
                p.Start();

                StreamReader reader = p.StandardOutput;

                string output = reader.ReadToEnd();
                string error = p.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine("K6 Errors:");
                    Console.WriteLine(error);
                    _scenarioContext.Add("error", error);

                    Log.Error($"Error with execution of K6 script: {error}");
                }

                p.WaitForExit();

                _scenarioContext.Add("output", output);
            }
            catch (Exception ex)
            {
                Log.Error($"Execution of K6 script failed.", ex);
                throw new InvalidOperationException("K6 script, see logs for more details.", ex);
            }
        }

        [Then("the appropriate status code should be returned")]
        public void ThenTheAppropriateStatusCodeShouldBeReturned()
        {
            var output = _scenarioContext.Get<string>("output");
            var match1 = Regex.IsMatch(output, "status is 200");

            
            var match2 = Regex.IsMatch(output, "checks_succeeded...: 100.00%");

            ClassicAssert.True(match1);
            ClassicAssert.True(match2);
            Log.Information("Assertions passed - Status code 200 has been returned and tests passed");
        }
    }
}
