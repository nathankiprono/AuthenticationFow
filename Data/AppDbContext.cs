using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AuthenticationFlow.Models;

namespace AuthenticationFlow.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
    }
}
