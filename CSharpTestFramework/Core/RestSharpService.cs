using RestSharp;
using Serilog;
using System.Text;

namespace Core
{
    public class RestSharpService
    {
        private readonly RestClient _siteClient;

        public RestSharpService(RestClient siteClient)
        {
            _siteClient = siteClient;
        }

        public async Task<RestResponse> GetContent(string path="")
        {
            try
            {
                var response = await _siteClient.ExecuteAsync(new RestRequest($"/{path}", Method.Get));
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to send REST request.", ex);
                throw new InvalidOperationException("Rest request failed, see logs for more details.", ex);
            }
            
        }

        public async Task<RestResponse> PostContent(string path = "")
        {
            try
            {
                var response = await _siteClient.ExecuteAsync(new RestRequest($"/{path}", Method.Post));
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to send REST request.", ex);
                throw new InvalidOperationException("Rest request failed, see logs for more details.", ex);
            }

        }

        public async Task<RestResponse> PostContactForm(string path = "/", string name = "Joe Bloggs", string email = "validmail@gmail.com", string message = "Message of minimum twenty characters.", string csrfToken = "")
        {
            try
            {
                var request = new RestRequest($"/{path}", Method.Post);
                request.AddParameter("name", name);
                request.AddParameter("email", email);
                request.AddParameter("message", message);
                request.AddParameter("csrf_token", csrfToken);
                request.AddHeader("referer", TestConfig.BaseUrl);

                var response = await _siteClient.ExecuteAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to send REST request.", ex);
                throw new InvalidOperationException("Rest request failed, see logs for more details.", ex);
            }

        }

        public async Task<RestResponse> PostLogin(string path = "/login", string name = "Joe Bloggs", string email = "validmail@gmail.com", string message = "Message of minimum twenty characters.", string csrfToken = "")
        {
            try
            {
                var request = new RestRequest($"/{path}", Method.Post);
                request.AddParameter("csrf_token", csrfToken);
                request.AddHeader("referer", TestConfig.BaseUrl);

                request.AddHeader("Authorization",
                    "Basic " + Convert.ToBase64String(
                        Encoding.UTF8.GetBytes($"{TestConfig.Username}:{TestConfig.Password}")
                    )
                );

                var response = await _siteClient.ExecuteAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to send REST request.", ex);
                throw new InvalidOperationException("Rest request failed, see logs for more details.", ex);
            }

        }

        public async Task<RestResponse> DeleteContent(string path = "")
        {
            try
            {
                var response = await _siteClient.ExecuteAsync(new RestRequest($"/{path}", Method.Delete));
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to send REST request.", ex);
                throw new InvalidOperationException("Rest request failed, see logs for more details.", ex);
            }

        }

    }
}