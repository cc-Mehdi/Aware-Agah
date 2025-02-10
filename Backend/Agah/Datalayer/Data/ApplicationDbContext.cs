using Datalayer.Models;
using Datalayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Datalayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductLog> ProductLog { get; set; }
        public DbSet<Alarm> Alarm { get; set; }
        public DbSet<Reserve> Reserve { get; set; }
        public DbSet<Notification_User> Notification_User { get; set; }
    }
}
