
namespace VirtualCreditCard.ConnexPayIntegration.Interfaces
{
    public interface IConnexPayAuthService
    {
        string? GetToken();
        Task<string> RefreshToken();
    }
}