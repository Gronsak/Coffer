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
    //TODO: more logging!
    public async Task<bool> UpdateLedgerAsync(Ledger updatedLedger)
    {
        if(updatedLedger.Id == default)
        {
            Logger.LogError("Ledger was not retrived, Id was set to GUID default!");
            return false;
        }
        
        using var db = await DbContextFactory.CreateDbContextAsync();

        var oldLedger = await FetchLedgerByIdAsync(updatedLedger.Id, db);

        if(oldLedger is null)
        {
            Logger.LogError("Ledger {updatedLedger.Id} was not found!", updatedLedger.Id);
            return false;
        }

        if(oldLedger.Name != updatedLedger.Name)
            oldLedger.Name = updatedLedger.Name;

        if(oldLedger.Description != updatedLedger.Description)
            oldLedger.Description = updatedLedger.Description;
        
        if(oldLedger.Owner != updatedLedger.Owner)
        {
            var owner = await db.Users.FindAsync(updatedLedger.Owner.Id);
            if(owner is null)
            {
                Logger.LogError("User {updatedLedger.Owner.Id} was not found when changing owner!", updatedLedger.Owner.Id);
                return false;
            }
            oldLedger.Owner = owner;
        }

        try
        {
            oldLedger = await UpdateMembers(oldLedger, updatedLedger.Members, db);
        }
        catch(ArgumentException e)
        {
            Logger.LogError(e, "There was an exception while trying to update Members!");
            return false;
        }

        try
        {
            oldLedger = await UpdateCosts(oldLedger, updatedLedger.Costs, db);
        }
        catch(ArgumentException e)
        {
            Logger.LogError(e, "There was an exception while trying to update Costs!");
            return false;
        }

        // TODO: add logic for Shares
        // TODO: add logic for IOUs (maybe, this should really be handled internally in the ledger and might not be needed)
        // TODO: add logic for Stakes (maybe, this is really handled internally in the ledger and might not be needed)

        try
        {
            await db.SaveChangesAsync();
        }
        catch(Exception e)
        {
            Logger.LogError(e, "There was an error saving ledger to the Database!");
            return false;
        }

        return true;
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
    private static async Task<Ledger> UpdateMembers(Ledger ledger, IEnumerable<AppUser> members, ApplicationDbContext context)
    {
        if(!ledger.Members.SequenceEqual(members)){
            foreach(var member in members)
            {
                // TODO: rewrite with FindAsync ?
                if(!await context.Users.AnyAsync(u => u.Id == member.Id))
                    throw new ArgumentException($"User with Id {member.Id} does not exist!");
                if(!ledger.Members.Any(m => m.Id == member.Id))
                {
                    ledger.AddMember(await context.Users.SingleAsync(u => u.Id == member.Id));
                }
            }
            var membersToRemove = ledger.Members.Where(m => !members.Any(m2 => m2.Id == m.Id));

            if (membersToRemove is not null)
                foreach(var member in membersToRemove)
                {
                    ledger.RemoveMember(member);
                }
        }
        return ledger;
    }
    private static async Task<Ledger> UpdateCosts(Ledger ledger, IEnumerable<Cost> costs, ApplicationDbContext context)
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
                    ledger.AddCost(cost);
                else
                {
                    var dbCost = ledger.Costs.Single(c => c.Id == cost.Id);
                    if(dbCost != cost)
                        dbCost.UpdateCost(cost);
                }
            }
            var costsToRemove = ledger.Costs.Where(c => !costs.Any(c2 => c2.Id == c.Id));

            if (costsToRemove is not null)
                foreach(var cost in costsToRemove)
                {
                    ledger.RemoveCost(cost);
                }
        }
        return ledger;
    }
}