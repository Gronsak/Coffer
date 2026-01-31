using Coffer.Areas.Identity.Data;

namespace Coffer.Models;
public class Debt
{
    public Guid Id { get; set; }
    public AppUser Debtor { get; set; }
    public AppUser Lender { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public decimal SettledAmount { get; set; }
    public bool Settled { get; set; }
}