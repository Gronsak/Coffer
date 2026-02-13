using System.Security.Claims;
using Coffer.Data;
using Coffer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Coffer.Services;

public class Treasurer(UserManager<AppUser> userManager, IDbContextFactory<ApplicationDbContext> dbContextFactory) : ITreasurer
{
    private UserManager<AppUser> UserManager { get; set; } = userManager;
    private IDbContextFactory<ApplicationDbContext> DbContextFactory { get; set; } = dbContextFactory;

    public async Task<bool> CreateLedgerAsync(string name, AppUser owner, Currency defaultCurrency, string description = "", ShareType defaultType = ShareType.Shares)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(owner.UserName);

        using var db = await DbContextFactory.CreateDbContextAsync();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == owner.Id);
        var currency = await db.Currencies.FirstOrDefaultAsync(c => c.ISONum == defaultCurrency.ISONum);

        if(currency is null)
        {
            if(!string.IsNullOrWhiteSpace(defaultCurrency.Name) && defaultCurrency.ISONum != 0 && !string.IsNullOrWhiteSpace(defaultCurrency.ISOName))
            {
                currency = defaultCurrency;
            }
            else
            {
                return false;
            }
        }

        if(user is null)
            return false;
        
        Ledger ledger = new(user, name, currency, description, defaultType);

        await db.AddAsync(ledger);
        var result = await db.SaveChangesAsync();

        if(result>0)
            return true;

        return false;
    }

    public Task<bool> DeleteLedgerAsync(Ledger ledger)
    {
        throw new NotImplementedException(); //TODO: implement
    }

    public async Task<List<Currency>> GetCurrenciesAsync()
    {
        using var db = await DbContextFactory.CreateDbContextAsync();
        return await db.Currencies.ToListAsync();
    }

    public async Task<Ledger?> GetLedgerAsync(Guid id)
    {
        using var db = await DbContextFactory.CreateDbContextAsync();

        var ledger = await db.Ledgers
            .Include(l => l.Costs)
                .ThenInclude(c => c.Tags)
            .Include(l => l.IOUs)
            .Include(l => l.Members)
            .Include(l => l.Shares)
                .ThenInclude(s => s.SingleCost)
            .Include(l => l.Shares)
                .ThenInclude(s => s.IncludeTags)
            .Include(l => l.Shares)
                .ThenInclude(s => s.ExcludeTags)
            .Include(l => l.Stakes)
            .Where(l => l.Id == id)
            .OrderBy(l => l.Name)
            .FirstOrDefaultAsync();

        if (ledger is null)
            return new();

        return ledger;
    }

    public async Task<Ledger?> GetLedgerAsync(string name)
    {
        using var db = await DbContextFactory.CreateDbContextAsync();

        var ledger = await db.Ledgers
            .Include(l => l.Costs)
                .ThenInclude(c => c.Tags)
            .Include(l => l.IOUs)
            .Include(l => l.Members)
            .Include(l => l.Shares)
                .ThenInclude(s => s.SingleCost)
            .Include(l => l.Shares)
                .ThenInclude(s => s.IncludeTags)
            .Include(l => l.Shares)
                .ThenInclude(s => s.ExcludeTags)
            .Include(l => l.Stakes)
            .Where(l => l.Name == name)
            .OrderBy(l => l.Name)
            .FirstOrDefaultAsync();

        return ledger;
    }

    public async Task<List<Ledger>> GetLedgersAsync()
    {
        using var db = await DbContextFactory.CreateDbContextAsync();

        var ledgers = await db.Ledgers
            .Include(l => l.Costs)
                .ThenInclude(c => c.Tags)
            .Include(l => l.IOUs)
            .Include(l => l.Members)
            .Include(l => l.Shares)
                .ThenInclude(s => s.SingleCost)
            .Include(l => l.Shares)
                .ThenInclude(s => s.IncludeTags)
            .Include(l => l.Shares)
                .ThenInclude(s => s.ExcludeTags)
            .Include(l => l.Stakes)
            .OrderBy(l => l.Name)
            .ToListAsync();

        if (ledgers is null)
            return [];

        return ledgers;
    }

    public async Task<List<Ledger>> GetLedgersAsync(AppUser owner)
    {
        using var db = await DbContextFactory.CreateDbContextAsync();

        var ledgers = await db.Ledgers
            .Include(l => l.Costs)
                .ThenInclude(c => c.Tags)
            .Include(l => l.IOUs)
            .Include(l => l.Members)
            .Include(l => l.Shares)
                .ThenInclude(s => s.SingleCost)
            .Include(l => l.Shares)
                .ThenInclude(s => s.IncludeTags)
            .Include(l => l.Shares)
                .ThenInclude(s => s.ExcludeTags)
            .Include(l => l.Stakes)
            .Where(l => l.Owner == owner)
            .OrderBy(l => l.Name)
            .ToListAsync();

        if (ledgers is null)
            return [];

        return ledgers;
    }

    public async Task<List<Ledger>> GetLedgersAsync(ClaimsPrincipal owner)
    {
        string? id = owner.FindFirstValue(ClaimTypes.NameIdentifier);
        if(id is null)
            return [];
        
        return await GetLedgersByOwnerIdAsync(id);
    }

    public async Task<List<Ledger>> GetLedgersByMemeberAsync(AppUser member)
    {
        using var db = await DbContextFactory.CreateDbContextAsync();

        var ledgers = await db.Ledgers
            .Include(l => l.Costs)
                .ThenInclude(c => c.Tags)
            .Include(l => l.IOUs)
            .Include(l => l.Members)
            .Include(l => l.Shares)
                .ThenInclude(s => s.SingleCost)
            .Include(l => l.Shares)
                .ThenInclude(s => s.IncludeTags)
            .Include(l => l.Shares)
                .ThenInclude(s => s.ExcludeTags)
            .Include(l => l.Stakes)
            .Where(l => l.Members.Contains(member))
            .OrderBy(l => l.Name)
            .ToListAsync();

        if (ledgers is null)
            return [];

        return ledgers;
    }

    public async Task<List<Ledger>> GetLedgersByMemeberAsync(ClaimsPrincipal member)
    {
        string? id = member.FindFirstValue(ClaimTypes.NameIdentifier);
        if(id is null)
            return [];
        
        return await GetLedgersByMemeberIdAsync(id);
    }

    public async Task<List<Ledger>> GetLedgersByMemeberIdAsync(string memberId)
    {
        var user = await UserManager.FindByIdAsync(memberId);

        if(user is null)
            return [];

        return await GetLedgersByMemeberAsync(user);
    }

    public async Task<List<Ledger>> GetLedgersByOwnerIdAsync(string ownerId)
    {
        var user = await UserManager.FindByIdAsync(ownerId);

        if(user is null)
            return [];

        return await GetLedgersAsync(user);
    }

    public async Task<bool> UpdateLedgerAsync(Ledger ledger)
    {
        throw new NotImplementedException(); //TODO: implement
    }
}