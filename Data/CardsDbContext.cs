using CardsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CardsAPI.Data
{
    public class CardsDbContext : DbContext
    {
        public CardsDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Cards> Cards { get; set; } 
    }
}
