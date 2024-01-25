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
        public DbSet<Session> Sessions { get; set; }    
        public DbSet<SessionClass> SessionClasses { get; set; }

        public DbSet<SessionClassOne>  sessionClassesOne { get; set; }

        public DbSet<SessionOne> sessionOnes { get; set; }
        }
}
