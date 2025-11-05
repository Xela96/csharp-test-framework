using Core;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Data.Tests.StepDefinitions
{
    [Binding]
    public sealed class HomepageDataStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public HomepageDataStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("the homepage_content table is populated")]
        public async Task GivenTheHomepageContentTableIsPopulated()
        {
            Log.Information("Initialising connection with Supabase database");

            var dbClient = new DatabaseClient();
            await dbClient.InitializeAsync();

            var service = new SupabaseService(dbClient);

            Log.Information("Retrieving homepage content from database");
            var homepageContent = await service.GetHomepageContentAsync();

            _scenarioContext["homepageContent"] = (homepageContent);

            ClassicAssert.IsNotEmpty(homepageContent);
            Log.Information("Retrieved homepage content");

        }

        [When("the about_me content is retrieved")]
        public void WhenTheAboutMeContentIsRetrieved()
        {
            Log.Debug("Accessing about_me content from homepage content");

            var homepageContent = _scenarioContext.Get<List<Core.DbModels.HomepageContent>>("homepageContent");
            var aboutMe = homepageContent[0];

            _scenarioContext["aboutMe"] = aboutMe;

        }

        [Then("the text should match the expected value")]
        public void ThenTheTextShouldMatchTheExpectedResult()
        {
            var aboutMe = _scenarioContext.Get<Core.DbModels.HomepageContent>("aboutMe").TextContent;
            const string expectedText = "Welcome to my personal website! Feel free to contact me through the contact me form below. I am a Software Engineer with 4+ years of experience developing automation manufacturing applications and automated test frameworks using <strong>C# .NET</strong> and <strong>Python</strong>. Passionate about test automation, solving real-life problems and making a difference to the world in my career. Currently focused on <strong>test automation</strong> and <strong>quality assurance</strong>. Industry experience in testing includes medical devices, performance and simulation system software and retail. ";

            ClassicAssert.AreEqual(aboutMe, expectedText);
            Log.Information("Assertion passed - Text is matching expected value");
        }
    }
}
