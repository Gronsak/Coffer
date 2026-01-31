using Coffer.Areas.Identity.Data;

namespace Coffer.Models;
public class Debt(AppUser debtor, AppUser userOwed, decimal amount, Currency currency)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public AppUser Debtor { get; set; } = debtor;
    public AppUser UserOwed { get; set; } = userOwed;
    public decimal Amount { get; set; } = amount;
    public Currency Currency { get; set; } = currency;
    public decimal SettledAmount { get; set; } = 0;
    public bool Settled { get; set; } = false;
}