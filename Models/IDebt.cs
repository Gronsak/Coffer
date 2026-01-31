using Coffer.Areas.Identity.Data;

namespace Coffer.Models;
public interface IDebt
{
    Guid Id { get; set; }
    AppUser Debtor { get; set; }
    AppUser Lender { get; set; }
    decimal Amount { get; set; }
    ICurrency Currency { get; set; }
}