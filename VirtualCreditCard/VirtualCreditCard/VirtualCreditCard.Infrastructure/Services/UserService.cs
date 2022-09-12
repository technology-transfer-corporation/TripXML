using Microsoft.EntityFrameworkCore;
using VirtualCreditCard.Infrastructure.Context;
using VirtualCreditCard.Infrastructure.Entities;

namespace VirtualCreditCard.Infrastructure.Service
{
    public class UserService : IUserService
    {
        private readonly VccDbContext _vccDbContext;
        public UserService(VccDbContext vccDbContext)
        {
            _vccDbContext = vccDbContext;
        }

        public async Task<User?> FindUserByNameAsync(string username)
        {
            return await _vccDbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        }
    }

    public interface IUserService
    {
        Task<User?> FindUserByNameAsync(string username);
    }
}
