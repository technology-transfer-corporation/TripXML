using VirtualCreditCard.Infrastructure.Seed;

namespace VirtualCreditCard.Extensions
{
    public static class SeedExtension
    {
        public static void SeedDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var seedService = scope.ServiceProvider.GetRequiredService<SeedService>();

                seedService.Seed();
            }
        }
    }
}
