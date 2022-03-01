using MessageWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageWebAPI.Repositories
{
    public class CoreDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options) { }
    }
}
