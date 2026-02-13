using Coffer.Data;
using Coffer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Coffer.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Cost> Costs { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<IOU> IOUs { get; set; }
    public DbSet<Ledger> Ledgers { get; set; }
    public DbSet<Share> Shares { get; set; }
    public DbSet<Stake> Stakes { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<Ledger>()
            .HasMany(l => l.Members)
            .WithMany(u => u.MemberOf);
        builder.Entity<Ledger>()
            .HasOne(l => l.Owner)
            .WithMany(u => u.LedgersOwned);
        builder.Entity<Tag>()
            .HasMany(t => t.Costs)
            .WithMany(c => c.Tags);
        builder.Entity<Tag>()
            .HasMany(t => t.IncludedShares)
            .WithMany(s => s.IncludeTags);
        builder.Entity<Tag>()
            .HasMany(t => t.ExcludedShares)
            .WithMany(s => s.ExcludeTags);
    }
}
