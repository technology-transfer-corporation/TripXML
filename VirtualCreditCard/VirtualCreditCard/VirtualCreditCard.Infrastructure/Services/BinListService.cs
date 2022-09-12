using VirtualCreditCard.ConnexPayIntegration.Api;
using VirtualCreditCard.Shared.Models;

namespace VirtualCreditCard.Infrastructure.Service
{
    public class BinListService : IBinListService
    {
        private readonly IBinListApiClient _binListApiClient;

        public BinListService(IBinListApiClient binListApiClient)
        {
            _binListApiClient = binListApiClient;
        }

        public async Task<CardTypeRS?> GetCardType(string number)
        {
            return await _binListApiClient.GetCardType(number);
        }
    }

    public interface IBinListService
    {
        Task<CardTypeRS?> GetCardType(string number);
    }
}
