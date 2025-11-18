using Core;
using NUnit.Framework.Legacy;
using RestSharp;
using System.Net;

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

        [Given("the homepage address with a logged in user")]
        public void GivenTheHomepageAddressWithALoggedInUser()
        {
            var homepage = TestConfig.BaseUrl;
            Log.Information($"Setting homepage address to {homepage}");
            _scenarioContext.Add("homepage", homepage);
        }

        [Given("the homepage address with route {string}")]
        public void GivenTheHomepageAddressWithRoute(string route)
        {
            var address = TestConfig.BaseUrl + route;
            Log.Information($"Setting homepage address to {address}");
            _scenarioContext.Add("homepage", address);
        }

        [When("the {word} request is sent with logout")]
        public async Task WhenTheRequestIsSentWithLogout(string method)
        {
            var homepage = _scenarioContext.Get<string>("homepage");
            var client = _scenarioContext.Get<RestClient>("siteClient");
            var service = new RestSharpService(client);
            RestResponse response;

            switch (method)
            {
                case "GET":
                    Log.Debug($"Sending GET request to {homepage}");
                    response = await service.GetContent();

                    Log.Information($"Received response with status {response.StatusCode}");
                    _scenarioContext.Add("getResponse", response);
                    break;

                case "POST":
                    Log.Debug($"Sending POST request to {homepage}");
                    response = await service.PostContent();

                    Log.Information($"Received response with status {response.StatusCode}");
                    _scenarioContext.Add("postResponse", response);
                    break;

                case "DELETE":
                    Log.Debug($"Sending DELETE request to {homepage}");
                    response = await service.DeleteContent();

                    Log.Information($"Received response with status {response.StatusCode}");
                    _scenarioContext.Add("deleteResponse", response);
                    break;

                default:
                    throw new ArgumentException($"Unsupported HTTP method: {method}");
            }

        }

        [When("the {word} request is sent")]
        public async Task WhenTheRequestIsSent(string method)
        {
            var homepage = _scenarioContext.Get<string>("homepage");
            var client = new RestClient(homepage);
            var service = new RestSharpService(client);
            RestResponse response;

            switch (method)
            {
                case "GET":
                    Log.Debug($"Sending GET request to {homepage}");
                    response = await service.GetContent();                    

                    Log.Information($"Received response with status {response.StatusCode}");
                    _scenarioContext.Add("getResponse", response);
                    break;

                case "POST":
                    Log.Debug($"Sending POST request to {homepage}");
                    response = await service.PostContent();

                    Log.Information($"Received response with status {response.StatusCode}");
                    _scenarioContext.Add("postResponse", response);
                    break;

                case "DELETE":
                    Log.Debug($"Sending DELETE request to {homepage}");
                    response = await service.DeleteContent();

                    Log.Information($"Received response with status {response.StatusCode}");
                    _scenarioContext.Add("deleteResponse", response);
                    break;

                default:
                    throw new ArgumentException($"Unsupported HTTP method: {method}");
            }
                
        }

        [When("the POST request is sent with contact form body:")]
        public async Task WhenThePOSTRequestIsSentWithContactFormBody(DataTable? table = null)
        {
            string name = "";
            string email = "";
            string message = "";

            var homepage = _scenarioContext.Get<string>("homepage");
            var client = _scenarioContext.Get<RestClient>("siteClient");
            var service = new RestSharpService(client);
            RestResponse response;

            if (table != null && table.Rows.Count > 0)
            {
                var row = table.Rows[0];
                if (row.ContainsKey("name")) name = row["name"];
                if (row.ContainsKey("email")) email = row["email"];
                if (row.ContainsKey("message")) message = row["message"];
            }


            Log.Debug($"Sending POST request to {homepage} with valid contact form fields");
            response = await service.PostContactForm(name: name, email: email, message: message, csrfToken: _scenarioContext.Get<string>("csrfToken"));

            Log.Information($"Received response with status {response.StatusCode}");
            _scenarioContext.Add("postResponse", response);

        }

        [When("the POST request is sent with contact form body")]
        public async Task WhenThePOSTRequestIsSentWithContactFormBody()
        {
            var homepage = _scenarioContext.Get<string>("homepage");
            var client = _scenarioContext.Get<RestClient>("siteClient");
            var service = new RestSharpService(client);
            RestResponse response;

            Log.Debug($"Sending POST request to {homepage} with valid contact form fields");
            response = await service.PostContactForm(csrfToken: _scenarioContext.Get<string>("csrfToken"));

            Log.Information($"Received response with status {response.StatusCode}");
            _scenarioContext.Add("postResponse", response);

        }


        [Then("the response body contains \\/download_cv")]
        public void ThenTheResponseBodyContainsDownload_Cv()
        {
            var response = _scenarioContext.Get<RestResponse>("getResponse");
            var responseBody = response.Content;
            var result = responseBody?.Contains("/download_cv");

            ClassicAssert.True(result);
            Log.Information("Assertion passed - response body contains /download_cv");

        }

        [Then("the response body contains external links")]
        public void ThenTheResponseBodyContainsExternalLinks()
        {
            var response = _scenarioContext.Get<RestResponse>("getResponse");
            var responseBody = response.Content;
            var resultGithub = responseBody?.Contains("https://www.linkedin.com/in/alex-doherty");
            var resultLinkedIn = responseBody?.Contains("https://github.com/Xela96");

            ClassicAssert.True(resultGithub);
            ClassicAssert.True(resultLinkedIn);
            Log.Information("Assertions passed - response body contains valid links");

        }

        [Then("the response body contains projects page link")]
        public void ThenTheResponseBodyContainsProjectsPageLink()
        {
            var response = _scenarioContext.Get<RestResponse>("getResponse");
            var responseBody = response.Content;
            var result = responseBody?.Contains("/projects");

            ClassicAssert.True(result);
            Log.Information("Assertion passed - response body contains /projects");
        }

        [Then("the response Content-Type is pdf")]
        public void ThenTheResponseContent_TypeIsPdf()
        {
            var response = _scenarioContext.Get<RestResponse>("getResponse");
            var responseContentType = response.ContentType;

            ClassicAssert.AreEqual(responseContentType, "application/pdf");
            Log.Information("Assertion passed - response ContentType is PDF");
        }

        [Then("the response filename is correct")]
        public void ThenTheResponseFilenameIsCorrect()
        {
            var response = _scenarioContext.Get<RestResponse>("getResponse");
            var contentDisposition = response.ContentHeaders?
                .FirstOrDefault(h => h.Name.Equals("Content-Disposition", StringComparison.OrdinalIgnoreCase))
                ?.Value?
                .ToString();

            var result = contentDisposition?.Contains("CV_AlexDoherty");

            ClassicAssert.True(result);
            Log.Information("Assertion passed - response Content-Disposition header contains CV title");
        }

        [Then("the file is not empty")]
        public void ThenTheFileIsNotEmpty()
        {
            var response = _scenarioContext.Get<RestResponse>("getResponse");
            var contentLength = response.ContentHeaders?
                .FirstOrDefault(h => h.Name.Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
                ?.Value?
                .ToString();

            int length = int.Parse(contentLength);
            var result = length > 1000 ? true : false;

            ClassicAssert.True(result);
            Log.Information("Assertion passed - response Content-Length > 1000 (not empty file)");
        }

        [Then(@"the (.*) response status is (\d+)")]
        public void ThenTheResponseStatusIs(string responseKey, int expectedStatus)
        {
            var response = _scenarioContext.Get<RestResponse>(responseKey);

            ClassicAssert.AreEqual((System.Net.HttpStatusCode)expectedStatus, response.StatusCode);

            Log.Information($"Assertion passed - status code is {expectedStatus}");
        }


        [Then("the response body contains logout link")]
        public void ThenTheResponseBodyContainsLogoutLink()
        {
            throw new PendingStepException();
        }


    }
}
