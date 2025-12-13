using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LEORE.Models;

namespace LEORE.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LEORE.Models.ProductReview> ProductReview { get; set; } = default!;
        public DbSet<LEORE.Models.RefreshToken> RefreshToken { get; set; } = default!;
        public DbSet<LEORE.Models.User> User { get; set; } = default!;
        public DbSet<LEORE.Models.WishList> WishList { get; set; } = default!;
        public DbSet<LEORE.Models.WishListItem> WishListItem { get; set; } = default!;
    }
}
