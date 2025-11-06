using RestSharp;
using Serilog;

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
    }
}