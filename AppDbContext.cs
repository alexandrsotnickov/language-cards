using LanguageCards.Api.Entities;
using LanguageCards.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace MyRestApi
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        {
        }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<UserCardStatus> UserCardsStatuses { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<User>().ToTable("users");
            builder.Entity<IdentityRole>().ToTable("roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("user_roles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("user_claims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("user_logins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("role_claims");
            builder.Entity<IdentityUserToken<string>>().ToTable("user_tokens");

            builder.Entity<Theme>()
                .HasKey(x => x.Id);

            builder.Entity<Theme>()
                .HasIndex(c => new { c.OwnerId, c.Name })
                .IsUnique();

            builder.Entity<Theme>()
                .HasOne(c => c.Owner)
                .WithMany()
                .HasForeignKey(o => o.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Card>()
                .HasIndex(c => new { c.Word, c.ThemeId })
                .IsUnique();

            builder.Entity<Card>()
                .HasOne(c => c.Theme)
                .WithMany()
                .HasForeignKey(c => c.ThemeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserCardStatus>()
                .HasKey(ucs => new { ucs.UserId, ucs.CardId });

            builder.Entity<UserCardStatus>()
                .HasOne(ucs => ucs.User)
                .WithMany()
                .HasForeignKey(ucs => ucs.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserCardStatus>()
                .HasOne(ucs => ucs.Card)
                .WithMany()
                .HasForeignKey(ucs => ucs.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Theme>()
                .HasMany(t => t.ThemeSubscribers)
                .WithMany(u => u.AddedThemes)
                .UsingEntity(j => j.ToTable("UserThemes"));

        }
    }
}