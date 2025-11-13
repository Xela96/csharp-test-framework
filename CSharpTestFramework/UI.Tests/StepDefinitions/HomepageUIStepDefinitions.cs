using NUnit.Framework.Legacy;
using UI.Tests.Pages;

namespace UI.Tests.StepDefinitions
{
    [Binding]
    internal sealed class HomepageUIStepDefinitions(Homepage homepage)
    {
        private readonly Homepage _homepage = homepage;

        [Given("a user accessing {string}")]
        public async Task GivenAUserAccessing(string address)
        {
            Log.Information("Accessing homepage of web application");
            await _homepage.GoToAsync();
        }

        [When("the page loads")]
        public async Task WhenThePageLoads()
        {
            await _homepage.WaitForPageAsync();
            Log.Information("Homepage has loaded");
        }

        [Then("the page title is correct")]
        public async Task ThenThePageTitleIsCorrect()
        {
            string title = await _homepage.GetTitleAsync();
            ClassicAssert.AreEqual("Alex Doherty", title);
            Log.Information("Assertion passed - Homepage has loaded with correct content verified");
        }
    }
}
