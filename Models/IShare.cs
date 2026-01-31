using Coffer.Areas.Identity.Data;

namespace Coffer.Models;
public interface IShare
{
    Guid Id { get; set; }
    Ledger Ledger { get; set; }
    AppUser User { get; set; }
    List<ITag> Tags { get; set; }
    ShareType Type { get; set; }
    decimal Amount { get; set; }
    ICurrency ShareCurrency { get; set; }
    decimal Split { get; set; }
    ICurrency SplitCurrency { get; set; }
}