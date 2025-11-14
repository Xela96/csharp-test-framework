using System;
using Core;
using NUnit.Framework.Legacy;
using Reqnroll;
using RestSharp;

namespace API.Tests.StepDefinitions
{
    [Binding]
    internal class HomepageAPIStepDefinitions(ScenarioContext scenarioContext)
    {
        private readonly ScenarioContext _scenarioContext = scenarioContext;

        [Given("the homepage address")]
        public void GivenTheHomepageAddress()
        {
            var homepage = TestConfig.BaseUrl;
            Log.Information($"Setting homepage address to {homepage}");
            _scenarioContext.Add("homepage", homepage);
        }

        [When("the request is sent")]
        public async Task WhenTheRequestIsSent()
        {
            var homepage = _scenarioContext.Get<string>("homepage");
            var client = new RestClient(homepage);
            var service = new RestSharpService(client);

            Log.Debug($"Sending GET request to {homepage}");
            var response = await service.GetContent();

            Log.Information($"Received response with status {response.StatusCode}");
            _scenarioContext.Add("response", response);
        }

        [Then("the response status should be Ok")]
        public void ThenTheResponseStatusShouldBeOk()
        {
            var response = _scenarioContext.Get<RestResponse>("response");
            ClassicAssert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            Log.Information("Assertion passed - status code is OK");
        }
    }
}
