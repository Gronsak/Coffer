using Coffer.Data;

namespace Coffer.Models;
public class Stake
{
    public Stake() {}
    public Stake(AppUser userWithStake, decimal stake = 0)
    {
        this.Holder = userWithStake;
        this.AmountStaked = stake;
    }
    public Guid Id { get; set; }
    public AppUser Holder { get; set; } = new();
    public decimal AmountStaked { get; set; } = 0;
    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Stake)
            return false;
        var other = (Stake)obj;
        if (Id != other.Id ||
            Holder != other.Holder ||
            AmountStaked != other.AmountStaked)
            return false;
        
        return true;
    }
    public static bool operator ==(Stake x, Stake y){ return x.Equals(y); }
    public static bool operator !=(Stake x, Stake y){ return x.Equals(y); }
    public override int GetHashCode()
    {
        return Id.GetHashCode()
            ^ Holder.GetHashCode()
            ^ AmountStaked.GetHashCode();
    }
}