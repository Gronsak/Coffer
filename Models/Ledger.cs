using Coffer.Data;

namespace Coffer.Models;
public class Ledger
{
    public Ledger() {}
    public Ledger(AppUser owner, string name, Currency defaulCurrency, ShareType defaultType = ShareType.Shares)
    {
        this.Name = name;
        this.Owner = owner;
        this.Members.Add(owner);
        this.DefaultShareType = defaultType;
        this.DefaultCurrency = defaulCurrency;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public AppUser Owner { get; set; } = new();
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime LastUpdated { get; set; } = DateTime.Now;
    public List<Share> Shares { get; set; } = [];
    public List<IOU> IOUs { get; set; } = [];
    public List<AppUser> Members { get; set; } = [];
    public List<Cost> Costs { get; set; } = [];
    public List<Stake> Stakes { get; set; } = [];
    public bool Active { get; set; } = true;
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
    public ShareType DefaultShareType { get; set; } = ShareType.Shares;
    public Currency DefaultCurrency { get; set; } = new();

    public bool CalculateShares()
    {
        throw new NotImplementedException();
    }
    public bool CalculateStakes()
    {
        var stakeHolders = (from cost in Costs
                            select cost.PayedBy).Distinct();
        if(stakeHolders is null)
            return false;

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
                Stakes.Add(new(holder, amount));
            }
        }
        return true;
    }
    public bool CalculateIOUs()
    {
        throw new NotImplementedException();
    }
    public bool AddCost(Cost cost)
    {
        if(!Active||Costs.Contains(cost))
            return false;

        Costs.Add(cost);
        CalculateStakes();
        return true;
    }
    public bool AddShare(Share share)
    {
        if(!Active)
            return false;
        throw new NotImplementedException();
    }
    public bool AddUser(AppUser user)
    {
        if(!Active)
            return false;
        throw new NotImplementedException();
    }
    public bool RemoveCost(Cost cost)
    {
        if(!Active)
            return false;
        throw new NotImplementedException();
    }
    public bool RemoveShare(Share share)
    {
        if(!Active)
            return false;
        throw new NotImplementedException();
    }
    public bool RemoveUser(AppUser user)
    {
        if(!Active)
            return false;
        throw new NotImplementedException();
    }
}
