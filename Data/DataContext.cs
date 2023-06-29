using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class DataContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<UserLike> Likes { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserLike>()
                .HasOne(x => x.SourceUser)
                .WithMany(x => x.LikedUsers)
                .HasForeignKey(x => x.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<UserLike>()
                .HasOne(x => x.LikedUser)
                .WithMany(x => x.LikedByUsers)
                .HasForeignKey(x => x.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}