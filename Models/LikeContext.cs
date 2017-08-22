
using Microsoft.EntityFrameworkCore;
 
namespace Ideas.Models
{
    public class LikeContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public LikeContext(DbContextOptions<LikeContext> options) : base(options) { }
        public DbSet<Users> Users {get;set;}
        public DbSet<Concepts> Concepts {get;set;}
        public DbSet<Likes> Likes {get;set;}
    }
}