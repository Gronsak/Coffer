using Coffer.Data;

namespace Coffer.Models;
public class Stake(AppUser userWithStake, decimal stake = 0)
{
    public Guid Id { get; set; }
    public AppUser Holder { get; set; } = userWithStake;
    public decimal AmountStaked { get; set; } = stake;
}