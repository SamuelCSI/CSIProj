using Microsoft.EntityFrameworkCore;
using SamuelAPIDemo.Models;

namespace SamuelAPIDemo.Data
{
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options)
        {
        }

        public DbSet<UserInfo> User_table { get; set; }
    }
}
