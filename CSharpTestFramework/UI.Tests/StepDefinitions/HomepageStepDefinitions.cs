using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using UI.Tests.Pages;

namespace UI.Tests.StepDefinitions
{
    [Binding]
    public sealed class HomepageStepDefinitions(Hooks hooks, Homepage homepage)
    {
        private readonly ScenarioContext _context;
        private readonly IPage _page = hooks.Page;
        private readonly Homepage _homepage = homepage;


        [Given("a user accessing {string}")]
        public async Task GivenAUserAccessing(string address)
        {
            await _homepage.GoToAsync();
        }

        [When("the page loads")]
        public async Task WhenThePageLoads()
        {
            await _homepage.WaitForPageAsync();
        }

        [Then("the page title is correct")]
        public async Task ThenThePageTitleIsCorrect()
        {
            string title = await _homepage.GetTitleAsync();
            ClassicAssert.AreEqual("Alex Doherty", title);
        }
    }
}
