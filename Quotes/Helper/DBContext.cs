using Microsoft.EntityFrameworkCore;
using Quotes.Models;

namespace Quotes.Helper
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {

        }

       public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Quote> Quotes { get; set; }

    }
}
