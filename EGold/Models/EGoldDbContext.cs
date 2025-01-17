using Microsoft.EntityFrameworkCore;

namespace EGold.Models
{
    public class EGoldDbContext : DbContext
    {
        public EGoldDbContext(DbContextOptions<EGoldDbContext> options) : base(options)
        {

        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TokenEntity> Tokens { get; set; }
        public DbSet<UserTokenEntity> UserTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<UserTokenEntity>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTokens)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<UserTokenEntity>()
                .HasOne(ut => ut.Token)
                .WithMany()
                .HasForeignKey(ut => ut.TokenId);
        }
    }
}
