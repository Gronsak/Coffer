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
    public Guid Id { get; set; }
    public AppUser User { get; set; } = new();
    public Cost? SingleCost { get; set; }
    public List<Tag> IncludeTags { get; set; } = [];
    public List<Tag> ExcludeTags { get; set; } = [];
    public ShareType Type { get; set; } = ShareType.Shares;
    public decimal Size { get; set; } = 0;
    public Currency ShareCurrency { get; set; } = new();
    public bool UpdateShare(Share share)
    {
        if(this.Id != share.Id || this.User != share.User)
            return false;
        
        this.SingleCost = share.SingleCost;
        this.IncludeTags = share.IncludeTags;
        this.ExcludeTags = share.ExcludeTags;
        this.Type = share.Type;
        this.Size = share.Size;
        this.ShareCurrency = share.ShareCurrency;

        return true;
    }
    public override bool Equals(object? obj)
    {
        if(obj is null || obj is not Share)
            return false;
        
        var other = (Share)obj;

        if (Id != other.Id ||
            User != other.User ||
            (SingleCost ?? new()) != (other.SingleCost ?? new()) ||
            IncludeTags.SequenceEqual(other.IncludeTags) ||
            ExcludeTags.SequenceEqual(other.ExcludeTags) ||
            Type != other.Type ||
            Size != other.Size ||
            ShareCurrency != other.ShareCurrency)
            return false;

        return true;
    }
    public static bool operator ==(Share x, Share y){ return x.Equals(y); }
    public static bool operator !=(Share x, Share y){ return x.Equals(y); }
    public override int GetHashCode()
    {
        return Id.GetHashCode()
            ^ User.GetHashCode()
            ^ (SingleCost?.GetHashCode() ?? 0)
            ^ IncludeTags.GetHashCode()
            ^ ExcludeTags.GetHashCode()
            ^ Type.GetHashCode()
            ^ Size.GetHashCode()
            ^ ShareCurrency.GetHashCode();
    }
}