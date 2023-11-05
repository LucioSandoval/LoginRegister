using Microsoft.EntityFrameworkCore;

namespace LoginRegistration.Models
{
    public class MyContext : DbContext
    {   
        public DbSet<User> Users { get; set; }
        public MyContext(DbContextOptions options) : base(options) { }
        
    }
}
