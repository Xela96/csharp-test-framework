using System;
using Core;
using NUnit.Framework.Legacy;
using Reqnroll;
using RestSharp;

namespace API.Tests.StepDefinitions
{
    [Binding]
    public class HomepageStepDefinitions(ScenarioContext scenarioContext)
    {
        private readonly ScenarioContext _scenarioContext = scenarioContext;

        [Given("the homepage address {string}")]
        public void GivenTheHomepageAddress(string homepage)
        {
            _scenarioContext.Add("homepage", homepage);
        }

        [When("the request is sent")]
        public async Task WhenTheRequestIsSent()
        {
            var homepage = _scenarioContext.Get<string>("homepage");
            var client = new RestClient(homepage);
            var service = new RestSharpService(client);
            var response = await service.GetContent();
            _scenarioContext.Add("response", response);
        }

        [Then("the response status should be Ok")]
        public void ThenTheResponseStatusShouldBeOk()
        {
            var response = _scenarioContext.Get<RestResponse>("response");
            ClassicAssert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
