using Coffer.Data;

namespace Coffer.Models;
public class Share(AppUser user, Currency shareCurrency, ShareType type)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public AppUser User { get; set; } = user;
    public List<Tag> Tags { get; set; } = new List<Tag>();
    public ShareType Type { get; set; } = type;
    public decimal Modifier { get; set; } = 0;
    public Currency ShareCurrency { get; set; } = shareCurrency;
    public decimal Split { get; set; } = 0;
    public Currency SplitCurrency { get; set; } = shareCurrency;
}