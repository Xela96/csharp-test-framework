using Core;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Data.Tests.StepDefinitions
{
    [Binding]
    public sealed class HomepageStepDefinitions
    {

        [Given("the homepage_content table is populated")]
        public async Task GivenTheHomepageContentTableIsPopulated()
        {
            var dbClient = new DatabaseClient();
            await dbClient.InitializeAsync();

            var service = new SupabaseService(dbClient);
            var homepageContent = await service.GetHomepageContentAsync();

            ClassicAssert.IsNotEmpty(homepageContent);
        }

        [When("the homepage is opened")]
        public void WhenTheHomepageIsOpened()
        {
            //TODO: implement act (action) logic

            throw new PendingStepException();
        }

        [Then("the about me content should be visible and matching the database")]
        public void ThenTheAboutMeContentShouldBeVisibleAndMatchingTheDatabase()
        {
            //TODO: implement assert (verification) logic

            throw new PendingStepException();
        }
    }
}
