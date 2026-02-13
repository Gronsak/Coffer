using Coffer.Data;

namespace Coffer.Models;
public class Share
{
    public Share() {}
    public Share(AppUser user, Currency shareCurrency, ShareType type)
    {
        this.User = user;
        this.ShareCurrency = shareCurrency;
        this.Type = type;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public AppUser User { get; set; } = new();
    public Cost? SingleCost { get; set; }
    public List<Tag> IncludeTags { get; set; } = [];
    public List<Tag> ExcludeTags { get; set; } = [];
    public ShareType Type { get; set; } = ShareType.Shares;
    public decimal Modifier { get; set; } = 0;
    public Currency ShareCurrency { get; set; } = new();
    public decimal Split { get; set; } = 0;
    public Currency SplitCurrency { get; set; } = new();
}