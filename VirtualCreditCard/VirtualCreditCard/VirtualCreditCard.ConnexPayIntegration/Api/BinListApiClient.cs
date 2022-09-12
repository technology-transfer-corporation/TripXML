using Newtonsoft.Json;
using RestSharp;
using System.Net;
using VirtualCreditCard.Core.Constants;
using VirtualCreditCard.Shared.Models;

namespace VirtualCreditCard.ConnexPayIntegration.Api
{
    public class BinListApiClient : IBinListApiClient
    {
        private readonly RestClient _client;

        public BinListApiClient()
        {
            _client = new RestClient(UrlConstants.BinListBaseUrl);
        }

        public async Task<CardTypeRS?> GetCardType(string number)
        {
            var request = new RestRequest(number, Method.GET);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var response = await _client.ExecuteAsync<CardTypeRS>(request);

            if(!response.IsSuccessful)
            {
                throw new Exception($"Binlist error: {response.ResponseUri}, Status: {response.StatusCode}");
            }

            return JsonConvert.DeserializeObject<CardTypeRS>(response.Content.ToString());
        }
    }
    public interface IBinListApiClient
    {
        Task<CardTypeRS?> GetCardType(string number);
    }
}
