using RestSharp;

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
            var response = await _siteClient.ExecuteAsync(new RestRequest($"/{path}", Method.Get));
            return response;
        }
    }
}