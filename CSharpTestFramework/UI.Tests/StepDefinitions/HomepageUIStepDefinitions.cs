using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Text;
using UI.Tests.Hooks;
using UI.Tests.Pages;

namespace UI.Tests.StepDefinitions
{
    [Binding]
    internal sealed class HomepageUIStepDefinitions
    {
        private readonly Homepage _homepage;
        private readonly UIHooks _hooks;
        private readonly ContactForm _contactForm;
        private IDownload? _download;
        private string? _downloadPath;
        private bool _postRequestTriggered = false;

        public HomepageUIStepDefinitions(Homepage homepage, UIHooks hooks, ContactForm contactForm)
        {
            _homepage = homepage;
            _hooks = hooks;
            _contactForm = contactForm;
        }

        [Given("a user accessing the homepage")]
        public async Task GivenAUserAccessing()
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

        [When(@"the user clicks the LinkedIn link")]
        public async Task WhenTheUserClicksLinkedInLink()
        {
            await _homepage.LinkedInLink.ClickAsync();
            Log.Information("Clicked LinkedIn link");
        }

        [When(@"the user clicks the Github link")]
        public async Task WhenTheUserClicksGithubLink()
        {
            await _homepage.GithubLink.ClickAsync();
            Log.Information("Clicked Github link");
        }

        [When(@"the user clicks the Projects link")]
        public async Task WhenTheUserClicksProjectsLink()
        {
            await _homepage.ProjectsLink.ClickAsync();
            Log.Information("Clicked Projects link");
        }

        [Then(@"the user is navigated to ""(.*)""")]
        public void ThenTheUserIsNavigatedTo(string expectedUrl)
        {
            string currentUrl = _hooks.Page.Url;
            Assert.That(currentUrl, Does.Contain(expectedUrl));
            Log.Information($"Assertion passed - URL contains: {expectedUrl}");
        }

        [When(@"the user clicks the download CV button")]
        public async Task WhenUserClicksDownloadCV()
        {
            var downloadTask = _hooks.Page.WaitForDownloadAsync();
            await _homepage.DownloadButton.ClickAsync();
            _download = await downloadTask;
            Log.Information("Clicked download CV button");
        }

        [Then(@"the CV file is downloaded successfully")]
        public async Task ThenCVFileIsDownloaded()
        {
            Assert.That(_download, Is.Not.Null, "Download did not start");
            _downloadPath = Path.Combine(
                Path.GetTempPath(),
                "CV_AlexDoherty.pdf"
            );
            try
            {
                await _download!.SaveAsAsync(_downloadPath);
                Assert.Multiple(() =>
                {
                    Assert.That(System.IO.File.Exists(_downloadPath), Is.True, "Downloaded file does not exist");
                    Assert.That(_download.SuggestedFilename, Is.EqualTo("CV_AlexDoherty.pdf"));
                });
                Log.Information($"Assertion passed - CV downloaded successfully to: {_downloadPath}");
            }
            finally
            {
                if (System.IO.File.Exists(_downloadPath))
                {
                    System.IO.File.Delete(_downloadPath);
                    Log.Information("Cleaned up downloaded file");
                }
            }
        }

        [Given(@"the user fills out the contact form with valid data")]
        public async Task GivenUserFillsValidForm()
        {
            await _contactForm.NameInput.FillAsync("John Doe");
            await _contactForm.EmailInput.FillAsync("validmail@gmail.com");
            await _contactForm.MessageInput.FillAsync("Valid message of greater than 20 chars");
            Log.Information("Filled contact form with valid data");
        }

        [When(@"the user submits the form")]
        public async Task WhenUserSubmitsForm()
        {
            await _contactForm.SubmitButton.ClickAsync();
            Log.Information("Submitted contact form");
        }

        [Given(@"the user fills the name field with ""(.*)""")]
        public async Task GivenTheUserFillsNameWith(string name)
        {
            await _contactForm.NameInput.FillAsync(name);
            Log.Information($"Filled name field with: {name}");
        }

        [Given(@"the user fills the email field with ""(.*)""")]
        public async Task GivenTheUserFillsEmailWith(string email)
        {
            await _contactForm.EmailInput.FillAsync(email);
            Log.Information($"Filled email field with: {email}");
        }

        [Given(@"the user fills the message field with ""(.*)""")]
        public async Task GivenTheUserFillsMessageWith(string message)
        {
            await _contactForm.MessageInput.FillAsync(message);
            Log.Information($"Filled message field with: {message}");
        }

        [When(@"the user submits the contact form")]
        public async Task WhenTheUserSubmitsContactForm()
        {
            // Set up request monitoring
            _hooks.Page.Request += (_, request) =>
            {
                if (request.Method == "POST" && request.Url.Contains("/"))
                {
                    _postRequestTriggered = true;
                }
            };

            await _contactForm.SubmitButton.ClickAsync();
            Log.Information("Clicked submit button");
        }

        [Then(@"the name field value is ""(.*)""")]
        public async Task ThenTheNameFieldValueIs(string expectedValue)
        {
            string actualValue = await _contactForm.NameInput.InputValueAsync();
            Assert.That(actualValue, Is.EqualTo(expectedValue));
            Log.Information($"Assertion passed - Name field value is: {expectedValue}");
        }

        [Then(@"the email field value is ""(.*)""")]
        public async Task ThenTheEmailFieldValueIs(string expectedValue)
        {
            string actualValue = await _contactForm.EmailInput.InputValueAsync();
            Assert.That(actualValue, Is.EqualTo(expectedValue));
            Log.Information($"Assertion passed - Email field value is: {expectedValue}");
        }

        [Then(@"the message field value is ""(.*)""")]
        public async Task ThenTheMessageFieldValueIs(string expectedValue)
        {
            string actualValue = await _contactForm.MessageInput.InputValueAsync();
            Assert.That(actualValue, Is.EqualTo(expectedValue));
            Log.Information($"Assertion passed - Message field value is: {expectedValue}");
        }

        [Then(@"no POST request is sent")]
        public void ThenNoPostRequestIsSent()
        {
            Assert.That(_postRequestTriggered, Is.False);
            Log.Information("Assertion passed - No POST request was triggered");
        }

        [Then(@"all contact form fields accept user input")]
        public async Task ThenAllFieldsAcceptInput()
        {
            string nameValue = await _contactForm.NameInput.InputValueAsync();
            string emailValue = await _contactForm.EmailInput.InputValueAsync();
            string messageValue = await _contactForm.MessageInput.InputValueAsync();

            Assert.Multiple(() =>
            {
                Assert.That(nameValue, Is.Not.Empty);
                Assert.That(emailValue, Is.Not.Empty);
                Assert.That(messageValue, Is.Not.Empty);
            });
            Log.Information("Assertion passed - All form fields accept input");
        }
    }
}