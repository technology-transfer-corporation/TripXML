using Microsoft.EntityFrameworkCore;
using VirtualCreditCard.Infrastructure.Entities;

namespace VirtualCreditCard.Infrastructure.Context
{
    public class VccDbContext : DbContext
    {
        public VccDbContext(DbContextOptions<VccDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
    }
}
