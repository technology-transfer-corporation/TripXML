using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using TripXML.Core.Models.ConnexPay;
using VirtualCreditCard.ConnexPayIntegration.Interfaces;
using VirtualCreditCard.Core.Constants;
using VirtualCreditCard.Core.Service.Interfaces;

namespace VirtualCreditCard.ConnexPayIntegration.Api
{
    public class BaseConnexPayAuth : IConnexPayAuthService
    {
        protected string TokenKey { get; set; } = null!;
        protected string GetTokenUrl { get; set; } = null!;

        protected readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        public BaseConnexPayAuth(
            IStorageService storageService, 
            IConfiguration configuration
            )
        {
            _storageService = storageService;
            _configuration = configuration;
        }

        public async Task<string> RefreshToken()
        {
            var client = new RestClient(GetTokenUrl);
            var request = new RestRequest()
                .AddParameter("grant_type", "password")
                .AddParameter("username", _configuration[ConfigurationConstants.ConnexPayUsername])
                .AddParameter("password", _configuration[ConfigurationConstants.ConnexPayPassword]);
            request.Method = Method.POST;

            var response = await client.ExecuteAsync(request);

            var authResponse = JsonConvert.DeserializeObject<ConnexPayAuthResponse>(
                response.Content.ToString());

            if (!response.IsSuccessful || string.IsNullOrEmpty(authResponse?.AccessToken))
            {
                throw new Exception("ConnexPay authorization error");
            }

            string bearerToken = "Bearer " + authResponse.AccessToken;

            SetToken(bearerToken);
            return bearerToken;
        }

        public string? GetToken() => _storageService.TryGet<string>(TokenKey);

        private void SetToken(string token)
        {
            _storageService.Set(TokenKey, token);
        }
    }
}
