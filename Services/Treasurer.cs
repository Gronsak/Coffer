using System.Security.Claims;
using Coffer.Data;
using Coffer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Coffer.Services;

public class Treasurer(UserManager<AppUser> userManager, IDbContextFactory<ApplicationDbContext> dbContextFactory, ILogger<Treasurer> logger) : ITreasurer
{
    private UserManager<AppUser> UserManager { get; set; } = userManager;
    private IDbContextFactory<ApplicationDbContext> DbContextFactory { get; set; } = dbContextFactory;
    private readonly ILogger<Treasurer> Logger = logger;

    public async Task<Ledger?> CreateLedgerAsync(string name, AppUser owner, Currency defaultCurrency, string description = "", ShareType defaultType = ShareType.Shares)
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
                return null;
            }
        }

        if(user is null)
            return null;
        
        Ledger ledger = new(user, name, currency, description, defaultType);

        await db.AddAsync(ledger);
        var result = await db.SaveChangesAsync();

        if(result>0)
        {
            return ledger;
        }

        return null;
    }

    public Task<bool> DeleteLedgerAsync(Ledger ledger)
    {
        throw new NotImplementedException(); //TODO: implement
    }

    public async Task<List<Currency>?> GetCurrenciesAsync()
    {
        using var db = await DbContextFactory.CreateDbContextAsync();
        var currencies = await db.Currencies.ToListAsync();
        return currencies;
    }

    public async Task<Ledger?> GetLedgerAsync(Guid id)
    {
        using var db = await DbContextFactory.CreateDbContextAsync();

        var ledger = await FetchLedgerByIdAsync(id, db);
        
        if (ledger is null)
            return new();

        return ledger;
    }

    public async Task<Ledger?> GetLedgerAsync(string name)
    {
        using var db = await DbContextFactory.CreateDbContextAsync();

        var ledger = await FetchLedgerByNameAsync(name, db);

        return ledger;
    }

    public async Task<List<Ledger>?> GetLedgersAsync()
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

        return ledgers;
    }

    public async Task<List<Ledger>?> GetLedgersAsync(AppUser owner)
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

        return ledgers;
    }

    public async Task<List<Ledger>?> GetLedgersAsync(ClaimsPrincipal owner)
    {
        string? id = owner.FindFirstValue(ClaimTypes.NameIdentifier);
        if(id is null)
            return null;
        
        return await GetLedgersByOwnerIdAsync(id);
    }

    public async Task<List<Ledger>?> GetLedgersByMemeberAsync(AppUser member)
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
        
        return ledgers;
    }

    public async Task<List<Ledger>?> GetLedgersByMemeberAsync(ClaimsPrincipal member)
    {
        string? id = member.FindFirstValue(ClaimTypes.NameIdentifier);
        if(id is null)
            return null;
        
        return await GetLedgersByMemeberIdAsync(id);
    }

    public async Task<List<Ledger>?> GetLedgersByMemeberIdAsync(string memberId)
    {
        var user = await UserManager.FindByIdAsync(memberId);

        if(user is null)
            return null;

        return await GetLedgersByMemeberAsync(user);
    }

    public async Task<List<Ledger>?> GetLedgersByOwnerIdAsync(string ownerId)
    {
        var user = await UserManager.FindByIdAsync(ownerId);

        if(user is null)
            return null;

        return await GetLedgersAsync(user);
    }
    //TODO: more logging!
    public async Task<Ledger?> UpdateLedgerAsync(Ledger updatedLedger)
    {
        if(updatedLedger.Id == default)
        {
            Logger.LogError("Ledger was not retrived, Id was set to GUID default!");
            return null;
        }
        
        using var db = await DbContextFactory.CreateDbContextAsync();

        var dbLedger = await FetchLedgerByIdAsync(updatedLedger.Id, db);

        if(dbLedger is null)
        {
            Logger.LogError("Ledger {updatedLedger.Id} was not found!", updatedLedger.Id);
            return null;
        }

        if(dbLedger.Name != updatedLedger.Name)
            dbLedger.Name = updatedLedger.Name;

        if(dbLedger.Description != updatedLedger.Description)
            dbLedger.Description = updatedLedger.Description;
        
        if(dbLedger.Owner != updatedLedger.Owner)
        {
            var owner = await db.Users.FindAsync(updatedLedger.Owner.Id);
            if(owner is null)
            {
                Logger.LogError("User {updatedLedger.Owner.Id} was not found when changing owner!", updatedLedger.Owner.Id);
                return null;
            }
            dbLedger.Owner = owner;
        }

        try
        {
            dbLedger = await UpdateMembers(dbLedger, updatedLedger.Members, db, AutoRecalc: false);
        }
        catch(ArgumentException e)
        {
            Logger.LogError(e, "There was an exception while trying to update Members!");
            return null;
        }

        try
        {
            dbLedger = await UpdateCosts(dbLedger, updatedLedger.Costs, db, AutoRecalc: false);
        }
        catch(ArgumentException e)
        {
            Logger.LogError(e, "There was an exception while trying to update Costs!");
            return null;
        }

        // TODO: add logic for Shares
        // TODO: add logic for IOUs (maybe, this should really be handled internally in the ledger and might not be needed)
        // TODO: add logic for Stakes (maybe, this is really handled internally in the ledger and might not be needed)
        try
        {
            dbLedger.RecalculateLedger();
        }
        catch(Exception e)
        {
            Logger.LogError(e, "There was an error recalculating the Ledger!");
            return null;
        }

        try
        {
            await db.SaveChangesAsync();
        }
        catch(Exception e)
        {
            Logger.LogError(e, "There was an error saving ledger to the Database!");
            return null;
        }

        return dbLedger;
    }

    private static async Task<Ledger?> FetchLedgerByIdAsync(Guid id, ApplicationDbContext context)
    {
        var ledger = context.Ledgers
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
            .FirstOrDefault();
        return ledger;
    }
    private static async Task<Ledger?> FetchLedgerByNameAsync(string name, ApplicationDbContext context)
    {
        var ledger = await context.Ledgers
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
    private static async Task<Ledger> UpdateMembers(Ledger ledger, IEnumerable<AppUser> members, ApplicationDbContext context, bool AutoRecalc = true)
    {
        if(!ledger.Members.SequenceEqual(members)){
            foreach(var member in members)
            {
                // TODO: rewrite with FindAsync ?
                if(!await context.Users.AnyAsync(u => u.Id == member.Id))
                    throw new ArgumentException($"User with Id {member.Id} does not exist!");
                if(!ledger.Members.Any(m => m.Id == member.Id))
                {
                    ledger.AddMember(member: await context.Users.SingleAsync(u => u.Id == member.Id), AutoRecalc: AutoRecalc);
                }
            }
            var membersToRemove = ledger.Members.Where(m => !members.Any(m2 => m2.Id == m.Id));

            if (membersToRemove is not null)
                foreach(var member in membersToRemove)
                {
                    ledger.RemoveMember(member: member, AutoRecalc: AutoRecalc);
                }
        }
        return ledger;
    }
    private static async Task<Ledger> UpdateCosts(Ledger ledger, IEnumerable<Cost> costs, ApplicationDbContext context, bool AutoRecalc = true)
    {
        if(!ledger.Costs.SequenceEqual(costs))
        {
            foreach(var cost in costs)
            {
                var payingUser = await context.Users.FindAsync(cost.PayedBy.Id);
                var addingUser = await context.Users.FindAsync(cost.AddedBy.Id);

                if(payingUser is null || addingUser is null)
                    if(payingUser is null)
                        throw new ArgumentException($"User with Id {cost.PayedBy.Id} does not exist!");
                    if(addingUser is null)
                        throw new ArgumentException($"User with Id {cost.AddedBy.Id} does not exist!");

                cost.PayedBy = payingUser;
                cost.AddedBy = payingUser;
                var tags = new List<Tag>();
                foreach(var tag in cost.Tags)
                {
                    var dbTag = await context.Tags.FirstOrDefaultAsync(t => t.Name == tag.Name);
                    if(dbTag is not null)
                        tags.Add(dbTag);
                    else
                        tags.Add(tag);
                }
                cost.Tags = tags;

                if(!ledger.Costs.Any( c => c.Id == cost.Id))
                    ledger.AddCost(cost: cost, AutoRecalc: AutoRecalc);
                else
                {
                    ledger.UpdateCost(cost: cost, AutoRecalc: AutoRecalc);
                }
            }
            var costsToRemove = ledger.Costs.Where(c => !costs.Any(c2 => c2.Id == c.Id));

            if (costsToRemove is not null)
                foreach(var cost in costsToRemove)
                {
                    ledger.RemoveCost(cost: cost, AutoRecalc: AutoRecalc);
                }
        }
        return ledger;
    }
}