using Coffer.Areas.Identity.Data;

namespace Coffer.Models;
public class IOU(AppUser owedByUser, AppUser OwedToUser, decimal amount, Currency currency)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public AppUser OwedByUser { get; set; } = owedByUser;
    public AppUser OwedToUser { get; set; } = OwedToUser;
    public decimal Amount { get; set; } = amount;
    public Currency Currency { get; set; } = currency;
    public decimal SettledAmount { get; set; } = 0;
    public bool Settled { get; set; } = false;
}