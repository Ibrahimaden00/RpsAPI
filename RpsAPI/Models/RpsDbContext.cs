using Microsoft.EntityFrameworkCore;
using RpsAPI.Models;

namespace RpsAPI.Models
{
    public class RpsDbContext : DbContext
    {
        public RpsDbContext(DbContextOptions<RpsDbContext> options) : base(options) { }

        public DbSet<UserItem> UserItem { get; set; } = null!;

        public DbSet<MessageItem > MessageItem { get; set; } = null!;   

        public DbSet<GameItem> GameItem { get; set; }=null!;
        
    }
   
}
