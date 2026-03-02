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

        builder.Entity<Cost>()
            .HasMany(c => c.Tags)
            .WithMany(t => t.Costs);
        builder.Entity<Cost>()
            .Navigation(c => c.Currency).AutoInclude();
        builder.Entity<Cost>()
            .Navigation(c => c.AddedBy).AutoInclude();
        builder.Entity<Cost>()
            .Navigation(c => c.PayedBy).AutoInclude();
        builder.Entity<Cost>()
            .Navigation(c => c.Tags).AutoInclude();
        
        builder.Entity<Currency>()
            .HasKey(c => c.ISONum);

        builder.Entity<IOU>()
            .Navigation(i => i.OwedByUser).AutoInclude();
        builder.Entity<IOU>()
            .Navigation(i => i.OwedToUser).AutoInclude();
        builder.Entity<IOU>()
            .Navigation(i => i.Currency).AutoInclude();

        builder.Entity<Ledger>()
            .HasMany(l => l.Members)
            .WithMany(u => u.MemberOf);
        builder.Entity<Ledger>()
            .HasOne(l => l.Owner)
            .WithMany(u => u.LedgersOwned);
        builder.Entity<Ledger>()
            .Property(l => l.Created)
            .HasField("_created");
        builder.Entity<Ledger>()
            .Property(l => l.LastUpdated)
            .HasField("_updated");
        builder.Entity<Ledger>()
            .Navigation(l => l.Owner).AutoInclude();
        builder.Entity<Ledger>()
            .Navigation(l => l.Shares).AutoInclude();
        builder.Entity<Ledger>()
            .Navigation(l => l.IOUs).AutoInclude();
        builder.Entity<Ledger>()
            .Navigation(l => l.Members).AutoInclude();
        builder.Entity<Ledger>()
            .Navigation(l => l.Costs).AutoInclude();
        builder.Entity<Ledger>()
            .Navigation(l => l.Stakes).AutoInclude();
        builder.Entity<Ledger>()
            .Navigation(l => l.DefaultCurrency).AutoInclude();
        
        builder.Entity<Share>()
            .HasMany(s => s.IncludeTags)
            .WithMany(t => t.IncludedShares);
        builder.Entity<Share>()
            .HasMany(s => s.ExcludeTags)
            .WithMany(t => t.ExcludedShares);
        builder.Entity<Share>()
            .Navigation(s => s.User).AutoInclude();
        builder.Entity<Share>()
            .Navigation(s => s.SingleCost).AutoInclude();
        builder.Entity<Share>()
            .Navigation(s => s.IncludeTags).AutoInclude();
        builder.Entity<Share>()
            .Navigation(s => s.ExcludeTags).AutoInclude();
        builder.Entity<Share>()
            .Navigation(s => s.Currency).AutoInclude();

        builder.Entity<Stake>()
            .Navigation(s => s.Holder).AutoInclude();
        
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
