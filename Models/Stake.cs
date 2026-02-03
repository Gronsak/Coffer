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
    public Guid Id { get; set; } = Guid.NewGuid();
    public AppUser Holder { get; set; } = new();
    public decimal AmountStaked { get; set; } = 0;
}