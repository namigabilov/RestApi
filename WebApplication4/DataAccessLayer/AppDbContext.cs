using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using WebApplication4.Entities;

namespace WebApplication4.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext( DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products{ get; set; }
    }
}
