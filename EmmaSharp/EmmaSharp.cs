using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers;
using System;
using System.Threading.Tasks;

namespace EmmaSharp
{
    /// <summary>
    /// Base Class for APIs
    /// </summary>
	public partial class EmmaApi
    {
        private const string BaseUrl = "https://api.e2ma.net";

        readonly string _accountId;
        readonly EmmaJsonSerializer _serializer;
        readonly HttpBasicAuthenticator _authenticator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmmaApi"/> class.
        /// </summary>
        /// <param name="publicKey">The account's public key.</param>
        /// <param name="secretKey">The account's private key.</param>
        /// <param name="accountId">The account id.</param>
        public EmmaApi(string publicKey, string secretKey, string accountId)
        {
            _accountId = accountId;
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new StringEnumConverter());
            _serializer = new EmmaJsonSerializer(serializer);
            _authenticator = new HttpBasicAuthenticator(publicKey, secretKey);
        }


        /// <summary>
        /// Execute the Call to the Emma API. All methods return this base method.
        /// </summary>
        /// <typeparam name="T">The model or type to bind the return response.</typeparam>
        /// <param name="request">The RestRequest request.</param>
        /// <param name="start">If more than 500 results, use these parameters to start/end pages.</param>
        /// <param name="end">If more than 500 results, use these parameters to start/end pages.</param>
        /// <returns>Response data from the API call.</returns>
        private async Task<T> Execute<T>(RestRequest request, int start = -1, int end = -1) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(BaseUrl);

            client.Authenticator = _authenticator;
            request.AddParameter("accountId", _accountId, ParameterType.UrlSegment); // used on every request

            if (start >= 0 && end >= 0) {
                request.AddQueryParameter("start", start.ToString());
                request.AddQueryParameter("end", end.ToString());
            }

            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = _serializer;

            var execute = await client.ExecuteAsync<T>(request).ConfigureAwait(false);
            checkResponse(execute);
            
            T response = JsonConvert.DeserializeObject<T>(execute.Content);
            return response;
        }

        private void checkResponse(IRestResponse response)
        {
            int code = (int)response.StatusCode;
            if (code >= 400)
            {
                throw new EmmaException(response);
            }
        }
    }
}