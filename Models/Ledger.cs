using System.ComponentModel.DataAnnotations;
using Coffer.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace Coffer.Models;
public class Ledger
{
    public Ledger() {}
    public Ledger(AppUser owner, string name, Currency defaulCurrency, string description = "", ShareType defaultType = ShareType.Shares)
    {
        this.Name = name;
        this.Description = description;
        this.Owner = owner;
        this._members.Add(owner);
        this.DefaultShareType = defaultType;
        this.DefaultCurrency = defaulCurrency;
    }
    public Guid Id { get; set; }
    [Required]
    public string Name 
    { 
        get; 
        set
        {
            field = value;
            _updated = DateTime.Now;
        }
    } = "";
    public string Description 
    {
        get; 
        set
        {
            field = value;
            _updated = DateTime.Now;
        }
    } = "";
    [Required]
    public AppUser Owner 
    {
        get; 
        set
        {
            field = value;
            _updated = DateTime.Now;
        }
    } = new();
    public DateTime Created { get { return _created; } }
    public DateTime LastUpdated { get { return _updated; } }
    public IEnumerable<Share> Shares
    {
        get { return _shares.AsEnumerable(); } 
    }
    public IEnumerable<IOU> IOUs
    {
        get { return _ious.AsEnumerable(); } 
    }
    public IEnumerable<AppUser> Members
    {
        get { return _members.AsEnumerable(); } 
    }
    public IEnumerable<Cost> Costs
    {
        get { return _costs.AsEnumerable(); }
    }
    public IEnumerable<Stake> Stakes
    {
        get { return _stakes.AsEnumerable(); }
    }
    public bool Active 
    {
        get; 
        set
        {
            field = value;
            _updated = DateTime.Now;
        }
    } = true;
    public ShareType DefaultShareType 
    {
        get; 
        set
        {
            field = value;
            _updated = DateTime.Now;
        }
    } = ShareType.Shares;
    public Currency DefaultCurrency 
    {
        get; 
        set
        {
            field = value;
            _updated = DateTime.Now;
        }
    } = new();
    public List<Tag> AllTags
    {
        get => GetAllTags();
    }
    public decimal CostsSum
    {
        get => (from cost in Costs select cost.Amount).Sum(); 
    }
    public bool AllIOUsSettled
    {
        get => (from iou in IOUs
                where iou.Settled == false
                select iou.Settled).FirstOrDefault(true);
    }
    private DateTime _created = DateTime.Now;
    private DateTime _updated = DateTime.Now;
    private List<Share> _shares = [];
    private List<IOU> _ious = [];
    private List<AppUser> _members = [];
    private List<Cost> _costs = [];
    private List<Stake> _stakes = [];

    private void CalculateShares()
    {
        throw new NotImplementedException();
    }
    private void CalculateStakes()
    {
        var stakeHolders = (from cost in Costs
                            select cost.PayedBy).Distinct();
        if(stakeHolders is null)
            return;

        foreach (var holder in stakeHolders)
        {
            var amount =   (from cost in Costs
                            where cost.PayedBy == holder
                            select cost.Amount).Sum();

            if(Stakes.Any(stake => stake.Holder == holder))
            {
                var stake = (from s in Stakes
                             where s.Holder == holder
                             select s).First();
                stake.AmountStaked = amount;
            }
            else
            {
                _stakes.Add(new(holder, amount));
            }
        }
    }
    private void CalculateIOUs()
    {
        throw new NotImplementedException();
    }
    public void RecalculateLedger()
    {
        CalculateStakes();
        // CalculateShares();
        // CalculateIOUs();
        this._updated = DateTime.Now;
    }
    public bool AddCost(Cost cost)
    {
        if(!Active||_costs.Contains(cost))
            return false;

        _costs.Add(cost);
        RecalculateLedger();
        return true;
    }
    public bool UpdateCost(Cost cost)
    {
        if(!Active||!_costs.Any(c => c.Id == cost.Id))
            return false;
        
        var existingCost = _costs.Single(c => c.Id == cost.Id);
        existingCost.UpdateCost(cost);
        RecalculateLedger();

        return true;
    }
    public bool RemoveCost(Cost cost)
    {
        if(!Active||!_costs.Any(c => c.Id == cost.Id))
            return false;
        
        var success = _costs.Remove(cost);
        if (success)
        {
            RecalculateLedger();
            return success;
        }
        else
            return success;
    }
    public bool AddShare(Share share)
    {
        if(!Active||_shares.Any(s => s.Id == share.Id))
            return false;
        _shares.Add(share);
        RecalculateLedger();
        return true;
    }
    public bool UpdateShare(Share share)
    {
        if(!Active||!_shares.Any(s => s.Id == share.Id))
            return false;
        
        var existingShare = _shares.Single(s => s.Id == share.Id);
        existingShare.UpdateShare(share);
        RecalculateLedger();

        return true;
    }
    public bool RemoveShare(Share share)
    {
        if(!Active || !_shares.Any(s => s.Id == share.Id) || _shares.Count(s => s.User == share.User) <= 1)
            return false;
        
        var success = _shares.Remove(share);
        if (success)
        {
            RecalculateLedger();
            return success;
        }
        else
            return success;
    }
    public bool AddMember(AppUser member)
    {
        if(!Active||_members.Any(m => m.Id == member.Id))
            return false;

        _members.Add(member);
        RecalculateLedger();
        return true;
    }
    public bool RemoveMember(AppUser member)
    {
        if(!Active||!_members.Any(m => m.Id == member.Id))
            return false;
        
        var success = _members.Remove(member);
        if (success)
        {
            RecalculateLedger();
            return success;
        }
        else
            return success;
    }
    private List<Tag> GetAllTags()
    {
        var costTags = Costs.SelectMany(c => c.Tags).Distinct() ?? [];
        var shareIncludeTags = Shares.SelectMany(s => s.IncludeTags).Distinct() ?? [];
        var shareExcludeTags = Shares.SelectMany(s => s.ExcludeTags).Distinct() ?? [];
        
        var allUniqueTags = costTags.Union(shareIncludeTags)
                                    .Union(shareExcludeTags)
                                    .ToList();

        return allUniqueTags;
    }
}
