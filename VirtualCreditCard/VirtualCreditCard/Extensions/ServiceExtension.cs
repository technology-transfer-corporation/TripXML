using VirtualCreditCard.ConnexPayIntegration.Api;
using VirtualCreditCard.Core.Service;
using VirtualCreditCard.Core.Service.Interfaces;
using VirtualCreditCard.Infrastructure.Seed;
using VirtualCreditCard.Infrastructure.Service;
using VirtualCreditCard.Service;

namespace VirtualCreditCard.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<BinListService>()
                .AddSingleton<ConnexPayPurchasesService>()
                .AddSingleton<IUrlService, UrlService>()
                .AddSingleton<IStorageService, LocalStorageService>()
                .AddSingleton<ConnexPayPurchaseAuthService>()
                .AddSingleton<ConnexPaySalesAuthService>()
                .AddTransient<IConnexPayPurchasesApiClient, ConnexPayPurchasesApiClient>()
                .AddTransient<IConnexPaySalesApiClient, ConnexPaySalesApiClient>()
                .AddTransient<IConnexPaySalesService, ConnexPaySalesService>()
                .AddTransient<IConnexPayService, ConnexPayPurchasesService>()
                .AddTransient<IBinListApiClient, BinListApiClient>()
                .AddTransient<IBinListService, BinListService>()
                .AddScoped<ITokenService, TokenService>()
                .AddTransient<IUserService, UserService>()
                .AddScoped<SeedService>();
        }
    }
}
