using Microsoft.Extensions.Configuration;
using VirtualCreditCard.Infrastructure.Context;
using VirtualCreditCard.Infrastructure.Entities;

namespace VirtualCreditCard.Infrastructure.Seed
{
    public class SeedService
    {
        private readonly VccDbContext _vccDbContext;
        private readonly IConfiguration _configuration;

        public SeedService(VccDbContext vccDbContext, IConfiguration configuration)
        {
            _vccDbContext = vccDbContext;
            _configuration = configuration;
        }

        public void Seed()
        {
            if(!_vccDbContext.Users.Any())
            {
                var trimXMLUser = new User()
                {
                    Id = 1,
                    Username = "TripXML",
                    Password = _configuration["Users:TripXMLPassword"]
                };

                _vccDbContext.Users.Add(trimXMLUser);
                _vccDbContext.SaveChanges();
            }
        }
    }
}
