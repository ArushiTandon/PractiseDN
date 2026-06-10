using Microsoft.EntityFrameworkCore;
using PractiseDN.Models;

namespace PractiseDN.Data
{
  public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
  {
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }
    }
    
}
