using System;
using System.Diagnostics;
using NUnit.Framework;
using System.Text.RegularExpressions;
using Reqnroll;
using NUnit.Framework.Legacy;

namespace Performance.Tests.StepDefinitions
{
    [Binding]
    public class HomepagePerformanceStepDefinitions (ScenarioContext _scenarioContext)
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

            Log.Information($"Executing script for load test");

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
            p.Start();

            StreamReader reader = p.StandardOutput;

            string output = reader.ReadToEnd();
            string error = p.StandardError.ReadToEnd();

            //log output/error here

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

        [Then("the appropriate status code should be returned")]
        public void ThenTheAppropriateStatusCodeShouldBeReturned()
        {
            var output = _scenarioContext.Get<string>("output");
            var match = Regex.IsMatch(output, "status is 200");

            ClassicAssert.True(match);
            Log.Information("Assertion passed - Status code 200 has been returned");
        }
    }
}
