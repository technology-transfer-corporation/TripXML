using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.Text;
using TripXML.Core.Models;
using TripXML.Core.Results;
using VirtualCreditCard.ConnexPayIntegration.Interfaces;
using VirtualCreditCard.Shared.DTO;

namespace VirtualCreditCard.ConnexPayIntegration.Api
{
    public class BaseApiClient
    {
        protected RestClient Client { get; set; } = null!;
        protected IConnexPayAuthService ConnexPayAuthService { get; set; } = null!;

        protected async Task<IResult<T>> ExecuteRequest<T>(IRestRequest restRequest)
        {
            IRestResponse response = await Client.ExecuteAsync(restRequest);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await UpdateToken(restRequest);
                response = await Client.ExecuteAsync(restRequest);
            }

            if (!response.IsSuccessful)
            {
                ConnexPayError? error;
                try
                {
                    error = JsonConvert.DeserializeObject<ConnexPayError>(response.Content);
                } catch
                {
                    error = new ConnexPayError();
                }

                if (error?.ModelState != null && error.ModelState.Count > 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(error.Message);
                    foreach (var item in error.ModelState)
                    {
                        foreach (var stateMessage in item.Value)
                        {
                            stringBuilder.Append($" {stateMessage}.");
                        }
                    }

                    error.Message = stringBuilder.ToString();
                }

                return Result<T>.Fail(new List<Error>
                {
                    new Error
                    {
                        Code =response.StatusCode.ToString(),
                        Value = error?.Message ?? response.Content,
                        ShortText = response.ErrorMessage,
                        RecordID = error?.ErrorId.ToString()
                    }
                });
            }

            var responseObj = JsonConvert.DeserializeObject<T>(response.Content.ToString());
            return Result<T>.Success(responseObj!);
        }

        protected async Task<IRestRequest> CreatePostRequest(string urlSegment, object body)
        {
            var request = new RestRequest(urlSegment, Method.POST);
            await FillRequestCommonHeaders(request);

            string jsonToSend = JsonConvert.SerializeObject(body, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);

            return request;
        }

        protected async Task<IRestRequest> CreateGetRequest(string urlSegment)
        {
            var request = new RestRequest(urlSegment, Method.GET);
            await FillRequestCommonHeaders(request);
            return request;
        }

        protected async Task FillRequestCommonHeaders(IRestRequest restRequest)
        {
            string? token = ConnexPayAuthService.GetToken();
            if (token == null)
            {
                token = await ConnexPayAuthService.RefreshToken();
            }

            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddOrUpdateHeader("Authorization", token);
        }

        protected async Task UpdateToken(IRestRequest restRequest)
        {
            string token = await ConnexPayAuthService.RefreshToken();
            restRequest.AddOrUpdateHeader("Authorization", token);
        }
    }
}
