using Coffer.Areas.Identity.Data;

namespace Coffer.Models;
public class Share
{
    public Guid Id { get; set; }
    public Ledger Ledger { get; set; }
    public AppUser User { get; set; }
    public List<Tag> Tags { get; set; }
    public ShareType Type { get; set; }
    public decimal Amount { get; set; }
    public Currency ShareCurrency { get; set; }
    public decimal Split { get; set; }
    public Currency SplitCurrency { get; set; }
}