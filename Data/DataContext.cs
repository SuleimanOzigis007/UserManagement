using Microsoft.EntityFrameworkCore;
using UserMangement.Models;

namespace UserMangement.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
